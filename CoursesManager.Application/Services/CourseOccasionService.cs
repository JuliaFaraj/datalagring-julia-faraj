using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Domain.Entities;
using ErrorOr;

namespace CoursesManager.Application.Services;

public class CourseOccasionService(
    ICourseRepository courseRepo,
    ITeacherRepository teacherRepo,
    ICourseOccasionRepository occasionRepo)
{
    private readonly ICourseRepository _courseRepo = courseRepo;
    private readonly ITeacherRepository _teacherRepo = teacherRepo;
    private readonly ICourseOccasionRepository _occasionRepo = occasionRepo;

    public async Task<ErrorOr<CourseOccasionDto>> CreateAsync(CreateCourseOccasionDto dto, CancellationToken ct = default)
    {
        var exists = await _occasionRepo.ExistsAsync(x => x.OccasionCode == dto.OccasionCode, ct);
        if (exists)
            return Error.Conflict("Occasions.Conflict", $"Occasion '{dto.OccasionCode}' already exists.");

        var course = await _courseRepo.GetOneAsync(x => x.CourseCode == dto.CourseCode, ct: ct);
        if (course is null)
            return Error.NotFound("Courses.NotFound", $"Course '{dto.CourseCode}' not found.");

        var teacher = await _teacherRepo.GetOneAsync(x => x.TeacherCode == dto.TeacherCode, ct: ct);
        if (teacher is null)
            return Error.NotFound("Teachers.NotFound", $"Teacher '{dto.TeacherCode}' not found.");

        var entity = new CourseOccasionEntity
        {
            OccasionCode = dto.OccasionCode,
            CourseId = course.Id,
            TeacherId = teacher.Id,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Location = dto.Location,
            UpdatedAt = DateTime.UtcNow
        };

        var saved = await _occasionRepo.CreateAsync(entity, ct);

        return new CourseOccasionDto(saved.Id, saved.OccasionCode, course.CourseCode, teacher.TeacherCode,
            saved.StartDate, saved.EndDate, saved.Location,
            saved.CreatedAt, saved.UpdatedAt, saved.RowVersion);
    }

    public Task<IReadOnlyList<CourseOccasionDto>> GetAllAsync(CancellationToken ct = default)
        => _occasionRepo.GetAllAsync(
            select: o => new CourseOccasionDto(
                o.Id,
                o.OccasionCode,
                o.Course.CourseCode,
                o.Teacher.TeacherCode,
                o.StartDate,
                o.EndDate,
                o.Location,
                o.CreatedAt,
                o.UpdatedAt,
                o.RowVersion
            ),
            orderBy: q => q.OrderByDescending(x => x.CreatedAt),
            ct: ct);

    public async Task<ErrorOr<CourseOccasionDto>> UpdateAsync(string occasionCode, UpdateCourseOccasionDto dto, CancellationToken ct = default)
    {
        var occasion = await _occasionRepo.GetOneAsync(x => x.OccasionCode == occasionCode, tracking: true, ct: ct);
        if (occasion is null)
            return Error.NotFound("Occasions.NotFound", $"Occasion '{occasionCode}' not found.");

        if (!occasion.RowVersion.SequenceEqual(dto.RowVersion))
            return Error.Conflict("Occasions.Conflict", "Updated by another user. Try again.");

        var teacher = await _teacherRepo.GetOneAsync(x => x.TeacherCode == dto.TeacherCode, ct: ct);
        if (teacher is null)
            return Error.NotFound("Teachers.NotFound", $"Teacher '{dto.TeacherCode}' not found.");

        occasion.TeacherId = teacher.Id;
        occasion.StartDate = dto.StartDate;
        occasion.EndDate = dto.EndDate;
        occasion.Location = dto.Location;
        occasion.UpdatedAt = DateTime.UtcNow;

        await _occasionRepo.SaveChangesAsync(ct);

        // här kan navigationer vara null om de inte är laddade; för G kan vi returnera med teacherCode från teacher
        return new CourseOccasionDto(
            occasion.Id,
            occasion.OccasionCode,
            occasion.Course.CourseCode,
            teacher.TeacherCode,
            occasion.StartDate,
            occasion.EndDate,
            occasion.Location,
            occasion.CreatedAt,
            occasion.UpdatedAt,
            occasion.RowVersion);
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(string occasionCode, CancellationToken ct = default)
    {
        var occasion = await _occasionRepo.GetOneAsync(x => x.OccasionCode == occasionCode, tracking: true, ct: ct);
        if (occasion is null)
            return Error.NotFound("Occasions.NotFound", $"Occasion '{occasionCode}' not found.");

        _occasionRepo.Remove(occasion);
        await _occasionRepo.SaveChangesAsync(ct);

        return Result.Deleted;
    }
}

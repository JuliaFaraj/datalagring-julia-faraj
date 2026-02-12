using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;
using ErrorOr;

namespace CoursesManager.Application.Services;

public class CourseService(ICourseRepository courseRepository)
{
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<ErrorOr<CourseDto>> CreateCourseAsync(CreateCourseDto dto, CancellationToken ct = default)
    {
        var exists = await _courseRepository.ExistsAsync(x => x.CourseCode == dto.CourseCode, ct);
        if (exists)
            return Error.Conflict("Courses.Conflict", $"Course with '{dto.CourseCode}' already exists.");

        var entity = new CourseEntity
        {
            CourseCode = dto.CourseCode,
            Title = dto.Title,
            Description = dto.Description
        };

        var saved = await _courseRepository.CreateAsync(entity, ct);
        return CourseMapper.ToCourseDto(saved);
    }

    public async Task<IReadOnlyList<CourseDto>> GetAllCoursesAsync(CancellationToken ct = default)
    {
        return await _courseRepository.GetAllAsync(
            select: c => new CourseDto
            {
                Id = c.Id,
                CourseCode = c.CourseCode,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                RowVersion = c.RowVersion
            },
            orderBy: q => q.OrderByDescending(x => x.CreatedAt),
            ct: ct
        );
    }

    public async Task<ErrorOr<CourseDto>> UpdateCourseAsync(string courseCode, UpdateCourseDto dto, CancellationToken ct = default)
    {
        // tracking:true eftersom vi ska ändra entity och spara
        var course = await _courseRepository.GetOneAsync(
            x => x.CourseCode == courseCode,
            tracking: true,
            ct: ct
        );

        if (course is null)
            return Error.NotFound("Courses.NotFound", $"Course with '{courseCode}' was not found.");

        // Optimistic concurrency check
        if (!course.RowVersion.SequenceEqual(dto.RowVersion))
            return Error.Conflict("Courses.Conflict", "Updated by another user. Try again.");

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.SaveChangesAsync(ct);
        return CourseMapper.ToCourseDto(course);
    }

    public async Task<ErrorOr<Deleted>> DeleteCourseAsync(string courseCode, CancellationToken ct = default)
    {
        var course = await _courseRepository.GetOneAsync(
            x => x.CourseCode == courseCode,
            tracking: true,
            ct: ct
        );

        if (course is null)
            return Error.NotFound("Courses.NotFound", $"Course with '{courseCode}' was not found.");

        _courseRepository.Remove(course);
        await _courseRepository.SaveChangesAsync(ct);

        return Result.Deleted;
    }
}

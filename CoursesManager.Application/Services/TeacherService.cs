using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Domain.Entities;
using ErrorOr;

namespace CoursesManager.Application.Services;

public class TeacherService(ITeacherRepository teacherRepository)
{
    private readonly ITeacherRepository _teacherRepository = teacherRepository;

    public async Task<ErrorOr<TeacherDto>> CreateAsync(CreateTeacherDto dto, CancellationToken ct = default)
    {
        var exists = await _teacherRepository.ExistsAsync(
            x => x.TeacherCode == dto.TeacherCode,
            ct);

        if (exists)
            return Error.Conflict("Teachers.Conflict",
                $"Teacher with '{dto.TeacherCode}' already exists.");

        var entity = new TeacherEntity
        {
            TeacherCode = dto.TeacherCode,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UpdatedAt = DateTime.UtcNow
        };

        var saved = await _teacherRepository.CreateAsync(entity, ct);

        return new TeacherDto(
            saved.Id,
            saved.TeacherCode,
            saved.FirstName,
            saved.LastName,
            saved.Email,
            saved.CreatedAt,
            saved.UpdatedAt,
            saved.RowVersion
        );
    }

    public Task<IReadOnlyList<TeacherDto>> GetAllAsync(CancellationToken ct = default)
        => _teacherRepository.GetAllAsync(
            select: t => new TeacherDto(
                t.Id,
                t.TeacherCode,
                t.FirstName,
                t.LastName,
                t.Email,
                t.CreatedAt,
                t.UpdatedAt,
                t.RowVersion
            ),
            orderBy: q => q.OrderByDescending(x => x.CreatedAt),
            ct: ct
        );

    public async Task<ErrorOr<TeacherDto>> UpdateAsync(string teacherCode, UpdateTeacherDto dto, CancellationToken ct = default)
    {
        var teacher = await _teacherRepository.GetOneAsync(
            x => x.TeacherCode == teacherCode,
            tracking: true,
            ct: ct);

        if (teacher is null)
            return Error.NotFound("Teachers.NotFound",
                $"Teacher with '{teacherCode}' was not found.");

        // Optimistic concurrency check
        if (!teacher.RowVersion.SequenceEqual(dto.RowVersion))
            return Error.Conflict("Teachers.Conflict",
                "Updated by another user. Try again.");

        teacher.FirstName = dto.FirstName;
        teacher.LastName = dto.LastName;
        teacher.Email = dto.Email;
        teacher.UpdatedAt = DateTime.UtcNow;

        await _teacherRepository.SaveChangesAsync(ct);

        return new TeacherDto(
            teacher.Id,
            teacher.TeacherCode,
            teacher.FirstName,
            teacher.LastName,
            teacher.Email,
            teacher.CreatedAt,
            teacher.UpdatedAt,
            teacher.RowVersion
        );
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(string teacherCode, CancellationToken ct = default)
    {
        var teacher = await _teacherRepository.GetOneAsync(
            x => x.TeacherCode == teacherCode,
            tracking: true,
            ct: ct);

        if (teacher is null)
            return Error.NotFound("Teachers.NotFound",
                $"Teacher with '{teacherCode}' was not found.");

        _teacherRepository.Remove(teacher);
        await _teacherRepository.SaveChangesAsync(ct);

        return Result.Deleted;
    }
}

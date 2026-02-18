using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Domain.Entities;
using ErrorOr;

namespace CoursesManager.Application.Services;

public class EnrollmentService(
    IEnrollmentRepository enrollmentRepo,
    IParticipantRepository participantRepo,
    ICourseOccasionRepository occasionRepo)
{
    private readonly IEnrollmentRepository _enrollmentRepo = enrollmentRepo;
    private readonly IParticipantRepository _participantRepo = participantRepo;
    private readonly ICourseOccasionRepository _occasionRepo = occasionRepo;

    public async Task<ErrorOr<EnrollmentDto>> CreateAsync(CreateEnrollmentDto dto, CancellationToken ct = default)
    {
        var participant = await _participantRepo.GetOneAsync(
            x => x.ParticipantCode == dto.ParticipantCode,
            ct: ct);

        if (participant is null)
            return Error.NotFound("Participants.NotFound",
                $"Participant '{dto.ParticipantCode}' not found.");

        var occasion = await _occasionRepo.GetOneAsync(
            x => x.OccasionCode == dto.OccasionCode,
            ct: ct);

        if (occasion is null)
            return Error.NotFound("Occasions.NotFound",
                $"Occasion '{dto.OccasionCode}' not found.");

        var exists = await _enrollmentRepo.ExistsAsync(
            x => x.ParticipantId == participant.Id &&
                 x.CourseOccasionId == occasion.Id,
            ct);

        if (exists)
            return Error.Conflict("Enrollments.Conflict",
                "Participant is already enrolled on this occasion.");

        var entity = new EnrollmentEntity
        {
            ParticipantId = participant.Id,
            CourseOccasionId = occasion.Id,
            EnrolledAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var saved = await _enrollmentRepo.CreateAsync(entity, ct);

        return new EnrollmentDto(
            saved.Id,
            participant.ParticipantCode,
            occasion.OccasionCode,
            saved.EnrolledAt,
            saved.RowVersion
        );
    }

    public Task<IReadOnlyList<EnrollmentDto>> GetAllAsync(CancellationToken ct = default)
        => _enrollmentRepo.GetAllAsync(
            select: e => new EnrollmentDto(
                e.Id,
                e.Participant.ParticipantCode,
                e.CourseOccasion.OccasionCode,
                e.EnrolledAt,
                e.RowVersion
            ),
            orderBy: q => q.OrderByDescending(x => x.CreatedAt),
            ct: ct);

    public async Task<ErrorOr<Deleted>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var enrollment = await _enrollmentRepo.GetOneAsync(
            x => x.Id == id,
            tracking: true,
            ct: ct);

        if (enrollment is null)
            return Error.NotFound("Enrollments.NotFound",
                $"Enrollment with id '{id}' not found.");

        _enrollmentRepo.Remove(enrollment);
        await _enrollmentRepo.SaveChangesAsync(ct);

        return Result.Deleted;
    }
}

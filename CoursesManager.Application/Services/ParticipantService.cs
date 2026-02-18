using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Domain.Entities;
using ErrorOr;

namespace CoursesManager.Application.Services;

public class ParticipantService(IParticipantRepository repo)
{
    private readonly IParticipantRepository _repo = repo;

    public async Task<ErrorOr<ParticipantDto>> CreateAsync(CreateParticipantDto dto, CancellationToken ct = default)
    {
        var exists = await _repo.ExistsAsync(x => x.ParticipantCode == dto.ParticipantCode, ct);
        if (exists)
            return Error.Conflict("Participants.Conflict", $"Participant '{dto.ParticipantCode}' already exists.");

        var entity = new ParticipantEntity
        {
            ParticipantCode = dto.ParticipantCode,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UpdatedAt = DateTime.UtcNow
        };

        var saved = await _repo.CreateAsync(entity, ct);

        return new ParticipantDto(saved.Id, saved.ParticipantCode, saved.FirstName, saved.LastName, saved.Email,
            saved.CreatedAt, saved.UpdatedAt, saved.RowVersion);
    }

    public Task<IReadOnlyList<ParticipantDto>> GetAllAsync(CancellationToken ct = default)
        => _repo.GetAllAsync(
            select: p => new ParticipantDto(p.Id, p.ParticipantCode, p.FirstName, p.LastName, p.Email,
                p.CreatedAt, p.UpdatedAt, p.RowVersion),
            orderBy: q => q.OrderByDescending(x => x.CreatedAt),
            ct: ct);

    public async Task<ErrorOr<ParticipantDto>> UpdateAsync(string participantCode, UpdateParticipantDto dto, CancellationToken ct = default)
    {
        var p = await _repo.GetOneAsync(x => x.ParticipantCode == participantCode, tracking: true, ct: ct);
        if (p is null)
            return Error.NotFound("Participants.NotFound", $"Participant '{participantCode}' not found.");

        if (!p.RowVersion.SequenceEqual(dto.RowVersion))
            return Error.Conflict("Participants.Conflict", "Updated by another user. Try again.");

        p.FirstName = dto.FirstName;
        p.LastName = dto.LastName;
        p.Email = dto.Email;
        p.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync(ct);

        return new ParticipantDto(p.Id, p.ParticipantCode, p.FirstName, p.LastName, p.Email,
            p.CreatedAt, p.UpdatedAt, p.RowVersion);
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(string participantCode, CancellationToken ct = default)
    {
        var p = await _repo.GetOneAsync(x => x.ParticipantCode == participantCode, tracking: true, ct: ct);
        if (p is null)
            return Error.NotFound("Participants.NotFound", $"Participant '{participantCode}' not found.");

        _repo.Remove(p);
        await _repo.SaveChangesAsync(ct);

        return Result.Deleted;
    }
}

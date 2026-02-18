namespace CoursesManager.Application.Dtos;

public record ParticipantDto(
    int Id,
    string ParticipantCode,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    byte[] RowVersion
);

public record CreateParticipantDto(
    string ParticipantCode,
    string FirstName,
    string LastName,
    string Email
);

public record UpdateParticipantDto(
    string FirstName,
    string LastName,
    string Email,
    byte[] RowVersion
);

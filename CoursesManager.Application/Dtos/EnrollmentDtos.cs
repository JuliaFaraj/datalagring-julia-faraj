namespace CoursesManager.Application.Dtos;

public record EnrollmentDto(
    int Id,
    string ParticipantCode,
    string OccasionCode,
    DateTime EnrolledAt,
    byte[] RowVersion
);

public record CreateEnrollmentDto(
    string ParticipantCode,
    string OccasionCode
);

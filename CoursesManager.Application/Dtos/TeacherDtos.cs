namespace CoursesManager.Application.Dtos;

public record TeacherDto(
    int Id,
    string TeacherCode,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    byte[] RowVersion
);

public record CreateTeacherDto(
    string TeacherCode,
    string FirstName,
    string LastName,
    string Email
);

public record UpdateTeacherDto(
    string FirstName,
    string LastName,
    string Email,
    byte[] RowVersion
);

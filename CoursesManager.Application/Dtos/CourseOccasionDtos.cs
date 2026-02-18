namespace CoursesManager.Application.Dtos;

public record CourseOccasionDto(
    int Id,
    string OccasionCode,
    string CourseCode,
    string TeacherCode,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    byte[] RowVersion
);

public record CreateCourseOccasionDto(
    string OccasionCode,
    string CourseCode,
    string TeacherCode,
    DateTime StartDate,
    DateTime EndDate,
    string Location
);

public record UpdateCourseOccasionDto(
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string TeacherCode,
    byte[] RowVersion
);

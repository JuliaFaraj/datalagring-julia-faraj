namespace CoursesManager.Domain.Entities;

public class CourseOccasionEntity
{
    public int Id { get; set; }
    public string OccasionCode { get; set; } = null!; // typ “NET101-2026SPRING”

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = null!;

    // FK -> Course
    public int CourseId { get; set; }
    public CourseEntity Course { get; set; } = null!;

    // FK -> Teacher
    public int TeacherId { get; set; }
    public TeacherEntity Teacher { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = null!;

    public ICollection<EnrollmentEntity> Enrollments { get; set; } = new List<EnrollmentEntity>();
}

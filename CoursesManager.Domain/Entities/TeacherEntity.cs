namespace CoursesManager.Domain.Entities;

public class TeacherEntity
{
    public int Id { get; set; }
    public string TeacherCode { get; set; } = null!; 
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = null!;

    public ICollection<CourseOccasionEntity> CourseOccasions { get; set; } = new List<CourseOccasionEntity>();
}

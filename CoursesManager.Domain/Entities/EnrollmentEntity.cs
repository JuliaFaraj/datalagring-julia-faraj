namespace CoursesManager.Domain.Entities;

public class EnrollmentEntity
{
    public int Id { get; set; }

    public int ParticipantId { get; set; }
    public ParticipantEntity Participant { get; set; } = null!;

    public int CourseOccasionId { get; set; }
    public CourseOccasionEntity CourseOccasion { get; set; } = null!;

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = null!;
}

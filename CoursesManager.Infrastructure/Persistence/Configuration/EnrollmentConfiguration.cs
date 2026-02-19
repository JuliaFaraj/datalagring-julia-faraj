using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configuration;

public class EnrollmentConfiguration : IEntityTypeConfiguration<EnrollmentEntity>
{
    public void Configure(EntityTypeBuilder<EnrollmentEntity> builder)
    {
        builder.ToTable("Enrollments");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Participant)
            .WithMany(p => p.Enrollments)
            .HasForeignKey(x => x.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CourseOccasion)
            .WithMany(o => o.Enrollments)
            .HasForeignKey(x => x.CourseOccasionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ParticipantId, x.CourseOccasionId }).IsUnique();

        builder.Property(x => x.EnrolledAt).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}

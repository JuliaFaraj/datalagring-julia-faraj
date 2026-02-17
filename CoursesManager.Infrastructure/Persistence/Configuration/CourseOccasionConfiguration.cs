using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configuration;

public class CourseOccasionConfiguration : IEntityTypeConfiguration<CourseOccasionEntity>
{
    public void Configure(EntityTypeBuilder<CourseOccasionEntity> builder)
    {
        builder.ToTable("CourseOccasions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OccasionCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.OccasionCode).IsUnique();

        builder.Property(x => x.Location).IsRequired().HasMaxLength(200);

        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();

        builder.HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Teacher)
            .WithMany(t => t.CourseOccasions)
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}

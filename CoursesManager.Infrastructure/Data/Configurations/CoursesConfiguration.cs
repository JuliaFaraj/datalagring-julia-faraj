using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<CourseEntity>
{
    public void Configure(EntityTypeBuilder<CourseEntity> builder)
    {
        builder.HasKey(e => e.CourseCode);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()").HasColumnType("datetime2");

        builder.Property(e => e.UpdatedAt).HasColumnType("datetime2");

        builder.Property(e => e.RowVersion).IsRowVersion();

    }
}


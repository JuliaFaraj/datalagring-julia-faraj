using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CoursesManager.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CourseEntity> Courses => Set<CourseEntity>();

    public DbSet<TeacherEntity> Teachers => Set<TeacherEntity>();
    public DbSet<ParticipantEntity> Participants => Set<ParticipantEntity>();
    public DbSet<CourseOccasionEntity> CourseOccasions => Set<CourseOccasionEntity>();
    public DbSet<EnrollmentEntity> Enrollments => Set<EnrollmentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new TeacherConfiguration());
        modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
        modelBuilder.ApplyConfiguration(new CourseOccasionConfiguration());
        modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());

    }
}


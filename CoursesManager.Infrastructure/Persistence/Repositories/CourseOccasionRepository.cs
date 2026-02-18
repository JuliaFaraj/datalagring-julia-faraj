using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Persistence;
using CoursesManager.Infrastructure.Persistence.Repositories;

namespace CoursesManager.Infrastructure.Repositories;

public class CourseOccasionRepository(AppDbContext context)
    : BaseRepository<CourseOccasionEntity>(context), ICourseOccasionRepository
{
}

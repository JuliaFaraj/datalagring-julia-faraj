using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Persistence;
using CoursesManager.Infrastructure.Persistence.Repositories;

namespace CoursesManager.Infrastructure.Repositories;

public class EnrollmentRepository(AppDbContext context)
    : BaseRepository<EnrollmentEntity>(context), IEnrollmentRepository
{
}

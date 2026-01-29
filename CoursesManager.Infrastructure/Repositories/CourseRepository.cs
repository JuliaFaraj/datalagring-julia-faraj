using CoursesManager.Domain.Entities;
using CoursesManager.Domain.Interfaces;
using CoursesManager.Infrastructure.Data;
using System.Threading.Tasks;

namespace CoursesManager.Infrastructure.Repositories;

public class CourseRepository(ApplicationDbContext context) : BaseRepository<CourseEntity>(context), ICourseRepository
{

}


using CoursesManager.Application.Dtos;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Mappers;

public class CourseMapper
{
    public static CourseDto ToCourseDto(CourseEntity entity) => new()
    {

        Id = entity.Id,
        CourseCode = entity.CourseCode,
        Title = entity.Title,
        Description = entity.Description,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt,
    };

}



using CoursesManager.Application.Dtos;
using CoursesManager.Application.Helpers;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;
using CoursesManager.Domain.Interfaces;

namespace CoursesManager.Application.Services;

public class CourseService(ICourseRepository courseRepositry)
{
    private readonly ICourseRepository _courseRepositry = courseRepositry;

    public async Task<ResponseResult<CourseDto>> CreateCourseAsync(CreateCourseDto dto)
    {
        if (await _courseRepositry.ExistsAsync(x => x.CourseCode == dto.CourseCode))
            return ResponseResult<CourseDto>.Conflict($"Course with code {dto.CourseCode} already exists");

        var savedCourse = await _courseRepositry.CreateAsync(new CourseEntity { CourseCode = dto.CourseCode, Title = dto.Title, Description = dto.Description });
        return ResponseResult<CourseDto>.OK(CourseMapper.ToCourseDto(savedCourse));
    }



    public async Task<ResponseResult<IEnumerable<CourseDto>>> GetAllCoursesAsync()
    {
        var courses = await _courseRepositry.GetAllAsync();
        return ResponseResult<IEnumerable<CourseDto>>.OK(courses.Select(c => new CourseDto
        {
            CourseCode = c.CourseCode,
            Title = c.Title,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,

        }));
    }

}

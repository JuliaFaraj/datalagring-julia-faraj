

namespace CoursesManager.Application.Dtos;

public class CourseDto
{
    public int Id { get; set; }
    public string? CourseCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = [];

}



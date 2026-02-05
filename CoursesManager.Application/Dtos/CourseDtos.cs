using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesManager.Application.Dtos;

public class CourseDto
{
    public string? CourseCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; } = [];

}



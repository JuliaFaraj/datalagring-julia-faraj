using CoursesManager.Application.Services;
using CoursesManager.Domain.Interfaces;
using CoursesManager.Infrastructure.Data;
using CoursesManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using CoursesManager.Application.Helpers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CourseService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("CoursesManager API is running"));

app.MapGet("/courses", async (CourseService service) =>
{
    var result = await service.GetAllCoursesAsync();

    return result.Status switch
    {
        ResultStatus.Ok => Results.Ok(result.Data),
        ResultStatus.NotFound => Results.NotFound(result.Message),
        ResultStatus.Conflict => Results.Conflict(result.Message),
        ResultStatus.Badrequest => Results.BadRequest(result.Message),
        _ => Results.BadRequest(result.Message)
    };
});


app.Run();


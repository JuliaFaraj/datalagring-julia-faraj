using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Application.Services;
using CoursesManager.Infrastructure.Persistence;
using CoursesManager.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI – repositories + services
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CourseService>();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("CoursesManager API is running"));

app.MapGet("/courses", async (CourseService service) =>
{
    var result = await service.GetAllCoursesAsync();
    return Results.Ok(result);
});


app.MapGet("/courses/{courseCode}", async (string courseCode, CourseService service) =>
{
    var result = await service.GetCourseByCodeAsync(courseCode);

    return result.Match(
        course => Results.Ok(course),
        errors => Results.NotFound(errors)
    );
});



app.MapPost("/courses", async (CourseService service, CreateCourseDto dto) =>
{
    var result = await service.CreateCourseAsync(dto);

    return result.Match(
        created => Results.Created($"/courses/{created.CourseCode}", created),
        errors => Results.BadRequest(errors)
    );
});

app.MapPut("/courses/{courseCode}", async (string courseCode, CourseService service, UpdateCourseDto dto) =>
{
    var result = await service.UpdateCourseAsync(courseCode, dto);

    return result.Match(
        updated => Results.Ok(updated),
        errors => Results.NotFound(errors)
    );
});

app.MapDelete("/courses/{courseCode}", async (string courseCode, CourseService service) =>
{
    var result = await service.DeleteCourseAsync(courseCode);

    return result.Match(
        _ => Results.NoContent(),
        errors => Results.NotFound(errors)
    );
});

app.Run();

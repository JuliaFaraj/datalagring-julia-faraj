using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Application.Services;
using CoursesManager.Infrastructure.Persistence;
using CoursesManager.Infrastructure.Persistence.Repositories;
using CoursesManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (React/Vite). Vite kan byta port (5173, 5174, 5175...) om den är upptagen.
const string corsPolicyName = "FrontendPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            
            .SetIsOriginAllowed(origin =>
                origin.StartsWith("http://localhost:517") || origin.StartsWith("https://localhost:517"))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DI – repositories + services
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CourseService>();

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<TeacherService>();

builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<ParticipantService>();

builder.Services.AddScoped<ICourseOccasionRepository, CourseOccasionRepository>();
builder.Services.AddScoped<CourseOccasionService>();

builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<EnrollmentService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyName);

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("CoursesManager API is running"));

// Courses 
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

// Teachers 
app.MapGet("/teachers", async (TeacherService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(result);
});

app.MapPost("/teachers", async (TeacherService service, CreateTeacherDto dto) =>
{
    var result = await service.CreateAsync(dto);

    return result.Match(
        created => Results.Created($"/teachers/{created.TeacherCode}", created),
        errors => Results.BadRequest(errors)
    );
});

app.MapPut("/teachers/{teacherCode}", async (string teacherCode, TeacherService service, UpdateTeacherDto dto) =>
{
    var result = await service.UpdateAsync(teacherCode, dto);

    return result.Match(
        updated => Results.Ok(updated),
        errors => Results.BadRequest(errors) 
    );
});

app.MapDelete("/teachers/{teacherCode}", async (string teacherCode, TeacherService service) =>
{
    var result = await service.DeleteAsync(teacherCode);

    return result.Match(
        _ => Results.NoContent(),
        errors => Results.NotFound(errors)
    );
});

// Participants 
app.MapGet("/participants", async (ParticipantService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(result);
});

app.MapPost("/participants", async (ParticipantService service, CreateParticipantDto dto) =>
{
    var result = await service.CreateAsync(dto);

    return result.Match(
        created => Results.Created($"/participants/{created.ParticipantCode}", created),
        errors => Results.BadRequest(errors)
    );
});

app.MapPut("/participants/{participantCode}", async (string participantCode, ParticipantService service, UpdateParticipantDto dto) =>
{
    var result = await service.UpdateAsync(participantCode, dto);

    return result.Match(
        updated => Results.Ok(updated),
        errors => Results.BadRequest(errors)
    );
});

app.MapDelete("/participants/{participantCode}", async (string participantCode, ParticipantService service) =>
{
    var result = await service.DeleteAsync(participantCode);

    return result.Match(
        _ => Results.NoContent(),
        errors => Results.NotFound(errors)
    );
});

//  Occasions 
app.MapGet("/occasions", async (CourseOccasionService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(result);
});

app.MapPost("/occasions", async (CourseOccasionService service, CreateCourseOccasionDto dto) =>
{
    var result = await service.CreateAsync(dto);

    return result.Match(
        created => Results.Created($"/occasions/{created.OccasionCode}", created),
        errors => Results.BadRequest(errors)
    );
});

app.MapPut("/occasions/{occasionCode}", async (string occasionCode, CourseOccasionService service, UpdateCourseOccasionDto dto) =>
{
    var result = await service.UpdateAsync(occasionCode, dto);

    return result.Match(
        updated => Results.Ok(updated),
        errors => Results.BadRequest(errors)
    );
});

app.MapDelete("/occasions/{occasionCode}", async (string occasionCode, CourseOccasionService service) =>
{
    var result = await service.DeleteAsync(occasionCode);

    return result.Match(
        _ => Results.NoContent(),
        errors => Results.NotFound(errors)
    );
});

// Enrollments 
app.MapGet("/enrollments", async (EnrollmentService service) =>
{
    var result = await service.GetAllAsync();
    return Results.Ok(result);
});

app.MapPost("/enrollments", async (EnrollmentService service, CreateEnrollmentDto dto) =>
{
    var result = await service.CreateAsync(dto);

    return result.Match(
        created => Results.Created($"/enrollments/{created.Id}", created),
        errors => Results.BadRequest(errors)
    );
});

app.MapDelete("/enrollments/{id:int}", async (int id, EnrollmentService service) =>
{
    var result = await service.DeleteAsync(id);

    return result.Match(
        _ => Results.NoContent(),
        errors => Results.NotFound(errors)
    );
});

app.Run();

public partial class Program { }

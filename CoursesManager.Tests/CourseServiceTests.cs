using System.Threading;
using System.Threading.Tasks;
using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos;
using CoursesManager.Application.Services;
using CoursesManager.Domain.Entities;
using Moq;
using Xunit;

namespace CoursesManager.Tests;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _repoMock;
    private readonly CourseService _sut;

    public CourseServiceTests()
    {
        _repoMock = new Mock<ICourseRepository>();
        _sut = new CourseService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateCourseAsync_WhenCourseCodeAlreadyExists_ReturnsConflict()
    {
        // Arrange
        var dto = new CreateCourseDto
        {
            CourseCode = "NET101",
            Title = "Intro till .NET",
            Description = "Grundkurs i C# och ASP.NET"
        };

        _repoMock
            .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<CourseEntity, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.CreateCourseAsync(dto, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }

    [Fact]
    public async Task UpdateCourseAsync_WhenCourseNotFound_ReturnsNotFound()
    {
        // Arrange
        var rowVersion = new byte[] { 1, 2, 3 };

        var dto = new UpdateCourseDto(
            Title: "Ny titel",
            Description: "Ny beskrivning",
            RowVersion: rowVersion
        );

        _repoMock
      .Setup(r => r.GetOneAsync(
          It.IsAny<System.Linq.Expressions.Expression<System.Func<CourseEntity, bool>>>(),
          It.IsAny<bool>(),
          It.IsAny<CancellationToken>()))
      .ReturnsAsync((CourseEntity?)null);

        // Act
        var result = await _sut.UpdateCourseAsync("NET101", dto, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }

    [Fact]
    public async Task DeleteCourseAsync_WhenCourseNotFound_ReturnsNotFound()
    {
        // Arrange
        _repoMock
       .Setup(r => r.GetOneAsync(
           It.IsAny<System.Linq.Expressions.Expression<System.Func<CourseEntity, bool>>>(),
           It.IsAny<bool>(),
           It.IsAny<CancellationToken>()))
       .ReturnsAsync((CourseEntity?)null);


        // Act
        var result = await _sut.DeleteCourseAsync("NET101", CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
    }
}


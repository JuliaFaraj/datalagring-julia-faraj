using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CoursesManager.Tests;

public class CoursesApiTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public CoursesApiTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_Courses_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/courses");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}


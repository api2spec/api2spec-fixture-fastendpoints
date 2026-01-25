using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Api.Tests;

[Collection("Api")]
public class HealthEndpointsTests
{
    private readonly ApiWebApplicationFactory _factory;

    public HealthEndpointsTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient() => _factory.CreateClient();

    [Fact]
    public async Task GetHealth_ReturnsOkWithStatus()
    {
        // Act
        var response = await CreateClient().GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HealthResponse>();
        content.Should().NotBeNull();
        content!.Status.Should().Be("ok");
        content.Version.Should().Be("0.1.0");
    }

    [Fact]
    public async Task GetHealthReady_ReturnsOkWithReadyStatus()
    {
        // Act
        var response = await CreateClient().GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HealthResponse>();
        content.Should().NotBeNull();
        content!.Status.Should().Be("ready");
        content.Version.Should().Be("0.1.0");
    }

    private record HealthResponse(string Status, string Version);
}

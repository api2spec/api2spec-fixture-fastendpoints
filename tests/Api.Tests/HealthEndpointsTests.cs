using System.Net;
using System.Net.Http.Json;
using FastEndpoints.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests;

public class HealthEndpointsTests(ApiTestFixture fixture, ITestOutputHelper output) : TestClass<ApiTestFixture>(fixture, output)
{
    [Fact]
    public async Task GetHealth_ReturnsOkWithStatus()
    {
        // Act
        var response = await Fixture.Client.GetAsync("/health");

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
        var response = await Fixture.Client.GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HealthResponse>();
        content.Should().NotBeNull();
        content!.Status.Should().Be("ready");
        content.Version.Should().Be("0.1.0");
    }

    private record HealthResponse(string Status, string Version);
}

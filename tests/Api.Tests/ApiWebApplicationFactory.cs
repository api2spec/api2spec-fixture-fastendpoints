using FastEndpoints.Testing;
using Microsoft.AspNetCore.Hosting;

namespace Api.Tests;

/// <summary>
/// TestFixture for testing FastEndpoints APIs using the FastEndpoints.Testing library.
/// This replaces the ICollectionFixture workaround with the official testing pattern.
///
/// The TestFixture handles:
/// - Application bootstrapping in memory via WebApplicationFactory
/// - Automatic caching and reuse across test classes
/// - Proper service provider lifecycle management
/// </summary>
public class ApiTestFixture : TestFixture<Program>
{
    protected override void ConfigureApp(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    protected override Task SetupAsync()
    {
        // One-time setup before all tests
        return Task.CompletedTask;
    }

    protected override Task TearDownAsync()
    {
        // Cleanup after all tests complete
        return Task.CompletedTask;
    }
}

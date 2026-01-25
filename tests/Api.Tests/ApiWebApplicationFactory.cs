using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Api.Tests;

/// <summary>
/// WebApplicationFactory for testing FastEndpoints APIs.
///
/// IMPORTANT: FastEndpoints caches validators as singletons on first resolution.
/// When using IClassFixture across multiple test classes running in parallel,
/// the first class to run will have its validators cached with a service provider
/// reference. When that test class disposes, subsequent tests fail with
/// ObjectDisposedException.
///
/// Solution: Use ICollectionFixture with a shared collection definition to ensure
/// a single factory instance is shared across ALL test classes, keeping the
/// service provider alive for the entire test run.
/// </summary>
public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}

/// <summary>
/// Collection definition that ensures all test classes using [Collection("Api")]
/// share the same ApiWebApplicationFactory instance. This prevents the
/// ObjectDisposedException that occurs when FastEndpoints' singleton validator
/// cache references a disposed service provider.
/// </summary>
[CollectionDefinition("Api")]
public class ApiTestCollection : ICollectionFixture<ApiWebApplicationFactory>
{
    // This class has no code and is never instantiated. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

using FastEndpoints;

namespace Api.Endpoints.Health;

public class HealthEndpoint : EndpointWithoutRequest<HealthResponse>
{
    public override void Configure()
    {
        Get("/health");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new HealthResponse { Status = "ok", Version = "0.1.0" });
    }
}

public class ReadyEndpoint : EndpointWithoutRequest<HealthResponse>
{
    public override void Configure()
    {
        Get("/health/ready");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new HealthResponse { Status = "ready", Version = "0.1.0" });
    }
}

public class HealthResponse
{
    public string Status { get; set; } = "";
    public string Version { get; set; } = "";
}

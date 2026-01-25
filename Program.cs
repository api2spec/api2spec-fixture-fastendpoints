using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();

var app = builder.Build();
app.UseFastEndpoints();

// Only run if not in test mode
if (!app.Environment.IsEnvironment("Testing"))
{
    app.Run("http://0.0.0.0:8080");
}
else
{
    app.Run();
}

public partial class Program { }

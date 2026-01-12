using FastEndpoints;

namespace Api.Endpoints.Users;

public record User(int Id, string Name, string Email);
public record UserRequest { public string Name { get; set; } = ""; public string Email { get; set; } = ""; }

public class ListUsersEndpoint : EndpointWithoutRequest<List<User>>
{
    public override void Configure() { Get("/users"); AllowAnonymous(); }
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new List<User> {
            new(1, "Alice", "alice@example.com"),
            new(2, "Bob", "bob@example.com")
        });
    }
}

public class GetUserEndpoint : EndpointWithoutRequest<User>
{
    public override void Configure() { Get("/users/{id}"); AllowAnonymous(); }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        await SendAsync(new User(id, "Sample User", "user@example.com"));
    }
}

public class CreateUserEndpoint : Endpoint<UserRequest, User>
{
    public override void Configure() { Post("/users"); AllowAnonymous(); }
    public override async Task HandleAsync(UserRequest req, CancellationToken ct)
    {
        await SendCreatedAtAsync<GetUserEndpoint>(new { id = 1 }, new User(1, req.Name, req.Email));
    }
}

public class UpdateUserEndpoint : Endpoint<UserRequest, User>
{
    public override void Configure() { Put("/users/{id}"); AllowAnonymous(); }
    public override async Task HandleAsync(UserRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        await SendAsync(new User(id, req.Name, req.Email));
    }
}

public class DeleteUserEndpoint : EndpointWithoutRequest
{
    public override void Configure() { Delete("/users/{id}"); AllowAnonymous(); }
    public override async Task HandleAsync(CancellationToken ct) { await SendNoContentAsync(); }
}

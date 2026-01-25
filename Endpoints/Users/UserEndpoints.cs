using FastEndpoints;
using FluentValidation;

namespace Api.Endpoints.Users;

public record User(int Id, string Name, string Email);
public record UserRequest { public string Name { get; set; } = ""; public string Email { get; set; } = ""; }

public class UserRequestValidator : Validator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}

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
        if (id == 999)
        {
            await SendNotFoundAsync();
            return;
        }
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
        if (id == 999)
        {
            await SendNotFoundAsync();
            return;
        }
        await SendAsync(new User(id, req.Name, req.Email));
    }
}

public class DeleteUserEndpoint : EndpointWithoutRequest
{
    public override void Configure() { Delete("/users/{id}"); AllowAnonymous(); }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        if (id == 999)
        {
            await SendNotFoundAsync();
            return;
        }
        await SendNoContentAsync();
    }
}

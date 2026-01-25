using FastEndpoints;
using FluentValidation;

namespace Api.Endpoints.Posts;

public record Post(int Id, int UserId, string Title, string Body);
public record PostRequest { public int UserId { get; set; } public string Title { get; set; } = ""; public string Body { get; set; } = ""; }

public class PostRequestValidator : Validator<PostRequest>
{
    public PostRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required");
    }
}

public class ListPostsEndpoint : EndpointWithoutRequest<List<Post>>
{
    public override void Configure() { Get("/posts"); AllowAnonymous(); }
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new List<Post> {
            new(1, 1, "First Post", "Hello world"),
            new(2, 1, "Second Post", "Another post")
        });
    }
}

public class GetPostEndpoint : EndpointWithoutRequest<Post>
{
    public override void Configure() { Get("/posts/{id}"); AllowAnonymous(); }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        if (id == 999)
        {
            await SendNotFoundAsync();
            return;
        }
        await SendAsync(new Post(id, 1, "Sample Post", "Post body"));
    }
}

public class CreatePostEndpoint : Endpoint<PostRequest, Post>
{
    public override void Configure() { Post("/posts"); AllowAnonymous(); }
    public override async Task HandleAsync(PostRequest req, CancellationToken ct)
    {
        await SendCreatedAtAsync<GetPostEndpoint>(new { id = 1 }, new Post(1, req.UserId, req.Title, req.Body));
    }
}

public class UserPostsEndpoint : EndpointWithoutRequest<List<Post>>
{
    public override void Configure() { Get("/users/{userId}/posts"); AllowAnonymous(); }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<int>("userId");
        if (userId == 999)
        {
            await SendNotFoundAsync();
            return;
        }
        await SendAsync(new List<Post> { new(1, userId, "User Post", "Content") });
    }
}

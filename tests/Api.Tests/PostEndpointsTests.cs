using System.Net;
using System.Net.Http.Json;
using FastEndpoints.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests;

public class PostEndpointsTests(ApiTestFixture fixture, ITestOutputHelper output) : TestClass<ApiTestFixture>(fixture, output)
{
    #region GET /posts

    [Fact]
    public async Task ListPosts_ReturnsOkWithPostList()
    {
        // Act
        var response = await Fixture.Client.GetAsync("/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var posts = await response.Content.ReadFromJsonAsync<List<PostResponse>>();
        posts.Should().NotBeNull();
        posts.Should().HaveCount(2);
        posts![0].Title.Should().Be("First Post");
        posts[1].Title.Should().Be("Second Post");
    }

    #endregion

    #region GET /posts/{id}

    [Fact]
    public async Task GetPost_WhenPostExists_ReturnsOkWithPost()
    {
        // Act
        var response = await Fixture.Client.GetAsync("/posts/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var post = await response.Content.ReadFromJsonAsync<PostResponse>();
        post.Should().NotBeNull();
        post!.Id.Should().Be(1);
        post.Title.Should().Be("Sample Post");
        post.Body.Should().Be("Post body");
    }

    [Fact]
    public async Task GetPost_WhenPostNotFound_Returns404()
    {
        // Act
        var response = await Fixture.Client.GetAsync("/posts/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region POST /posts

    [Fact]
    public async Task CreatePost_WithValidData_Returns201()
    {
        // Arrange
        var newPost = new { UserId = 1, Title = "New Post", Body = "Post content" };

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/posts", newPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var post = await response.Content.ReadFromJsonAsync<PostResponse>();
        post.Should().NotBeNull();
        post!.Title.Should().Be("New Post");
        post.Body.Should().Be("Post content");
        post.UserId.Should().Be(1);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task CreatePost_WithEmptyTitle_Returns400()
    {
        // Arrange
        var invalidPost = new { UserId = 1, Title = "", Body = "Some content" };

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/posts", invalidPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePost_WithShortTitle_Returns400()
    {
        // Arrange
        var invalidPost = new { UserId = 1, Title = "AB", Body = "Some content" };

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/posts", invalidPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePost_WithEmptyBody_Returns400()
    {
        // Arrange
        var invalidPost = new { UserId = 1, Title = "Valid Title", Body = "" };

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/posts", invalidPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePost_WithZeroUserId_Returns400()
    {
        // Arrange
        var invalidPost = new { UserId = 0, Title = "Valid Title", Body = "Valid body" };

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/posts", invalidPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreatePost_WithNegativeUserId_Returns400()
    {
        // Arrange
        var invalidPost = new { UserId = -1, Title = "Valid Title", Body = "Valid body" };

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/posts", invalidPost);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region GET /users/{userId}/posts

    [Fact]
    public async Task GetUserPosts_WhenUserExists_ReturnsOkWithPosts()
    {
        // Act
        var response = await Fixture.Client.GetAsync("/users/1/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var posts = await response.Content.ReadFromJsonAsync<List<PostResponse>>();
        posts.Should().NotBeNull();
        posts.Should().HaveCount(1);
        posts![0].UserId.Should().Be(1);
        posts[0].Title.Should().Be("User Post");
    }

    [Fact]
    public async Task GetUserPosts_WhenUserNotFound_Returns404()
    {
        // Act
        var response = await Fixture.Client.GetAsync("/users/999/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    private record PostResponse(int Id, int UserId, string Title, string Body);
}

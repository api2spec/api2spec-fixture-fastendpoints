using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Api.Tests;

[Collection("Api")]
public class UserEndpointsTests
{
    private readonly ApiWebApplicationFactory _factory;

    public UserEndpointsTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient() => _factory.CreateClient();

    #region GET /users

    [Fact]
    public async Task ListUsers_ReturnsOkWithUserList()
    {
        // Act
        var response = await CreateClient().GetAsync("/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
        users.Should().NotBeNull();
        users.Should().HaveCount(2);
        users![0].Name.Should().Be("Alice");
        users[1].Name.Should().Be("Bob");
    }

    #endregion

    #region GET /users/{id}

    [Fact]
    public async Task GetUser_WhenUserExists_ReturnsOkWithUser()
    {
        // Act
        var response = await CreateClient().GetAsync("/users/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(1);
        user.Name.Should().Be("Sample User");
        user.Email.Should().Be("user@example.com");
    }

    [Fact]
    public async Task GetUser_WhenUserNotFound_Returns404()
    {
        // Act
        var response = await CreateClient().GetAsync("/users/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region POST /users

    [Fact]
    public async Task CreateUser_WithValidData_Returns201()
    {
        // Arrange
        var newUser = new { Name = "Alice", Email = "alice@example.com" };

        // Act
        var response = await CreateClient().PostAsJsonAsync("/users", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();
        user.Should().NotBeNull();
        user!.Name.Should().Be("Alice");
        user.Email.Should().Be("alice@example.com");
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateUser_WithEmptyName_Returns400()
    {
        // Arrange
        var invalidUser = new { Name = "", Email = "test@example.com" };

        // Act
        var response = await CreateClient().PostAsJsonAsync("/users", invalidUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_Returns400()
    {
        // Arrange
        var invalidUser = new { Name = "Test User", Email = "not-an-email" };

        // Act
        var response = await CreateClient().PostAsJsonAsync("/users", invalidUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithEmptyEmail_Returns400()
    {
        // Arrange
        var invalidUser = new { Name = "Test User", Email = "" };

        // Act
        var response = await CreateClient().PostAsJsonAsync("/users", invalidUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithShortName_Returns400()
    {
        // Arrange
        var invalidUser = new { Name = "A", Email = "test@example.com" };

        // Act
        var response = await CreateClient().PostAsJsonAsync("/users", invalidUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region PUT /users/{id}

    [Fact]
    public async Task UpdateUser_WithValidData_ReturnsOk()
    {
        // Arrange
        var updatedUser = new { Name = "Updated Name", Email = "updated@example.com" };

        // Act
        var response = await CreateClient().PutAsJsonAsync("/users/1", updatedUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();
        user.Should().NotBeNull();
        user!.Id.Should().Be(1);
        user.Name.Should().Be("Updated Name");
        user.Email.Should().Be("updated@example.com");
    }

    [Fact]
    public async Task UpdateUser_WhenUserNotFound_Returns404()
    {
        // Arrange
        var updatedUser = new { Name = "Updated Name", Email = "updated@example.com" };

        // Act
        var response = await CreateClient().PutAsJsonAsync("/users/999", updatedUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUser_WithInvalidData_Returns400()
    {
        // Arrange
        var invalidUser = new { Name = "", Email = "invalid" };

        // Act
        var response = await CreateClient().PutAsJsonAsync("/users/1", invalidUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region DELETE /users/{id}

    [Fact]
    public async Task DeleteUser_WhenUserExists_Returns204()
    {
        // Act
        var response = await CreateClient().DeleteAsync("/users/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteUser_WhenUserNotFound_Returns404()
    {
        // Act
        var response = await CreateClient().DeleteAsync("/users/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    private record UserResponse(int Id, string Name, string Email);
}

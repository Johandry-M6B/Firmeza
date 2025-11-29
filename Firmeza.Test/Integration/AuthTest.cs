using System.Net.Http.Json;
using Application.Auth.Commands.Login;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Firmeza.Test.Integration;

public class AuthTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_WithDefaultAdmin_ShouldReturnToken()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new LoginCommand("admin@firmeza.com", "Admin123*");

        // Verify user exists
        using (var scope = _factory.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Infrastructure.Identity.ApplicationUser>>();
            var user = await userManager.FindByEmailAsync("admin@firmeza.com");
            Assert.NotNull(user);
            var checkPassword = await userManager.CheckPasswordAsync(user, "Admin123*");
            Assert.True(checkPassword, "Password check failed");
        }

        // Act
        var response = await client.PostAsJsonAsync("/api/Auth/login", command);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {response.StatusCode} - {content}");
        }

        // Assert
        response.EnsureSuccessStatusCode();
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResponse);
        Assert.NotEmpty(loginResponse.Token);
        Assert.Equal("admin@firmeza.com", loginResponse.Email);
    }
}

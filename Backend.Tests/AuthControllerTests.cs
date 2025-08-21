using Backend.Controllers;
using Backend.DTOs.AuthDtos;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Register_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123"
        };

        var response = new LoginResponseDto
        {
            Token = "test-token",
            User = new UserInfoDto
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com",
                Role = "usuario"
            }
        };

        _mockAuthService.Setup(x => x.RegisterAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal("test-token", returnedResponse.Token);
        Assert.Equal("testuser", returnedResponse.User.Username);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Username = "testuser",
            Password = "password123"
        };

        var response = new LoginResponseDto
        {
            Token = "test-token",
            User = new UserInfoDto
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com",
                Role = "usuario"
            }
        };

        _mockAuthService.Setup(x => x.LoginAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResponse = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal("test-token", returnedResponse.Token);
        Assert.Equal("testuser", returnedResponse.User.Username);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        _mockAuthService.Setup(x => x.LoginAsync(request))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.Login(request);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Register_UserExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Username = "existinguser",
            Email = "existing@example.com",
            Password = "password123"
        };

        _mockAuthService.Setup(x => x.RegisterAsync(request))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.Register(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
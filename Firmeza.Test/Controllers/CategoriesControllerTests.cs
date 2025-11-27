using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.DeleteCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.DTOs;
using Application.Categories.Queries.GetCategories;
using Application.Categories.Queries.GetCategoryById;
using Firmeza.Api.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Firmeza.Test.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CategoriesController();
        
        // Set up HttpContext with the mocked Mediator
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider
            .Setup(x => x.GetService(typeof(ISender)))
            .Returns(_mediatorMock.Object);
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider.Object
            }
        };
    }

    [Fact]
    public async Task GetCategories_ShouldReturnOkWithCategories()
    {
        // Arrange
        var expectedCategories = new List<CategoryDto>
        {
            new() { Id = 1, Name = "Category 1", Active = true },
            new() { Id = 2, Name = "Category 2", Active = true }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), default))
            .ReturnsAsync(expectedCategories);

        // Act
        var result = await _controller.GetCategories(true);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedCategories);
    }

    [Fact]
    public async Task GetCategoryById_WhenCategoryExists_ShouldReturnOkWithCategory()
    {
        // Arrange
        var expectedCategory = new CategoryDto { Id = 1, Name = "Category 1", Active = true };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), default))
            .ReturnsAsync(expectedCategory);

        // Act
        var result = await _controller.GetCategoryById(1);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task GetCategoryById_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), default))
            .ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _controller.GetCategoryById(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnOkWithCategoryId()
    {
        // Arrange
        var command = new CreateCategoryCommand { Name = "New Category" };
        var expectedId = 1;

        _mediatorMock
            .Setup(m => m.Send(command, default))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _controller.Create(command);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().Be(expectedId);
    }

    [Fact]
    public async Task Update_WithMatchingId_ShouldReturnNoContent()
    {
        // Arrange
        var command = new UpdateCategoryCommand { Id = 1, Name = "Updated Category" };

        _mediatorMock
            .Setup(m => m.Send(command, default))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Update(1, command);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_WithMismatchedId_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new UpdateCategoryCommand { Id = 1, Name = "Updated Category" };

        // Act
        var result = await _controller.Update(2, command);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), default))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mediatorMock.Verify(m => m.Send(It.Is<DeleteCategoryCommand>(c => c.Id == 1), default), Times.Once);
    }
}

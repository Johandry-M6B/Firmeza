using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.DTOs;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProducts;
using Firmeza.Api.Controllers;
using Firmeza.Application.Products.Commands.UpdateProduct;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Firmeza.Test.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductsController();
        
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
    public async Task GetProducts_ShouldReturnOkWithProducts()
    {
        // Arrange
        var expectedProducts = new List<ProductDto>
        {
            new() { Id = 1, Name = "Product 1", SalePrice = 100, Active = true },
            new() { Id = 2, Name = "Product 2", SalePrice = 200, Active = true }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProductsQuery>(), default))
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetProducts(true);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ShouldReturnOkWithProduct()
    {
        // Arrange
        var expectedProduct = new ProductDto { Id = 1, Name = "Product 1", SalePrice = 100 };
        
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(expectedProduct);

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync((ProductDto?)null);

        // Act
        var result = await _controller.GetProductById(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnOkWithProductId()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = "New Product",
            CategoryId = 1
        };
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
        var command = new UpdateProductCommand { Id = 1, Name = "Updated Product" };

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
        var command = new UpdateProductCommand { Id = 1, Name = "Updated Product" };

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
            .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mediatorMock.Verify(m => m.Send(It.Is<DeleteProductCommand>(c => c.Id == 1), default), Times.Once);
    }
}

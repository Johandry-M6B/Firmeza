// Firmeza.Application/Products/Commands/DeleteProduct/DeleteProductCommand.cs

using MediatR;

namespace Application.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
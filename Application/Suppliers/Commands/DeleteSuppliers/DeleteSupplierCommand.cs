using MediatR;

namespace Application.Suppliers.Commands.DeleteSuppliers;

public class DeleteSupplierCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
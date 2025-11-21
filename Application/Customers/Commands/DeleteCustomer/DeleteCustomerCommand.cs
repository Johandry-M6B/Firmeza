using MediatR;

namespace Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
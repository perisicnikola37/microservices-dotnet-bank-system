using Customer.Application.Features.Queries;
using MediatR;

namespace Customer.Application.Features.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<CustomerDto>
    {
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    }
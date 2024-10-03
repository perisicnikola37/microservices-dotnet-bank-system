using MediatR;

namespace Customer.Application.Features.Queries.GetCustomer
    {
    public class CustomerQuery(Guid id) : IRequest<CustomerDto>
        {
        public Guid Id { get; set; } = id;
        }
    }
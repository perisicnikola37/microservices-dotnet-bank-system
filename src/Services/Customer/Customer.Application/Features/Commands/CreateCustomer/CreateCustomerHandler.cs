using AutoMapper;
using Customer.Application.Contracts.Persistance;
using Customer.Application.Features.Queries;
using MediatR;

namespace Customer.Application.Features.Commands.CreateCustomer
{
    public class CreateCustomerHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
        : IRequestHandler<CreateCustomerCommand, CustomerDto>
        {

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerEntity = mapper.Map<Domain.Entities.Customer>(request);
            var newCustomer = await customerRepository.AddAsync(customerEntity!);
            
            return mapper.Map<CustomerDto>(newCustomer)!;
        }
    }
}
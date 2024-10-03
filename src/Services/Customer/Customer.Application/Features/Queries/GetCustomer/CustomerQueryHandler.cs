using AutoMapper;
using Customer.Application.Contracts.Persistance;
using MediatR;

namespace Customer.Application.Features.Queries.GetCustomer
    {
    public class CustomerQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        : IRequestHandler<CustomerQuery, CustomerDto>
        {
        public async Task<CustomerDto> Handle(CustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByIdAsync(request.Id);
            
            return mapper.Map<CustomerDto>(customer)!;
        }
        }
    }
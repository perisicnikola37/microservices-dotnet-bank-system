using Customer.Application.Contracts.Persistance;
using Grpc.Core;

namespace Customer.GRPC.Services
    {
    public class CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        : CustomerProtoService.CustomerProtoServiceBase
        {
        public override async Task<CheckCustomerModel> CheckCustomer(CheckCustomerRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                logger.LogWarning("Customer id is not valid.");
                return new CheckCustomerModel() { Result = false };
            }


            return new CheckCustomerModel
            {
                Result = await customerRepository.AnyAsync(x => x.Id == Guid.Parse(request.Id))
            };
        }
        }
    }
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Commands.CreateAccount
    {
    public class CreateAccountHandler(
        IAccountRepository accountRepository,
        IMapper mapper,
        ILogger<CreateAccountHandler> logger)
        : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
        {
        public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var accountEntity = mapper.Map<Domain.Entities.Account>(request);
                var newAccount = await accountRepository.AddAsync(accountEntity!);
                
                logger.LogInformation($"Account created successfully. AccountId: {newAccount.Id}, CustomerId: {newAccount.CustomerId}, Initial Balance: {newAccount.Balance}");
                
                return new CreateAccountResponse(newAccount.Id, newAccount.CustomerId, newAccount.Balance);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating account for CustomerId: {CustomerId}", request.CustomerId);
                throw; 
            }
        }
        }
    }
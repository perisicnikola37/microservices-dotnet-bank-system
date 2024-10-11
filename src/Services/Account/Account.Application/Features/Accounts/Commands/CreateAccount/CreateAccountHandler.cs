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
        private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        private readonly ILogger<CreateAccountHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var accountEntity = _mapper.Map<Domain.Entities.Account>(request);
                var newAccount = await _accountRepository.AddAsync(accountEntity!).ConfigureAwait(false);

                _logger.LogInformation("Account created successfully. AccountId: {AccountId}, CustomerId: {CustomerId}, Initial Balance: {InitialBalance}",
                    newAccount.Id, newAccount.CustomerId, newAccount.Balance);
                
                return new CreateAccountResponse(newAccount.Id, newAccount.CustomerId, newAccount.Balance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating account for CustomerId: {CustomerId}", request.CustomerId);
                throw; 
            }
        }
        }
}

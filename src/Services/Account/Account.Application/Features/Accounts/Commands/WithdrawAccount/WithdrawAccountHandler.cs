using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Commands.WithdrawAccount
{
    public class WithdrawingHandler(IAccountRepository accountRepository, ILogger<WithdrawingHandler> logger)
        : IRequestHandler<WithdrawAccountCommand>
        {
        private readonly IAccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

        public async Task Handle(WithdrawAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId);
    
            if (account is null)
            {
                logger.LogError($"Account not found: {request.AccountId}");
                throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);
            }

            if (account.CustomerId != request.CustomerId)
            {
                logger.LogWarning($"Unauthorized access attempt by CustomerId: {request.CustomerId} on AccountId: {request.AccountId}");
                throw new UnauthorizedAccessException("You do not have access to this account.");
            }

            if (account.Balance < request.Amount)
            {
                logger.LogWarning($"Insufficient balance for AccountId: {request.AccountId}. Balance: {account.Balance}, Requested: {request.Amount}");
                throw new InsufficientBalanceException(account.Balance);
            }

            // Perform withdrawal
            account.Balance -= request.Amount;
            await _accountRepository.UpdateAsync(account);
    
            logger.LogInformation($"Withdrawal successful for AccountId: {request.AccountId}. New Balance: {account.Balance}");
        }
        }
}

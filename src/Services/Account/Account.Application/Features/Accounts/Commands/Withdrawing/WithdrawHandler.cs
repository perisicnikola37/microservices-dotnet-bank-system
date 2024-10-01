using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Commands.Withdrawing
{
    public class WithdrawingHandler(IAccountRepository accountRepository, ILogger<WithdrawingHandler> logger)
        : IRequestHandler<WithdrawCommand>
        {
        public async Task Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var accountUpdate = await accountRepository.GetByIdAsync(request.AccountId);
            if (accountUpdate is null)
            {
                logger.LogError($"Account not found: {request.AccountId}");
                throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);
            }

            if (accountUpdate.CustomerId != request.CustomerId)
            {
                logger.LogWarning($"Unauthorized access attempt by CustomerId: {request.CustomerId} on AccountId: {request.AccountId}");
                throw new UnauthorizedAccessException();
            }

            if (accountUpdate.Balance < request.Amount)
            {
                logger.LogWarning($"Insufficient balance for AccountId: {request.AccountId}. Balance: {accountUpdate.Balance}, Requested: {request.Amount}");
                throw new InsufficientBalanceException(accountUpdate.Balance);
            }

            // Perform withdrawal
            accountUpdate.Balance -= request.Amount;

            // Update account in the repository
            await accountRepository.UpdateAsync(accountUpdate);
            logger.LogInformation($"Withdrawal successful for AccountId: {request.AccountId}. New Balance: {accountUpdate.Balance}");
        }
    }
}

using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Commands.Updating
{
    public class UpdateAccountHandler(
        IAccountRepository accountRepository,
        ILogger<UpdateAccountHandler> logger) 
        : IRequestHandler<UpdateAccountCommand>
    {
        public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
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

            // Update the account balance
            accountUpdate.Balance += request.Amount;

            await accountRepository.UpdateAsync(accountUpdate);
            logger.LogInformation($"Account updated successfully. AccountId: {request.AccountId}, New Balance: {accountUpdate.Balance}");
        }
    }
}

using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using MediatR;

namespace Account.Application.Features.Accounts.Commands.Updating;

public class UpdateAccountHandler(IAccountRepository accountRepository) : IRequestHandler<UpdateAccountCommand>
    {
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var accountUpdate = await accountRepository.GetByIdAsync(request.AccountId);
        if (accountUpdate is null)
            throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);
        if (accountUpdate.CustomerId != request.CustomerId)
            throw new UnauthorizedAccessException();
        
        accountUpdate.Balance += request.Amount;

        await accountRepository.UpdateAsync(accountUpdate);
    }
    }
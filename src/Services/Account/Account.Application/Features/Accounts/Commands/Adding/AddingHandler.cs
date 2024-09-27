using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using MediatR;

namespace Account.Application.Features.Accounts.Commands.Adding;

public class AddingHandler(IAccountRepository accountRepository)
    : IRequestHandler<AddingCommand>
    {
    public async Task<Unit> Handle(AddingCommand request)
    {
        var accountUpdate = await accountRepository.GetByIdAsync(request.AccountId);
        if (accountUpdate is null)
            throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);
        if (accountUpdate!.CustomerId != request.CustomerId)
            throw new UnauthorizedAccessException();
        accountUpdate.Balance += request.Amount;
        await accountRepository.UpdateAsync(accountUpdate);
        
        return Unit.Value;
    }

    Task IRequestHandler<AddingCommand>.Handle(AddingCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    }
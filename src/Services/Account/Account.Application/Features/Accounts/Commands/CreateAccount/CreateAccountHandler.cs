using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Account.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountHandler(
    IAccountRepository accountRepository,
    IMapper mapper)
    : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
    {
    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var accountEntity = mapper.Map<Domain.Entities.Account>(request);
        var newAccount = await accountRepository.AddAsync(accountEntity!);
        
        return new CreateAccountResponse(newAccount.Id, newAccount.CustomerId, newAccount.Balance);
    }
    }
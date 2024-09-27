using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Account.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountHandler(
    IAccountRepository accountRepository,
    IMapper mapper)
    : IRequestHandler<CreateAccountCommand, Guid>
    {
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var accountEntity = mapper.Map<Domain.Entities.Account>(request);
        var newAccount = await accountRepository.AddAsync(accountEntity!);
        
        return newAccount.Id;
    }
    }
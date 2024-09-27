using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;

namespace Account.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountHandler(
    IAccountRepository accountRepository,
    IMapper mapper)
    : IRequestHandler<CreateAccountCommand, Guid>
    {
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var accountEntity = _mapper.Map<Domain.Entities.Account>(request);
        var newAccount = await accountRepository.AddAsync(accountEntity!);
        
        return newAccount.Id;
    }
    }
using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace Account.Application.Features.Accounts.Queries.GetAccount;

public class AccountQueryHandler(
    IAccountRepository accountRepository,
    IMapper mapper)
    : IRequestHandler<AccountQueryRequest, GetAccountResponse>
    {

    public async Task<GetAccountResponse> Handle(AccountQueryRequest request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAsync(x => x.Id == request.AccountId);
        
        if (account is null)
            throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);
        
        return mapper.Map<GetAccountResponse>(account)!;
    }
    }
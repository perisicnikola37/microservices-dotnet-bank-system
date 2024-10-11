using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Queries.GetAccount;

public class GetAccountQueryHandler(
    IAccountRepository accountRepository,
    IMapper mapper,
    ILogger<GetAccountQueryHandler> logger)
    : IRequestHandler<GetAccountQueryRequest, GetAccountResponse>
    {
    private readonly IAccountRepository _accountRepository =
        accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

    private readonly ILogger<GetAccountQueryHandler>
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<GetAccountResponse> Handle(GetAccountQueryRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAsync(x => x.Id == request.AccountId);

        if (account is null)
        {
            _logger.LogError($"Account not found: {request.AccountId}");
            throw new EntityNotFoundException(nameof(Domain.Entities.Account), request.AccountId);
        }

        _logger.LogInformation(
            $"Account retrieved successfully. AccountId: {request.AccountId}, CustomerId: {account.CustomerId}, Balance: {account.Balance}");

        return _mapper.Map<GetAccountResponse>(account);
    }
    }
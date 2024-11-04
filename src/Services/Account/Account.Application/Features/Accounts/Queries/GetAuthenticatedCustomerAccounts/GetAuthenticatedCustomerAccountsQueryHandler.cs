using Account.Application.Contracts.DTO.Response;
using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Queries.GetAuthenticatedCustomerAccounts;

public class
    GetAuthenticatedCustomerAccountsQueryHandler(
        IAccountRepository accountRepository,
        IMapper mapper,
        ILogger<GetAuthenticatedCustomerAccountsQueryHandler> logger)
    : IRequestHandler<GetAuthenticatedCustomerAccountsQueryRequest,
        List<AccountDto>>
    {
    private readonly IAccountRepository _accountRepository =
        accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

    private readonly ILogger<GetAuthenticatedCustomerAccountsQueryHandler> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<List<AccountDto>> Handle(
        GetAuthenticatedCustomerAccountsQueryRequest queryRequest,
        CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllAsync(x => x.CustomerId == queryRequest.CustomerId);

        if (accounts == null || !accounts.Any())
        {
            _logger.LogWarning($"No accounts found for customer: {queryRequest.CustomerId}");
            throw new EntityNotFoundException("Accounts", queryRequest.CustomerId);
        }

        _logger.LogInformation($"Accounts retrieved successfully for customer: {queryRequest.CustomerId}");

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts);

        return accountDtos;
    }
    }
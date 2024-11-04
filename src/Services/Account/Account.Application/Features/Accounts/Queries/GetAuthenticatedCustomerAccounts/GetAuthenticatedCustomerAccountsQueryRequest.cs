using Account.Application.Contracts.DTO.Response;
using MediatR;

namespace Account.Application.Features.Accounts.Queries.GetAuthenticatedCustomerAccounts;

public record GetAuthenticatedCustomerAccountsQueryRequest(Guid CustomerId)
    : IRequest<GetAuthenticatedCustomerAccountsResponse>, IRequest<List<AccountDto>>;
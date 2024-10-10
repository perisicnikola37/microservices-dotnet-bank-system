using MediatR;

namespace Account.Application.Features.Accounts.Queries.GetAccount
    {
    public record GetAccountQueryRequest(Guid AccountId) : IRequest<GetAccountResponse>;
    }
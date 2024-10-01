using MediatR;

namespace Account.Application.Features.Accounts.Queries.GetAccount;

public class AccountQueryRequest : IRequest<GetAccountResponse>
    {
    public Guid AccountId { get; set; }
    }

using MediatR;

namespace Account.Application.Features.Accounts.Queries.GetAccount;

public class GetAccountQueryRequest : IRequest<GetAccountResponse>
    {
    public Guid AccountId { get; set; }
    }

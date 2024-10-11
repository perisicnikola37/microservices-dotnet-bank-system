using MediatR;

namespace Account.Application.Features.Accounts.Commands.DepositAccount;

public class DepositAccountCommand : IRequest
    {
    public Guid AccountId { get; private set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }

    public void SetAccountId(Guid accountId)
    {
        AccountId = accountId;
    }
    }
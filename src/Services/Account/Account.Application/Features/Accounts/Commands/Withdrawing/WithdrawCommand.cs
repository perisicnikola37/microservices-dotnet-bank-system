using MediatR;

namespace Account.Application.Features.Accounts.Commands.Withdrawing;

public class WithdrawCommand : IRequest
    {
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    }
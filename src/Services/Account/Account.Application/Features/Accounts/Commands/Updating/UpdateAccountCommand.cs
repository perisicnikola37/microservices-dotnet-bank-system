using MediatR;

namespace Account.Application.Features.Accounts.Commands.Updating;

public class UpdateAccountCommand : IRequest
    {
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    }
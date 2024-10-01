using MediatR;

namespace Account.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommand: IRequest<CreateAccountResponse>
    {
    public Guid CustomerId { get; set; }
    }
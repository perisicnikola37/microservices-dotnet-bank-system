using MediatR;

namespace Account.Application.Features.Accounts.Commands.CreateAccount;
    
public record CreateAccountCommand(Guid CustomerId) : IRequest<CreateAccountResponse>;
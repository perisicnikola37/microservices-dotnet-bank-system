namespace Account.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountResponse(Guid AccountId, Guid CustomerId, decimal Amount);
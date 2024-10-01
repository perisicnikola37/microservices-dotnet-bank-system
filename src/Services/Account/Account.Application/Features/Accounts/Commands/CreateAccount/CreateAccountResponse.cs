namespace Account.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountResponse(Guid id, Guid customerId, decimal amount)
    {
    public Guid Id { get; set; } = id;
    public Guid CustomerId { get; set; } = customerId;
    public decimal Amount { get; set; } = amount;
    }
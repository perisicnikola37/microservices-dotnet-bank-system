namespace Account.Application.Features.Accounts.Queries.GetAccount;

public record GetAccountResponse
    {
    public Guid AccountId { get; init; }       
    public Guid CustomerId { get; init; }
    public decimal Balance { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime LastModifiedDate { get; init; }
    }
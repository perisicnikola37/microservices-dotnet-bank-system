namespace Account.Application.Features.Accounts.Queries.GetAccount;

public record GetAccountResponse
    {
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }  
    }

namespace Account.Application.Contracts.DTO.Response;

public class AccountDto
    {
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Balance { get; set; }
    }
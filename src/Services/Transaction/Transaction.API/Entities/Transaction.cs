using EventBus.Messages.Events;

namespace Transaction.API.Entities;

public class Transaction
    {
    public Guid Id { get; set; } = Guid.NewGuid(); 
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
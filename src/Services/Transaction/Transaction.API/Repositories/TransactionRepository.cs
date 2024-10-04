using MongoDB.Driver;
using Transaction.API.Data.DTOs.Requests;
using Transaction.API.Data.Interfaces;
using Transaction.API.Filters;
using Transaction.API.Repositories.Interfaces;

namespace Transaction.API.Repositories;

public class TransactionRepository(ITransactionContext transactionContext) : ITransactionRepository
    {
    public async Task Add(Entities.Transaction transaction)
    {
        await transactionContext.Transactions.InsertOneAsync(transaction);
    }

    public async Task<IEnumerable<Entities.Transaction>> GetByAccountId(Guid accountId)
    {
        return await transactionContext
            .Transactions
            .Find(x => x.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Entities.Transaction>> GetWithFilter(RequestFilter filter)
    {
        var filterBuilder = new TransactionFilterBuilder()
            .WithCustomerId(filter.CustomerId)
            .WithStartDate(filter.StartDate)
            .WithEndDate(filter.EndDate);

        var combinedFilter = filterBuilder.Build();

        return await transactionContext
            .Transactions
            .Find(combinedFilter)
            .ToListAsync();
    }
    }
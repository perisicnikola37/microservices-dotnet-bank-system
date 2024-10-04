using MongoDB.Driver;

namespace Transaction.API.Filters;

public class TransactionFilterBuilder
    {
    private readonly FilterDefinitionBuilder<Entities.Transaction> _filterBuilder = Builders<Entities.Transaction>.Filter;
    private readonly List<FilterDefinition<Entities.Transaction>> _filters = [];

    public TransactionFilterBuilder WithCustomerId(Guid customerId)
    {
        if (customerId != Guid.Empty)
        {
            _filters.Add(_filterBuilder.Eq(x => x.CustomerId, customerId));
        }
        return this;
    }

    public TransactionFilterBuilder WithStartDate(DateTime startDate)
    {
        if (startDate != DateTime.MinValue)
        {
            _filters.Add(_filterBuilder.Gte(x => x.CreatedDate, startDate));
        }
        return this;
    }

    public TransactionFilterBuilder WithEndDate(DateTime endDate)
    {
        if (endDate != DateTime.MinValue)
        {
            _filters.Add(_filterBuilder.Lt(x => x.CreatedDate, endDate));
        }
        return this;
    }

    public FilterDefinition<Entities.Transaction> Build()
    {
        return _filters.Count > 0 ? _filterBuilder.And(_filters) : _filterBuilder.Empty;
    }
    }

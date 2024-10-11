using System.Linq.Expressions;
using Customer.Application.Contracts.Persistance;
using Customer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Repositories
    {
    public class CustomerRepository(CustomerDatabaseContext dbContext)
        : RepositoryBase<Domain.Entities.Customer>(dbContext), ICustomerRepository
        {
        private readonly CustomerDatabaseContext _databaseContext = dbContext;
        public async Task<bool> AnyAsync(Expression<Func<Domain.Entities.Customer, bool>> predicate)
        {
            return await _databaseContext.Customers
                .AnyAsync(predicate);
        }
        }
    }
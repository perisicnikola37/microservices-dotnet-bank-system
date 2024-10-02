using Customer.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Persistance;

public class CustomerDatabaseContext  : DbContext
    {
    public CustomerDatabaseContext(DbContextOptions<CustomerDatabaseContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Customer> Customers { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    }

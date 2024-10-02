using Microsoft.Extensions.Logging;

namespace Customer.Infrastructure.Persistance;

public class CustomerContextSeed
    {
    public static async Task SeedAsync(CustomerDatabaseContext customerContext, ILogger<CustomerContextSeed> logger)
    {
        if (!customerContext.Customers.Any())
        {
            customerContext.Customers.AddRange(GetPreconfiguredCustomers());
            await customerContext.SaveChangesAsync();
            logger.LogInformation($"Seed database associated with context {nameof(CustomerContextSeed)}");
        }
    }

    private static IEnumerable<Domain.Entities.Customer> GetPreconfiguredCustomers()
    {
        return new List<Domain.Entities.Customer>
        {
            new(Guid.Parse("ef533977-e666-4c75-ac4e-ea1de9ea4aef"), "perisicnikola37@gmail.com", "Nikola", "Perišić")
        };
    }
    }
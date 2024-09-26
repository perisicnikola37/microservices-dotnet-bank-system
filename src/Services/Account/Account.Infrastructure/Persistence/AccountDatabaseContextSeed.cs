using Microsoft.Extensions.Logging;

namespace Account.Infrastructure.Persistence;

public abstract class AccountDatabaseContextSeed
    {
    public static async Task SeedAsync(AccountDatabaseContext accountContext, ILogger<AccountDatabaseContextSeed> logger)
    {
        if (!accountContext.Accounts.Any())
        {
            accountContext.Accounts.AddRange(GetPreconfiguredAccounts());
            await accountContext.SaveChangesAsync();
            logger.LogInformation($"Seed database associated with context {nameof(AccountDatabaseContextSeed)}");
        }
    }

    private static List<Domain.Entities.Account> GetPreconfiguredAccounts()
    {
        // my own account with balance $5,500
        return
        [
            new Domain.Entities.Account(Guid.Parse("a3372135-ea3d-4eb9-8209-5a36634b2bba"), Guid.Parse("ef533977-e666-4c75-ac4e-ea1de9ea4aef"),
                5_500)
        ];
    }
    }
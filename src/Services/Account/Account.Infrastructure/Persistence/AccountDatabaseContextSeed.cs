using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Account.Infrastructure.Persistence;

public abstract class AccountDatabaseContextSeed
    {
    public static async Task SeedDataAsync(AccountDatabaseContext accountContext, ILogger<AccountDatabaseContextSeed> logger)
    {
        if (!accountContext.Accounts.Any())
        {
            accountContext.Accounts.AddRange(GetPreconfiguredAccounts()!);
            await accountContext.SaveChangesAsync();
            logger.LogInformation($"Seed database associated with context {nameof(AccountDatabaseContextSeed)}");
        }
    }

    private static List<Domain.Entities.Account>? GetPreconfiguredAccounts()
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Accounts.json");
        var jsonData = File.ReadAllText(jsonFilePath);
        var accounts = JsonConvert.DeserializeObject<List<Domain.Entities.Account>>(jsonData);
            
        return accounts;
    }
    }
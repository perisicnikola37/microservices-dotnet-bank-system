using EventBus.Messages.Events;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Transaction.API;

public abstract class TransactionDatabaseContextSeed
    {
    public static void SeedData(IMongoCollection<Entities.Transaction> transactionCollection)
    {
        Console.WriteLine("c2");
        var existTransaction = transactionCollection.Find(p => true).Any();
        if (!existTransaction)
        {
            transactionCollection.InsertManyAsync(GetPreconfiguredTransactions());
        }
    }
    private static List<Entities.Transaction>? GetPreconfiguredTransactions()
    {
        Console.WriteLine("c3");
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/Transactions.json");
        var jsonData = File.ReadAllText(jsonFilePath);
        var transactions = JsonConvert.DeserializeObject<List<Entities.Transaction>>(jsonData);
        
        return transactions;
    }
    }

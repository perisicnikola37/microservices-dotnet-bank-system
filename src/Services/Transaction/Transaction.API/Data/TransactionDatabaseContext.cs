using MongoDB.Driver;
using Transaction.API.Data.Interfaces;

namespace Transaction.API.Data
{
    public class TransactionDatabaseContext : ITransactionContext
    {
        public TransactionDatabaseContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            var collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");
            if (collectionName != null && !CollectionExists(database, collectionName))
            {
                database.CreateCollection(collectionName);
            }
            else
            {
                Console.WriteLine($"Collection '{collectionName}' already exists.");
            }

            Transactions = database.GetCollection<Entities.Transaction>(collectionName);
            CreateIndexes();

            // if (configuration.GetValue<bool>("RunMigrations"))
            // {
            Console.WriteLine("c1");
                TransactionDatabaseContextSeed.SeedData(Transactions);
            // }
        }

        private static bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var collections = database.ListCollectionNames().ToList();
            return collections.Contains(collectionName);
        }

        private void CreateIndexes()
        {
            // Indexing by `AccountId`
            var accountIdIndex = Builders<Entities.Transaction>.IndexKeys.Ascending(x => x.AccountId);
            Transactions.Indexes.CreateOne(new CreateIndexModel<Entities.Transaction>(accountIdIndex));

            // Indexing by `CreatedDate`
            var createdDateIndex = Builders<Entities.Transaction>.IndexKeys.Ascending(x => x.CreatedDate);
            Transactions.Indexes.CreateOne(new CreateIndexModel<Entities.Transaction>(createdDateIndex));
        }

        public IMongoCollection<Entities.Transaction> Transactions { get; }
    }
}

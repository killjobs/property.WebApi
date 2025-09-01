using MongoDB.Driver;

namespace property.Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) { 
            string lowerName = name.ToLower();
            return database.GetCollection<T>(lowerName);
        }

    }
}

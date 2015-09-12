using MongoDB.Driver;

namespace MongoDb.HelloWorld.Console.Domain
{
    public class ItemsService
    {
        // constructor
        public ItemsService(MongoClient client)
        {
            // Get database (if it does not exists it will be created)
            var database = client.GetDatabase("db");

            // Get collections (if it does not exists it will be created)
            MyItems = database.GetCollection<Item>("itemsCollection");
        }

        // collections
        public IMongoCollection<Item> MyItems { get; }
    }
}
using System;

using MongoDB.Bson.Serialization.Attributes;

namespace MongoDb.HelloWorld.Console.Domain
{
    public class Item
    {
        public Guid Id { get; set; }

        [BsonElement("type")]
        public string DocumentType => "Item";

        public string Name { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ItemVersion { get; set; }

        public bool Deleted { get; set; }

        public DateTime DeletedDate { get; set; }

        /// <summary>
        ///     Gets a sample document.
        /// </summary>
        public static Item GetSampleDocument(string name)
            => new Item { Name = name, ModifiedDate = DateTime.Now, ItemVersion = 101, Deleted = false, DeletedDate = DateTime.MinValue };
    }
}
using MongoDb.HelloWorld.Console.Domain;

using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDb.HelloWorld.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var client = new MongoClient("<your mongodb connection string here>");

            // ## Documents
            DocumentsDemo(client);

            // ## Blobs
            BlobsDemo(client);

            // ## Blobs with meta-data
            // BlobsAndMetadataDemo(client);

            System.Console.ReadLine();
        }

        private static void DocumentsDemo(MongoClient client)
        {
            var itemsService = new ItemsService(client);

            // ## Create data
            var doc1 = Item.GetSampleDocument("mongodb");
            var doc2 = Item.GetSampleDocument("csharp6");

            // If document id is null it will be set automatically
            itemsService.MyItems.InsertOneAsync(doc1).Wait();
            itemsService.MyItems.InsertOneAsync(doc2).Wait();

            // List all documents
            System.Console.WriteLine("List all documents:");
            foreach (var item in itemsService.MyItems.Find(_ => true).ToListAsync().Result)
            {
                System.Console.WriteLine("Document: {0} - size: {1} bytes", item.Name, item.ToBson().Length);
            }

            // Find data
            System.Console.WriteLine("Find mongodb:");
            var result = itemsService.MyItems.Find(x => x.Name == "mongodb").ToListAsync().Result;
            foreach (var item in result)
            {
                System.Console.WriteLine("Document: {0} - size: {1} bytes", item.Name, item.ToBson().Length);
            }
        }

        private static void BlobsDemo(MongoClient client)
        {
            var blobsService = new BlobService(client);

            // Upload blob
            blobsService.UploadBlob(@".\sampleFiles\blob1");
            blobsService.UploadBlob(@".\sampleFiles\blob2");

            // List all blobs
            foreach (var document in blobsService.BlobMetadata.Find(_ => true).ToListAsync().Result)
            {
                var fileName = document.GetElement("filename").Value.ToString();
                var size = int.Parse(document.GetElement("length").Value.ToString());

                System.Console.WriteLine("Blob: {0} - size: {1} KB", fileName, size / 1024);

                // Download blob
                blobsService.DownloadBlob(fileName);
            }
        }

        private static void BlobsAndMetadataDemo(MongoClient client)
        {
            // Services
            var blobsService = new BlobService(client);

            // ## Create data
            var doc1 = Item.GetSampleDocument("mongodb");
            var doc2 = Item.GetSampleDocument("csharp6");

            // Add blobs and set the document has meta-data
            blobsService.UploadBlob(@".\sampleFiles\blob1", doc1);
            blobsService.UploadBlob(@".\sampleFiles\blob2", doc2);

            // List all blobs
            foreach (var document in blobsService.BlobMetadata.Find(_ => true).ToListAsync().Result)
            {
                var fileName = document.GetElement("filename").Value.ToString();
                var size = int.Parse(document.GetElement("length").Value.ToString());

                System.Console.WriteLine("Blob: {0} - size: {1} KB", fileName, size / 1024);
            }
        }
    }
}
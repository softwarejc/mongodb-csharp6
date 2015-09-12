using System;
using System.IO;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MongoDb.HelloWorld.Console.Domain
{
    public class BlobService
    {
        // privates
        private readonly IGridFSBucket _databaseBlobs;

        // constructor
        public BlobService(MongoClient client)
        {
            // Get database (if it does not exists it will be created)
            var database = client.GetDatabase("db");

            // Get blobs bucket
            _databaseBlobs = new GridFSBucket(database);

            // Get collections (if it does not exists it will be created)
            BlobMetadata = database.GetCollection<BsonDocument>("fs.files");
        }

        // collections
        public IMongoCollection<BsonDocument> BlobMetadata { get; }

        // public methods
        /// <summary>
        ///     Downloads a BLOB.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        public void DownloadBlob(string blobName)
        {
            using (var fileStream = new FileStream(blobName, FileMode.OpenOrCreate))
            {
                // read bytes
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, (int)fileStream.Length);

                _databaseBlobs.DownloadToStreamByNameAsync(blobName, fileStream).Wait();
            }
        }

        /// <summary>
        ///     Uploads a BLOB.
        /// </summary>
        public Guid UploadBlob(string filePath, Item metadata = null)
        {
            var blobMetadata = metadata.ToBsonDocument();

            var id = Guid.NewGuid();

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                // read bytes
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, (int)fileStream.Length);

                _databaseBlobs.UploadFromBytesAsync(
                    id.ToString(),
                    bytes,
                    new GridFSUploadOptions { BatchSize = 512, ChunkSizeBytes = 1024, Metadata = blobMetadata }).Wait();
            }

            return id;
        }
    }
}
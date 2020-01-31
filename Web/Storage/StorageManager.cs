using Microsoft.Azure.Storage.Blob;
using Maximiz.Storage.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Logging;
using Laixer.AppSettingsValidation.Exceptions;

namespace Maximiz.Storage
{

    /// <summary>
    /// This class manages our storage. 
    /// TODO Clean up
    /// </summary>
    public sealed class StorageManager : IStorageManager
    {

        private ILogger logger;
        private readonly string connectionString;
        private readonly string accountName;
        private readonly string accountKey;
        private readonly Uri cdnUri;

        // TODO How to keep track of container?
        private readonly string containerName = "my-container";

        /// <summary>
        /// Constructor for dependency injection and connection string setup.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="loggerFactory"><see cref="ILoggerFactory"/></param>
        public StorageManager(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) { throw new ConfigurationException("Missing logger factory for storage manager"); }
            logger = loggerFactory.CreateLogger(nameof(StorageManager));

            connectionString = configuration.GetConnectionString("StorageAccountImages");
            if (string.IsNullOrEmpty(connectionString)) { throw new ConfigurationException("Missing connection string for storage manager"); }

            var cdnUriString = configuration.GetValue<string>("CdnImageUri");
            if (string.IsNullOrEmpty(cdnUriString)) { throw new ConfigurationException("Missing CDN URI for storage manager"); }
            cdnUri = new Uri(cdnUriString);

            var extractor = new ConnectionStringExtractor(connectionString);
            if (!extractor.HasKey("AccountName")) { throw new ConfigurationException("Connection string does not contain account name"); }
            if (!extractor.HasKey("AccountKey")) { throw new ConfigurationException("Connection string does not contain account key"); }
            accountName = extractor.GetValue("AccountName");
            accountKey = extractor.GetValue("AccountKey");
        }

        /// <summary>
        /// Attempts to upload a file.
        /// </summary>
        /// <remarks>
        /// If the upload fails, the exception gets caught and logged. The 
        /// function will return false in this case.
        /// </remarks>
        /// <param name="file"></param>
        /// <returns>True if successful</returns>
        public async Task<bool> UploadFile(IFormFile file)
        {
            try
            {
                var storageCredentials = new StorageCredentials(accountName, accountKey);
                var storageAccount = new CloudStorageAccount(storageCredentials, true);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                await container.SetPermissionsAsync(new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                // TODO Should we use a GUID for this?
                var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
                using (var stream = file.OpenReadStream())
                {
                    blob.Properties.ContentType = file.ContentType;
                    await blob.UploadFromStreamAsync(stream);
                }

                return true;
            } catch (Exception e)
            {
                logger.LogError(e, "Could not upload file");
                return false;
            }
        }

        /// <summary>
        /// Retrieves all uploaded images from the blob storage.
        /// TODO Clean up
        /// </summary>
        /// <param name="query">Search query string</param>
        /// <returns><see cref="Uri"/> list pointing to CDN images</returns>
        public async Task<IEnumerable<Uri>> GetUploadedImages(string query = null)
        {
            StorageCredentials credentials = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            var blobs = await container.ListBlobsSegmentedAsync(new BlobContinuationToken());

            var cdnUris = new List<Uri>();
            foreach (var blob in blobs.Results)
            {
                cdnUris.Add(new Uri(cdnUri, blob.Uri.LocalPath));
            }
            return cdnUris;
        }

    }
}

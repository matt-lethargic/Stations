using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Stations.Core.Interfaces;
using Stations.Core.Interfaces.Events;
using Stations.Core.SharedKernel;

namespace Stations.Infrastructure.Data
{
    public class FlatFileRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly CloudBlobContainer _container;
        private readonly CloudBlobClient _blobClient;

        private readonly IDomainEventDispatcher _dispatcher;

        public FlatFileRepository(IDomainEventDispatcher dispatcher)
        {
            var creds = new StorageCredentials("", "");
            CloudStorageAccount storageAccount = new CloudStorageAccount(creds, true);
           
            _blobClient = storageAccount.CreateCloudBlobClient();
            _container = _blobClient.GetContainerReference("data");
            _dispatcher = dispatcher;
        }

        public async Task Save(T entity)
        {
            await _container.CreateIfNotExistsAsync();

            string entityReference = $"{typeof(T).Name}\\{entity.Id.ToString()}";

            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(entityReference);

            var dataJson = JsonConvert.SerializeObject(entity);
            var dataArray = Encoding.ASCII.GetBytes(dataJson);
            await blockBlob.UploadFromByteArrayAsync(dataArray, 0, dataArray.Length);


            var events = entity.Events.ToArray();
            entity.Events.Clear();
            foreach(var domainEvent in events)
            {
                await _dispatcher.Dispatch(domainEvent);
            }
        }

        public async Task<T> GetById(Guid id)
        {
            await _container.CreateIfNotExistsAsync();

            string entityReference = $"{typeof(T).Name}\\{id.ToString()}";

            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(entityReference);
            var dataJson = await blockBlob.DownloadTextAsync();

            return JsonConvert.DeserializeObject<T>(dataJson);
        }

        public async Task<IList<T>> List()
        {
            await _container.CreateIfNotExistsAsync();

            string entityFolder = $"{typeof(T).Name}";

            var blobs = _container.ListBlobs(prefix: entityFolder, useFlatBlobListing: true);

            var results = new List<T>();

            foreach (IListBlobItem blobItem in blobs)
            {
                var blockBlob = new CloudBlockBlob(blobItem.Uri, _blobClient);
                var dataJson = await blockBlob.DownloadTextAsync();

                var entity = JsonConvert.DeserializeObject<T>(dataJson);
                results.Add(entity);
            }

            return results;
        }
    }
}

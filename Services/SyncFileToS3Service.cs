using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.Storage;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public  interface ISyncFileToS3Service
    {
        Task<int> SyncAsync();
    }
    public class SyncFileToS3Service: ISyncFileToS3Service, IScopedLifetime
    {
        private readonly IStorageService _storageService;
        private readonly LocalStorageService _localStorageService;
        private readonly ICustomerRepository _customerRepository;

        public SyncFileToS3Service(
            IStorageService storageService,
            LocalStorageService localStorageService,
            ICustomerRepository customerRepository)
        {
            _storageService = storageService;
            _localStorageService = localStorageService;
            _customerRepository = customerRepository;
        }

        public async Task<int> SyncAsync()
        {
            var customers = _customerRepository.FilterBy(x => true);
            var customerToUpdates = new List<Customer>();
            foreach (var customer in customers.Where(x => x.Documents?.Any() == true))
            {
                bool isUpdate = false;
                foreach (var document in customer.Documents.Where(x => x.Documents?.Any() == true))
                {
                    foreach (var doc in document.Documents.Where(x => x.UploadedMedias?.Any() == true))
                    {
                        foreach (var media in doc.UploadedMedias)
                        {
                            try
                            {
                                var file = await _localStorageService.GetObjectAsync(media.Uri);
                                var fileNew = await _storageService.UploadFileAsync(file.GetDirectory(), file.FileName, file.Bytes);
                                media.Uri = fileNew.AbsolutePath;
                                isUpdate = true;
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                if(customer.RecordFile != null)
                {
                    try
                    {
                        var file = await _localStorageService.GetObjectAsync(customer.RecordFile.Uri);
                        var fileNew = await _storageService.UploadFileAsync(file.GetDirectory(), file.FileName, file.Bytes);
                        customer.RecordFile.Uri = fileNew.AbsolutePath;
                        isUpdate = true;
                    }
                    catch (Exception)
                    {
                    }
                }

                if(isUpdate)
                {
                    customer.ModifiedDate = DateTime.Now;
                    customerToUpdates.Add(customer);
                }
            }

            if(customerToUpdates.Any())
            {
                await UpdateManyAsync(customerToUpdates);
            }

            return customerToUpdates.Count;
        }

        private async Task UpdateManyAsync(IEnumerable<Customer> customers)
        {
            var listOfReplaceOneModels = customers.Select(customer => new ReplaceOneModel<Customer>(Builders<Customer>.Filter.Where(y => y.Id == customer.Id), customer));
            await _customerRepository.GetCollection().BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}

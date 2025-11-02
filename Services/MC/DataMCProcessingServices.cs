using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public class DataMCProcessingServices : IScopedLifetime
    {
        private readonly ILogger<DataMCProcessingServices> _logger;
        private readonly IMongoCollection<DataMCProcessing> _dataMCProcessing;
        private readonly IMapper _mapper;

        private readonly ICustomerRepository _customerRepository;

        public DataMCProcessingServices(
            ILogger<DataMCProcessingServices> logger,
            IMongoDbConnection connection,
            IMapper mapper,
            ICustomerRepository customerRepository)
        {
            _logger = logger;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _dataMCProcessing = database.GetCollection<DataMCProcessing>(Common.MongoCollection.DataMCProcessing);
            _mapper = mapper;
            _customerRepository = customerRepository;
        }
        public DataMCProcessing CreateOne(DataMCProcessing dataMC)
        {
            var newData = new DataMCProcessing();
            try
            {
                var entity = _dataMCProcessing.Find(x => x.Status == dataMC.Status && x.CustomerId == dataMC.CustomerId).FirstOrDefault();
                if (entity == null)
                {
                    _dataMCProcessing.InsertOne(dataMC);
                    return dataMC;
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return newData;
        }
        public long UpdateById(string id, DataMCProcessing body)
        {
            try
            {
                var dataMC = _dataMCProcessing.Find(d => d.Id == id).FirstOrDefault();
                dataMC.Status = body.Status;
                dataMC.FinishDate = DateTime.Now;
                dataMC.PayLoad = body.PayLoad;
                dataMC.Message = body.Message;
                dataMC.Response = body.Response;
                var modifiedCount = _dataMCProcessing.ReplaceOne(d => d.Id == dataMC.Id, dataMC).ModifiedCount;
                return modifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public List<DataMCProcessing> GetDataMCProcessings(string status)
        {
            var listDataCRMProcessing = new List<DataMCProcessing>();
            try
            {
                listDataCRMProcessing = _dataMCProcessing.Find(d => d.Status == status).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return listDataCRMProcessing;
        }

        public async Task<PagingResponse<DataMCProcessingResponse>> GetAsync(GetDataMCProcessingRequest getDataMCProcessingRequest)
        {
            try
            {
                var filter = Builders<DataMCProcessing>.Filter.Eq(x => x.CustomerId, getDataMCProcessingRequest.CustomerId);
                var dataMCProcessings = await _dataMCProcessing.Find(filter)
                    .SortByDescending(c => c.CreateDate)
                    .Skip((getDataMCProcessingRequest.PageIndex - 1) * getDataMCProcessingRequest.PageSize)
                    .Limit(getDataMCProcessingRequest.PageSize).ToListAsync();

                var total = await _dataMCProcessing.Find(filter).CountDocumentsAsync();

                var dataMCProcessingDtos = _mapper.Map<IEnumerable<DataMCProcessingResponse>>(dataMCProcessings);

                var result = new PagingResponse<DataMCProcessingResponse>
                {
                    TotalRecord = total,
                    Data = dataMCProcessingDtos
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<DataMCProcessing> Resend(string customerId)
        {
            try
            {
                var customerDetail = _customerRepository.FindById(customerId);

                if (customerDetail == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var newRecord = new DataMCProcessing
                {
                    CreateDate = DateTime.Now,
                    CustomerId = customerId,
                    Status = DataCRMProcessingStatus.InProgress
                };

                await _dataMCProcessing.InsertOneAsync(newRecord);

                return newRecord;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

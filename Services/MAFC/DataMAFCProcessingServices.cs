using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public class DataMAFCProcessingServices : IScopedLifetime
    {
        private readonly ILogger<DataMAFCProcessingServices> _logger;
        private readonly IMongoCollection<DataMAFCProcessingModel> _collection;
        private readonly IMapper _mapper;

        public DataMAFCProcessingServices(
            ILogger<DataMAFCProcessingServices> logger,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<DataMAFCProcessingModel>(Common.MongoCollection.DataMAFCProcessing);
            _mapper = mapper;
        }
        public async Task CreateOneAsync(DataMAFCProcessingModel model)
        {
            try
            {
                var entity = _collection.Find(x => x.Status == model.Status && x.CustomerId == model.CustomerId).FirstOrDefault();
                if (entity == null)
                {
                    var pendingEntity = _collection.Find(x => x.Status == DataProcessingStatus.PENDING && x.CustomerId == model.CustomerId).FirstOrDefault();
                    if (pendingEntity != null)
                    {
                        pendingEntity.Status = DataProcessingStatus.IN_PROGRESS;
                        await _collection.ReplaceOneAsync(x => x.Id == pendingEntity.Id, pendingEntity);
                    }
                    else
                    {
                        await _collection.InsertOneAsync(model);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public async Task<string> CreatePushDeferAsync(string customerId)
        {
            try
            {
                DataMAFCProcessingModel model = new DataMAFCProcessingModel()
                {
                    CustomerId = customerId,
                    Status = DataProcessingStatus.PUSH_STAGE,
                    Step = "STAGE",
                };
                await _collection.InsertOneAsync(model);
                return model.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public long AddPayload(string id, PayloadModel payload)
        {
            try
            {
                var data = _collection.Find(d => d.Id == id).FirstOrDefault();
                var temp = new List<PayloadModel>();
                if (data.Payloads != null)
                {
                    temp = data.Payloads.ToList();
                    temp.Add(payload);
                }
                else
                {
                    temp.Add(payload);
                }
                data.Payloads = temp;
                var modifiedCount = _collection.ReplaceOne(d => d.Id == data.Id, data).ModifiedCount;
                return modifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }
        public long UpdateStep(string id, string step)
        {
            try
            {
                var data = _collection.Find(d => d.Id == id).FirstOrDefault();
                data.Step = step;
                var modifiedCount = _collection.ReplaceOne(d => d.Id == data.Id, data).ModifiedCount;
                return modifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public long UpdateStatus(string id, string status, string message = "")
        {
            try
            {
                var data = _collection.Find(d => d.Id == id).FirstOrDefault();
                data.Status = status;
                data.Message = message;
                var modifiedCount = _collection.ReplaceOne(d => d.Id == data.Id, data).ModifiedCount;
                return modifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }
        public long UpdateById(string id, DataMAFCProcessingModel body)
        {
            try
            {
                var data = _collection.Find(d => d.Id == id).FirstOrDefault();
                data.Status = body.Status;
                data.FinishDate = DateTime.Now;
                data.Payloads = body.Payloads;
                data.Message = body.Message;
                var modifiedCount = _collection.ReplaceOne(d => d.Id == data.Id, data).ModifiedCount;
                return modifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public List<DataMAFCProcessingModel> GetDataMAFCProcessings(string status)
        {
            try
            {
                return _collection.Find(d => d.Status == status).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public async Task<DataMAFCProcessingModel> GetByCustomerId(string customerId)
        {
            try
            {
                return await _collection.Find(d => d.CustomerId == customerId && d.Status == DataProcessingStatus.IN_PROGRESS).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public async Task<DataMAFCProcessingModel> GetDataMAFCProcessingByIdAsync(string id)
        {
            try
            {
                return await _collection.Find(d => d.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<PagingResponse<DataMAFCProcessingDto>> GetAsync(GetDataMAFCProcessingRequest getDataMAFCProcessingRequest)
        {
            try
            {
                var filter = Builders<DataMAFCProcessingModel>.Filter.Eq(x => x.CustomerId, getDataMAFCProcessingRequest.CustomerId);
                var dataMAFCProcessings = await _collection.Find(filter)
                    .SortByDescending(c => c.CreateDate)
                    .Skip((getDataMAFCProcessingRequest.PageIndex - 1) * getDataMAFCProcessingRequest.PageSize)
                    .Limit(getDataMAFCProcessingRequest.PageSize).ToListAsync();

                var total = await _collection.Find(filter).CountDocumentsAsync();

                var dataMAFCProcessingDtos = _mapper.Map<IEnumerable<DataMAFCProcessingDto>>(dataMAFCProcessings);

                var result = new PagingResponse<DataMAFCProcessingDto>
                {
                    TotalRecord = total,
                    Data = dataMAFCProcessingDtos
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateDataMAFCProcessingRequest updateDataMAFCProcessingRequest)
        {
            try
            {
                var dataProcessing = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (dataProcessing == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(DataMAFCProcessingModel)));
                }

                _mapper.Map(updateDataMAFCProcessingRequest, dataProcessing);

                await _collection.ReplaceOneAsync(x => x.Id == id, dataProcessing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

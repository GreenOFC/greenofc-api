using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IGroupNotificationRepository
    {
        Task<GroupNotification> Get(string id);
        Task<GroupNotification> Create(CreateGroupNotificationDto request);
        Task<IEnumerable<GroupNotification>> CreateManyAsync(ICollection<GroupNotification> request);
        Task<GroupNotification> GetDetailByCode(string groupCode);
        Task<GroupNotification> GetDetailByName(string groupName);
        Task<IEnumerable<GroupNotification>> GetAllGroupNotification();
        Task<GroupNotification> Update(GroupNotification request);
        Task<IEnumerable<GroupNotification>> GetList(string textSearch = null, int pageIndex = 1, int pageSize = 10);
        Task<long> CountGroupNotification(string textSearch = null);
        Task Delete(string id);
    }

    public class GroupNotificationRepository : IGroupNotificationRepository, IScopedLifetime
    {
        private readonly ILogger<GroupNotificationRepository> _logger;
        private readonly IMongoRepository<GroupNotification> _groupNotificationRepository;

        private readonly ICounterRepository _counterRepository;

        public GroupNotificationRepository(
            ILogger<GroupNotificationRepository> logger,
            IMongoRepository<GroupNotification> groupNotification,
            ICounterRepository counterRepository
            )
        {
            _logger = logger;
            _groupNotificationRepository = groupNotification;
            _counterRepository = counterRepository;
        }

        public async Task<GroupNotification> Create(CreateGroupNotificationDto request)
        {
            try
            {
                var groupNotification = new GroupNotification
                {
                    GroupName = request.GroupName,
                    GroupCode = "GRC" + await _counterRepository.GetNextSequenceAsync(nameof(GroupNotification), 2000)
                };
                await _groupNotificationRepository.InsertOneAsync(groupNotification);
                return groupNotification;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotification>> CreateManyAsync(ICollection<GroupNotification> request)
        {
            try
            {
                await _groupNotificationRepository.InsertManyAsync(request);
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupNotification> GetDetailByCode(string groupCode)
        {
            try
            {
                return await _groupNotificationRepository.FindOneAsync(x => x.GroupCode.Equals(groupCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotification>> GetAllGroupNotification()
        {
            try
            {
                var groupNotificationList = (await _groupNotificationRepository.FilterByAsync(x => true)).ToList();
                return groupNotificationList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupNotification> Get(string id)
        {
            try
            {
                var groupNotificationDetail = await _groupNotificationRepository.FindOneAsync(x => x.Id == id);
                return groupNotificationDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupNotification> Update(GroupNotification request)
        {
            try
            {
                var detail = Get(request.Id);

                if (detail != null)
                {
                    await _groupNotificationRepository.ReplaceOneAsync(request);
                }

                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotification>> GetList(string textSearch = null, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var filter = Builders<GroupNotification>.Filter.Empty;

                if (!textSearch.IsEmpty())
                {
                    var regex = new BsonRegularExpression($"/{textSearch}/i");

                    filter &= Builders<GroupNotification>.Filter.Regex(x => x.GroupCode, regex) |
                        Builders<GroupNotification>.Filter.Regex(x => x.GroupName, regex);
                }

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "GroupName", 1 },
                        { "GroupCode", 1 }
                    };

                IEnumerable<GroupNotification> result = await _groupNotificationRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(filter)
                                                                            .Project(mapping)
                                                                            .As<GroupNotification>()
                                                                            .Skip((pageIndex - 1) * pageSize)
                                                                            .Limit(pageSize)
                                                                            .SortByDescending(x => x.CreatedDate)
                                                                            .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<long> CountGroupNotification(string textSearch = null)
        {
            try
            {
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var filter = Builders<GroupNotification>.Filter.Empty;

                if (!textSearch.IsEmpty())
                {
                    var regex = new BsonRegularExpression($"/{textSearch}/i");

                    filter &= Builders<GroupNotification>.Filter.Regex(x => x.GroupCode, regex) |
                        Builders<GroupNotification>.Filter.Regex(x => x.GroupName, regex);
                }

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "GroupName", 1 },
                        { "GroupCode", 1 }
                    };

                IEnumerable<GroupNotification> result = await _groupNotificationRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(filter)
                                                                            .Project(mapping)
                                                                            .As<GroupNotification>()
                                                                            .ToListAsync();

                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupNotification> GetDetailByName(string groupName)
        {
            try
            {
                return await _groupNotificationRepository.FindOneAsync(x => x.GroupName.Equals(groupName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _groupNotificationRepository.DeleteOneAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

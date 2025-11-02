using _24hplusdotnetcore.Extensions;
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
    public interface IGroupNotificationUserRepository
    {
        Task<GroupNotificationUser> Create(GroupNotificationUser request);
        Task<GroupNotificationUser> CheckExist(string userId, string groupId);
        Task<IEnumerable<GroupNotificationUserResponse>> GetUserInGroup(string groupNotificationId, string textSearch = null, int pageIndex = 1, int pageSize = 10);
        Task<IEnumerable<GroupNotificationUser>> GetUserInGroup(IEnumerable<string> groupNotificationIds);
        Task<long> CountAsync(string groupNotificationId, string textSearch = null);
        Task RemoveUserFromGroup(string userId);
        Task RemoveUserFromGroup(string userId, string groupNotificationId);
        Task CreateMany(ICollection<GroupNotificationUser> request);
        Task Delete(string id);

        Task<IEnumerable<GroupNotificationUserResponse>> GetUserRegistrationToken(string groupNotificationId);
    }

    public class GroupNotificationUserRepository : IGroupNotificationUserRepository, IScopedLifetime
    {
        private readonly ILogger<GroupNotificationUserRepository> _logger;
        private readonly IMongoRepository<GroupNotificationUser> _groupNotificationUserRepository;

        public GroupNotificationUserRepository(ILogger<GroupNotificationUserRepository> logger, IMongoRepository<GroupNotificationUser> groupNotificationUser)
        {
            _logger = logger;
            _groupNotificationUserRepository = groupNotificationUser;
        }

        public async Task<GroupNotificationUser> CheckExist(string userId, string groupNotificationId)
        {
            try
            {
                return await _groupNotificationUserRepository.FindOneAsync(x => x.UserId == userId && x.GroupNotificationId == groupNotificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupNotificationUser> Create(GroupNotificationUser request)
        {
            try
            {
                await _groupNotificationUserRepository.InsertOneAsync(request);
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task CreateMany(ICollection<GroupNotificationUser> request)
        {
            try
            {
                foreach (var item in request)
                {
                    var isExist = await _groupNotificationUserRepository.FindOneAsync(x => x.UserId == item.UserId && x.GroupNotificationId == item.GroupNotificationId);

                    if (isExist == null)
                    {
                        await _groupNotificationUserRepository.InsertManyAsync(request);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _groupNotificationUserRepository.DeleteOneAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotificationUserResponse>> GetUserInGroup(string groupNotificationId, string textSearch = null, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var groupnotificationUsers = await _groupNotificationUserRepository.FilterByAsync(x => x.GroupNotificationId == groupNotificationId);
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var preCondition = Builders<GroupNotificationUser>.Filter.Empty;

                if(!groupNotificationId.IsEmpty())
                {
                    preCondition &= Builders<GroupNotificationUser>.Filter.Eq(x => x.GroupNotificationId, groupNotificationId);
                }

                var textSearchFilter = Builders<GroupNotificationUserResponse>.Filter.Empty;

                if (!string.IsNullOrEmpty(textSearch))
                {
                    var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                    textSearchFilter &= Builders<GroupNotificationUserResponse>.Filter.Regex(x => x.GroupNotificationLookupResult.GroupName, regex) |
                        Builders<GroupNotificationUserResponse>.Filter.Regex(x => x.UserLookupResult.FullName, regex);
                }

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "GroupNotificationLookupResult.GroupName", 1 },
                        { "GroupNotificationLookupResult.GroupCode", 1 },
                        { "GroupNotificationLookupResult._id", 1 },
                        { "UserLoginLookupResult._id", 1 },
                        { "UserLoginLookupResult.UserId", 1 },
                        { "UserLoginLookupResult.UserName", 1 },
                        { "UserLoginLookupResult.registration_token", 1 },
                        { "UserLookupResult._id", 1 },
                        { "UserLookupResult.UserName", 1 },
                        { "UserLookupResult.FullName", 1 },
                        { "UserLookupResult.Phone", 1 },
                        { "UserLookupResult.UserEmail", 1 },

                    };

                IEnumerable<GroupNotificationUserResponse> result = await _groupNotificationUserRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(preCondition)
                                                                            .SortByDescending(x => x.CreatedDate)
                                                                            .Lookup("UserLogin", "UserId", "UserId", "UserLoginLookupResult")
                                                                            .Unwind("UserLoginLookupResult", unwindOption)
                                                                            .Lookup("GroupNotification", "GroupNotificationId", "_id", "GroupNotificationLookupResult")
                                                                            .Unwind("GroupNotificationLookupResult", unwindOption)
                                                                            .Lookup("Users", "UserId", "_id", "UserLookupResult")
                                                                            .Unwind("UserLookupResult", unwindOption)
                                                                            .Project(mapping)
                                                                            .As<GroupNotificationUserResponse>()
                                                                            .Match(textSearchFilter)
                                                                            .Skip((pageIndex - 1) * pageSize)
                                                                            .Limit(pageSize)
                                                                            .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<long> CountAsync(string groupNotificationId, string textSearch = null)
        {
            try
            {
                var groupnotificationUsers = await _groupNotificationUserRepository.FilterByAsync(x => x.GroupNotificationId == groupNotificationId);
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var preCondition = Builders<GroupNotificationUser>.Filter.Empty;

                if (!groupNotificationId.IsEmpty())
                {
                    preCondition &= Builders<GroupNotificationUser>.Filter.Eq(x => x.GroupNotificationId, groupNotificationId);
                }

                var textSearchFilter = Builders<GroupNotificationUserResponse>.Filter.Empty;

                if (!string.IsNullOrEmpty(textSearch))
                {
                    var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                    textSearchFilter &= Builders<GroupNotificationUserResponse>.Filter.Regex(x => x.GroupNotificationLookupResult.GroupName, regex) |
                        Builders<GroupNotificationUserResponse>.Filter.Regex(x => x.UserLookupResult.FullName, regex);
                }

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "GroupNotificationLookupResult.GroupName", 1 },
                        { "GroupNotificationLookupResult.GroupCode", 1 },
                        { "GroupNotificationLookupResult._id", 1 },
                        { "UserLoginLookupResult._id", 1 },
                        { "UserLoginLookupResult.UserId", 1 },
                        { "UserLoginLookupResult.UserName", 1 },
                        { "UserLoginLookupResult.registration_token", 1 },
                        { "UserLookupResult._id", 1 },
                        { "UserLookupResult.UserName", 1 },
                        { "UserLookupResult.FullName", 1 },
                        { "UserLookupResult.Phone", 1 },
                        { "UserLookupResult.UserEmail", 1 },

                    };

                IEnumerable<GroupNotificationUserResponse> result = await _groupNotificationUserRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(preCondition)
                                                                            .Lookup("UserLogin", "UserId", "UserId", "UserLoginLookupResult")
                                                                            .Unwind("UserLoginLookupResult", unwindOption)
                                                                            .Lookup("GroupNotification", "GroupNotificationId", "_id", "GroupNotificationLookupResult")
                                                                            .Unwind("GroupNotificationLookupResult", unwindOption)
                                                                            .Lookup("Users", "UserId", "_id", "UserLookupResult")
                                                                            .Unwind("UserLookupResult", unwindOption)
                                                                            .Project(mapping)
                                                                            .As<GroupNotificationUserResponse>()
                                                                            .Match(textSearchFilter)
                                                                            .ToListAsync();

                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task RemoveUserFromGroup(string userId)
        {
            try
            {
                await _groupNotificationUserRepository.DeleteManyAsync(x => x.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task RemoveUserFromGroup(string userId, string groupNotificationId)
        {
            try
            {
                await _groupNotificationUserRepository.DeleteOneAsync(x => x.UserId == userId && x.GroupNotificationId == groupNotificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotificationUserResponse>> GetUserRegistrationToken(string groupNotificationId)
        {
            try
            {
                var groupnotificationUsers = await _groupNotificationUserRepository.FilterByAsync(x => x.GroupNotificationId == groupNotificationId);
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var preCondition = Builders<GroupNotificationUser>.Filter.Empty;

                if (!groupNotificationId.IsEmpty())
                {
                    preCondition &= Builders<GroupNotificationUser>.Filter.Eq(x => x.GroupNotificationId, groupNotificationId);
                }

                var textSearchFilter = Builders<GroupNotificationUserResponse>.Filter.Empty;

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "GroupNotificationLookupResult.GroupName", 1 },
                        { "GroupNotificationLookupResult.GroupCode", 1 },
                        { "GroupNotificationLookupResult._id", 1 },
                        { "UserLoginLookupResult._id", 1 },
                        { "UserLoginLookupResult.UserId", 1 },
                        { "UserLoginLookupResult.UserName", 1 },
                        { "UserLoginLookupResult.registration_token", 1 },
                        { "UserLookupResult._id", 1 },
                        { "UserLookupResult.UserName", 1 },
                        { "UserLookupResult.FullName", 1 },
                        { "UserLookupResult.Phone", 1 },
                        { "UserLookupResult.UserEmail", 1 },

                    };

                IEnumerable<GroupNotificationUserResponse> result = await _groupNotificationUserRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(preCondition)
                                                                            .SortByDescending(x => x.CreatedDate)
                                                                            .Lookup("UserLogin", "UserId", "UserId", "UserLoginLookupResult")
                                                                            .Unwind("UserLoginLookupResult", unwindOption)
                                                                            .Lookup("GroupNotification", "GroupNotificationId", "_id", "GroupNotificationLookupResult")
                                                                            .Unwind("GroupNotificationLookupResult", unwindOption)
                                                                            .Lookup("Users", "UserId", "_id", "UserLookupResult")
                                                                            .Unwind("UserLookupResult", unwindOption)
                                                                            .Project(mapping)
                                                                            .As<GroupNotificationUserResponse>()
                                                                            .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotificationUser>> GetUserInGroup(IEnumerable<string> groupNotificationIds)
        {
            try
            {
                //var filter = Builders<GroupNotificationUser>.Filter.In(x => x.GroupNotificationId, groupNotificationIds);
                return await _groupNotificationUserRepository.FilterByAsync(x => groupNotificationIds.Contains(x.GroupNotificationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}

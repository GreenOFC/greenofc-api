using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.ModelResponses.Pos;
using _24hplusdotnetcore.ModelResponses.Role;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories.Models;
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
    public interface IUserRepository : IMongoRepository<User>
    {
        Task<IEnumerable<GetUserResponse>> GetAsync(UserFilterDto userFilterDto);
        Task<long> CountAsync(UserFilterDto userFilterDto);
        Task<GetUserDetailResponse> GetDetailAsync(string id);
        Task<GetUserProfileResponse> GetProfileAsync(string id);
        Task<IEnumerable<string>> GetTeamMemberAsTeamleadAsync(string userId);
        Task<IEnumerable<string>> GetTeamMemberAsAsmAsync(string userId);
        Task<IEnumerable<string>> GetTeamMemberAsMultiPosAsync(string userId);
        Task<IEnumerable<string>> GetTeamMemberAsHeadOfSaleAdminAsync(string userId);
        Task<IEnumerable<User>> GetTeammemberAsPosLead(string userId, string roleId);
        Task<IEnumerable<User>> GetTeammemberAsAdmin(string userId);
        Task<IEnumerable<GetUserResponse>> GetUserInPos(string posId, string textSearch, int pageIndex = 1, int pageSize = 10, string roleId = null, string teamLeadUserId = null);
        Task<long> CountUserInPosAsync(string posId, string textSearch, string roleId, string teamLeadUserId);
        Task<IEnumerable<User>> GetUserByRoleIds(IEnumerable<string> roleIds, string userId = null, string podId = null, IEnumerable<string> excludeUserIds = null);

        Task UpdateSaleChanelAsync(string posId, SaleChanelInfo saleChanelInfo, string modifier);
        Task UpdateSaleChanelByListPosIdAsync(IEnumerable<string> posIds, SaleChanelInfo saleChanelInfo, string modifier);
        Task<IEnumerable<GetUserTeamLeadResponse>> GetTeamLeadAsync(GetTeamLeadRequest getTeamLeadRequest);
    }

    public class UserRepository : MongoRepository<User>, IUserRepository, IScopedLifetime
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<GetUserResponse>> GetAsync(UserFilterDto userFilterDto)
        {
            var filter = GetFilter(userFilterDto);

            var users = await _collection
                        .Aggregate()
                        .Match(filter)
                        .Sort(new BsonDocument(nameof(User.CreatedDate), -1))
                        .Skip((userFilterDto.PageIndex - 1) * userFilterDto.PageSize)
                        .Limit(userFilterDto.PageSize)
                        .Lookup("Roles", "RoleIds", "_id", "Roles")
                        .As<GetUserResponse>()
                        .ToListAsync();
            return users;
        }

        public async Task<IEnumerable<GetUserResponse>> GetUserInPos(string posId, string textSearch, int pageIndex = 1, int pageSize = 10, string roleId = null, string teamLeadUserId = null)
        {
            try
            {
                var projectMapping = BuildGetUserInPosProjectMapping();
                var filter = GetUserInPosFilter(posId, textSearch, roleId, teamLeadUserId);

                IEnumerable<GetUserResponse> users = await _collection
                        .Aggregate()
                        .Match(filter)
                        .SortByDescending(x => x.CreatedDate)
                        .Lookup("Roles", "RoleIds", "_id", "Roles")
                        .Project(projectMapping)
                        .Skip((pageIndex - 1) * pageSize)
                        .Limit(pageSize)
                        .As<GetUserResponse>()
                        .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<long> CountUserInPosAsync(string posId, string textSearch, string roleId, string teamLeadUserId)
        {
            var filter = GetUserInPosFilter(posId, textSearch, roleId, teamLeadUserId);
            var total = await _collection
                        .Find(filter)
                        .CountDocumentsAsync();

            return total;
        }

        private BsonDocument BuildGetUserInPosProjectMapping()
        {
            return new BsonDocument()
                {
                    { "UserName", 1 },
                    { "FullName", 1 },
                    { "UserEmail", 1 },
                    { "Phone", 1 },
                    { "PosId", 1 },
                    { "TeamLeadUserId", 1 },
                    { "IdCard", 1 },
                    { "TeamLeadInfo._id", 1},
                    { "TeamLeadInfo.UserName", 1},
                    { "TeamLeadInfo.FullName", 1 },
                    { "IsActive", 1 },
                    { "RoleIds", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "Roles", 1 },
                    { "PosInfo", 1 },
                };
        }

        private FilterDefinition<User> GetUserInPosFilter(string posId, string textSearch, string roleId, string teamLeadUserId)
        {
            var filter = Builders<User>.Filter.Empty;

            if (!textSearch.IsEmpty())
            {
                var regex = new BsonRegularExpression($"/{textSearch}/i");
                filter &= Builders<User>.Filter.Regex(user => user.UserName, regex) |
                    Builders<User>.Filter.Regex(user => user.FullName, regex);
            }

            if (!string.IsNullOrEmpty(roleId) && ObjectId.TryParse(roleId, out ObjectId roleObjectId))
            {
                filter &= Builders<User>.Filter.AnyEq(nameof(User.RoleIds), roleObjectId);
            }

            if (!string.IsNullOrEmpty(teamLeadUserId))
            {
                filter &= Builders<User>.Filter.Eq(x => x.TeamLeadInfo.Id, teamLeadUserId);
            }

            if (!string.IsNullOrEmpty(posId))
            {
                filter &= Builders<User>.Filter.Eq(x => x.PosInfo.Id, posId);
            }

            return filter;

        }

        public async Task<long> CountAsync(UserFilterDto userFilterDto)
        {
            var filter = GetFilter(userFilterDto);
            var countResult = await _collection
                .Find(filter)
                .CountDocumentsAsync();

            return countResult;
        }

        private FilterDefinition<User> GetFilter(UserFilterDto userFilterDto)
        {
            var filter = Builders<User>.Filter.Empty;

            if (!userFilterDto.PosId.IsEmpty())
            {
                filter &= Builders<User>.Filter.Eq(x => x.PosInfo.Id, userFilterDto.PosId);
            }
            if (!userFilterDto.SaleChanelId.IsEmpty())
            {
                filter &= Builders<User>.Filter.Eq(x => x.SaleChanelInfo.Id, userFilterDto.SaleChanelId);
            }

            if (userFilterDto.IsActive.HasValue)
            {
                filter &= Builders<User>.Filter.Eq(x => x.IsActive, userFilterDto.IsActive);
            }

            if (!string.IsNullOrEmpty(userFilterDto.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{userFilterDto.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<User>.Filter.Regex(x => x.UserName, regex) |
                    Builders<User>.Filter.Regex(x => x.IdCard, regex) |
                    Builders<User>.Filter.Regex(x => x.FullName, regex);
            }

            if (!string.IsNullOrEmpty(userFilterDto.RoleId) && ObjectId.TryParse(userFilterDto.RoleId, out ObjectId roleObjectId))
            {
                filter &= Builders<User>.Filter.AnyEq(nameof(User.RoleIds), roleObjectId);
            }

            if (!string.IsNullOrEmpty(userFilterDto.TeamLeadUserId))
            {
                filter &= Builders<User>.Filter.Eq(x => x.TeamLeadInfo.Id, userFilterDto.TeamLeadUserId);
            }

            if (userFilterDto.Status.HasValue)
            {
                filter &= Builders<User>.Filter.Eq(x => x.Status, userFilterDto.Status);
            }

            if (userFilterDto.Ids?.Any() == true)
            {
                var ids = userFilterDto.Ids.Select(x => ObjectId.TryParse(x, out ObjectId id) ? id : default);
                filter &= Builders<User>.Filter.In("_id", ids);
            }
            if (userFilterDto.UserIds?.Any() == true)
            {
                var ids = userFilterDto.UserIds.Select(x => ObjectId.TryParse(x, out ObjectId id) ? id : default);
                filter &= Builders<User>.Filter.In(nameof(User.Creator), ids);
            }
            return filter;
        }

        public async Task<GetUserDetailResponse> GetDetailAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            GetUserDetailResponse user = await _collection
                    .Aggregate()
                    .Match(filter)
                    .Lookup("Roles", "RoleIds", "_id", "Roles")
                    .As<GetUserDetailResponse>()
                    .FirstOrDefaultAsync();

            return user;
        }

        public async Task<GetUserProfileResponse> GetProfileAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "UserName", 1 },
                    { "FullName", 1 },
                    { "MAFCCode", 1 },
                    { "EcDsaCode", 1 },
                    { "EcSaleCode", 1 },
                    { "UserEmail", 1 },
                    { "Phone", 1 },
                    { "IdCard", 1 },
                    { "RoleName", 1 },
                    { "Status", 1 },
                    { "RoleIds", 1 },
                    { "TeamLeadInfo._id", 1},
                    { "TeamLeadInfo.UserName", 1},
                    { "TeamLeadInfo.FullName", 1 },
                    { "IsActive", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                };
            var user = await _collection
                    .Aggregate()
                    .Match(filter)
                    .Project(projectMapping)
                    .As<GetUserProfileResponse>()
                    .FirstOrDefaultAsync();

            return user;
        }

        public async Task<IEnumerable<string>> GetTeamMemberAsTeamleadAsync(string userId)
        {
            try
            {
                var teamMemberIds = await _collection.Find(x => x.TeamLeadInfo.Id == userId).Project(x => x.Id).ToListAsync();
                return teamMemberIds.Concat(new List<string> { userId }).Distinct();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<string>> GetTeamMemberAsAsmAsync(string userId)
        {
            try
            {
                var teamMemberIds = await _collection.Find(x => x.AsmInfo.Id == userId).Project(x => x.Id).ToListAsync();
                return teamMemberIds.Concat(new List<string> { userId }).Distinct();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetTeamMemberAsHeadOfSaleAdminAsync(string userId)
        {
            var userDetail = await base.FindOneAsync(x => x.Id == userId);
            var teamMemberIds = new List<string>();

            if (!string.IsNullOrEmpty(userDetail?.SaleChanelInfo?.Id))
            {
                var userIds = await _collection.Find(x => x.SaleChanelInfo.Id == userDetail.SaleChanelInfo.Id).Project(x => x.Id).ToListAsync();
                teamMemberIds.AddRange(userIds);
            }

            teamMemberIds.Add(userId);
            return teamMemberIds.Distinct();
        }

        public async Task<IEnumerable<string>> GetTeamMemberAsMultiPosAsync(string userId)
        {
            try
            {
                var userDetail = await base.FindOneAsync(x => x.Id == userId);
                var teamMemberIds = new List<string>();

                var posIds = new List<string>();
                if (userDetail.PosInfo != null)
                {
                    posIds.Add(userDetail.PosInfo.Id);
                }
                if (userDetail.IsManageMultiPos == true && userDetail.ManagePosInfos?.Any() == true)
                {
                    posIds.AddRange(userDetail.ManagePosInfos.Select(x => x.Id));
                }

                if (posIds.Any())
                {
                    var userIds = await _collection.Find(x => posIds.Contains(x.PosInfo.Id)).Project(x => x.Id).ToListAsync();
                    teamMemberIds.AddRange(userIds);
                }

                teamMemberIds.Add(userDetail.Id);
                return teamMemberIds.Distinct();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetTeammemberAsPosLead(string userId, string teamLeadRoleId)
        {
            try
            {
                var userDetail = await base.FindOneAsync(x => x.Id == userId);

                var result = new List<User>
                {
                    userDetail
                };

                string posId = userDetail.PosInfo.Id;

                // get all teamlead users in pos
                var teamLeadUsers = (await _collection
                    .FindAsync(x => x.RoleIds != null && x.RoleIds.Contains(teamLeadRoleId) && x.PosInfo.Id == posId))
                    .ToList();

                result.AddRange(teamLeadUsers);

                var teamLeadIs = teamLeadUsers.Select(x => x.Id).ToList();

                if (teamLeadUsers.Any())
                {
                    // get all member of teamleads
                    var teamMembers = (await _collection.FindAsync(x => x.PosInfo.Id == posId && teamLeadIs.Contains(x.TeamLeadInfo.Id))).ToList();
                    result.AddRange(teamMembers);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetTeammemberAsAdmin(string userId)
        {
            try
            {
                try
                {
                    var teamMembers = (await base.FilterByAsync(x => true)).ToList();
                    return teamMembers;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateSaleChanelAsync(string posId, SaleChanelInfo saleChanelInfo, string modifier)
        {
            var update = Builders<User>.Update
                        .Set(x => x.SaleChanelInfo, saleChanelInfo)
                        .Set(x => x.ModifiedDate, DateTime.Now)
                        .Set(x => x.Modifier, modifier);

            await _collection.UpdateManyAsync(x => x.PosInfo.Id == posId, update);
        }
        public async Task UpdateSaleChanelByListPosIdAsync(IEnumerable<string> posIds, SaleChanelInfo saleChanelInfo, string modifier)
        {
            try
            {
                var update = Builders<User>.Update
                            .Set(x => x.SaleChanelInfo, saleChanelInfo)
                            .Set(x => x.ModifiedDate, DateTime.Now)
                            .Set(x => x.Modifier, modifier);

                await _collection.UpdateManyAsync(x => posIds.Contains(x.PosInfo.Id), update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetUserByRoleIds(IEnumerable<string> roleIds, string userId = null, string podId = null, IEnumerable<string> excludeUserIds = null)
        {
            var filter = Builders<User>.Filter.AnyIn(x => x.RoleIds, roleIds);
            if (!string.IsNullOrEmpty(userId))
            {
                filter &= Builders<User>.Filter.Eq(x => x.Id, userId);
            }
            if (excludeUserIds?.Any() == true)
            {
                filter &= Builders<User>.Filter.Nin(x => x.Id, excludeUserIds);
            }
            if (!string.IsNullOrEmpty(podId))
            {
                filter &= Builders<User>.Filter.Eq(x => x.PosInfo.Id, podId);
            }
            var users = await _collection
                        .Aggregate()
                        .Match(filter)
                        .ToListAsync();
            return users;
        }
        public async Task<IEnumerable<GetUserTeamLeadResponse>> GetTeamLeadAsync(GetTeamLeadRequest getTeamLeadRequest)
        {
            var filter = Builders<User>.Filter.Ne(x => x.IsDeleted, true);

            if (!string.IsNullOrEmpty(getTeamLeadRequest.RoleName))
            {
                filter &= Builders<User>.Filter.Eq(x => x.RoleName, getTeamLeadRequest.RoleName);
            }
            if (!string.IsNullOrEmpty(getTeamLeadRequest.PosId))
            {
                filter &= Builders<User>.Filter.Eq(x => x.PosInfo.Id, getTeamLeadRequest.PosId);
            }
            if (!string.IsNullOrEmpty(getTeamLeadRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{getTeamLeadRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<User>.Filter.Regex(x => x.UserName, regex) |
                    Builders<User>.Filter.Regex(x => x.FullName, regex);
            }
            var users = await _collection
                        .Aggregate()
                        .Match(filter)
                        .As<GetUserTeamLeadResponse>()
                        .ToListAsync();
            return users;
        }
    }
}

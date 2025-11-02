using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.Roles;
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
    public interface IRoleRepository: IMongoRepository<Role>
    {
        Task<IEnumerable<GetRoleResponse>> GetAsync(string textSearch, int pageIndex, int pageSize);
        Task<long> CountAsync(string textSearch);
        Task<GetRoleDetailResponse> GetDetailAsync(string id);

        Task<IEnumerable<Role>> GetDetailByIdsAsync(IEnumerable<string> ids);

        Task<Role> GetDetailByNameAsync(string name);
    }
    public class RoleRepository: MongoRepository<Role>, IRoleRepository, IScopedLifetime
    {
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(ILogger<RoleRepository> logger, IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<GetRoleResponse>> GetAsync(string textSearch, int pageIndex, int pageSize)
        {
            var filter = GetFilter(textSearch);
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "RoleName", 1 },
                    { "RoleDescription", 1 },
                    { "Permissions._id", 1},
                    { "Permissions.Name", 1},
                    { "Permissions.Value", 1 }
                };
            var roles = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(x => x.CreatedDate)
                    .Lookup("Permission", "PermissionIds", "_id", "Permissions")
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .Project(projectMapping)
                    .As<GetRoleResponse>()
                    .ToListAsync();

            return roles;
        }

        public async Task<long> CountAsync(string textSearch)
        {
            var filter = GetFilter(textSearch);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<Role> GetFilter(string textSearch)
        {
            var filter = Builders<Role>.Filter.Ne(u => u.IsDeleted, true);
            if (!string.IsNullOrEmpty(textSearch))
            {
                var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<Role>.Filter.Regex(user => user.RoleName, regex);
            }
            return filter;
        }

        public async Task<GetRoleDetailResponse> GetDetailAsync(string id)
        {
            var filter = Builders<Role>.Filter.Eq(u => u.Id, id);
            filter &= Builders<Role>.Filter.Ne(u => u.IsDeleted, true);

            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

            var projectMapping = new BsonDocument()
                 {
                    { "_id", 1 },
                    { "RoleName", 1 },
                    { "RoleDescription", 1 },
                    { "Permissions._id", 1},
                    { "Permissions.Name", 1},
                    { "Permissions.Value", 1 },
                    { "Permissions.Group", 1 }
                };

            GetRoleDetailResponse role = await _collection
                    .Aggregate()
                    .Match(filter)
                    .Lookup("Permission", "PermissionIds", "_id", "Permissions")
                    .Project(projectMapping)
                    .As<GetRoleDetailResponse>()
                    .FirstOrDefaultAsync();

            return role;
        }

        public async Task<IEnumerable<Role>> GetDetailByIdsAsync(IEnumerable<string> ids)
        {
            try
            {
                var roleDetails = await _collection.FindAsync(x => ids.Contains(x.Id));

                return roleDetails.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Role> GetDetailByNameAsync(string name)
        {
            try
            {
                var roleDetails = await _collection.FindAsync(x => x.RoleName == name);

                return roleDetails.SingleOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

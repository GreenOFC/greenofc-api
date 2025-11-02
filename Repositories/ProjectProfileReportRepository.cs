using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.ProjectProfileReports;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IProjectProfileReportRepository : IMongoRepository<ProjectProfileReport>
    {
        Task<IEnumerable<ProjectProfileReportResponse>> GetAsync(string textSearch, int pageIndex = 1, int pageSize = 10);

        Task<long> CountAsync(string textSearch);

        Task<ProjectProfileReportDetailResponse> GetDetailAsync(string id);
    }

    public class ProjectProfileReportRepository : MongoRepository<ProjectProfileReport>, IProjectProfileReportRepository, IScopedLifetime
    {
        public ProjectProfileReportRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<ProjectProfileReportResponse>> GetAsync(string textSearch, int pageIndex = 1, int pageSize = 10)
        {
            var filter = GetFilter(textSearch);
            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .As<ProjectProfileReportResponse>()
                    .ToListAsync();

            return result;
        }

        public async Task<long> CountAsync(string textSearch)
        {
            var filter = GetFilter(textSearch);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        public async Task<ProjectProfileReportDetailResponse> GetDetailAsync(string id)
        {
            var filter = Builders<ProjectProfileReport>.Filter.Ne(x => x.IsDeleted, true) & Builders<ProjectProfileReport>.Filter.Eq(x => x.Id, id);
            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .As<ProjectProfileReportDetailResponse>()
                    .FirstOrDefaultAsync();

            return result;
        }

        private FilterDefinition<ProjectProfileReport> GetFilter(string textSearch)
        {
            var filter = Builders<ProjectProfileReport>.Filter.Ne(x => x.IsDeleted, true);

            if (!string.IsNullOrEmpty(textSearch))
            {
                var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<ProjectProfileReport>.Filter.Regex(x => x.SaleInfomation.UserName, regex) |
                    Builders<ProjectProfileReport>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<ProjectProfileReport>.Filter.Regex(x => x.FileName, regex);
            }
            return filter;
        }
    }
}

using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.CheckSims;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.MC
{
    public interface ICheckSimRepository : IMongoRepository<CheckSim>
    {
        Task<IEnumerable<GetCheckSimResponse>> GetAsync(IEnumerable<string> creatorIds, GetCheckSimRequest getCheckSimRequest);
        Task<long> CountAsync(IEnumerable<string> creatorIds, GetCheckSimRequest getCheckSimRequest);
        Task<CheckSim> FindLastAsync(string idCard, string phoneNumber, string creator, CheckSimAction action, CheckSimProject project);
    }

    public class CheckSimRepository : MongoRepository<CheckSim>, ICheckSimRepository, IScopedLifetime
    {
        public CheckSimRepository(
            IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetCheckSimResponse>> GetAsync(IEnumerable<string> creatorIds,GetCheckSimRequest getCheckSimRequest)
        {
            var filter = GetFilter(creatorIds, getCheckSimRequest);

            return await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((getCheckSimRequest.PageIndex - 1) * getCheckSimRequest.PageSize)
                .Limit(getCheckSimRequest.PageSize)
                .As<GetCheckSimResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(IEnumerable<string> creatorIds,GetCheckSimRequest getCheckSimRequest)
        {
            var filter = GetFilter(creatorIds, getCheckSimRequest);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        public async Task<CheckSim> FindLastAsync(string idCard, string phoneNumber, string creator, CheckSimAction action, CheckSimProject project)
        {
            return await _collection
                .Find(x => x.IdCard == idCard && x.PhoneNumber == phoneNumber && x.Creator == creator && x.Action == action && x.Project == project)
                .SortByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        private FilterDefinition<CheckSim> GetFilter(IEnumerable<string> creatorIds,GetCheckSimRequest request)
        {
            var filter = Builders<CheckSim>.Filter.Ne(x => x.IsDeleted, true);

            filter &= Builders<CheckSim>.Filter.Eq(x => x.Project, request.Project);
            if (creatorIds.Any())
            {
                filter &= Builders<CheckSim>.Filter.In(x => x.Creator, creatorIds);
            }
            if (!string.IsNullOrEmpty(request.Customer))
            {
                var regex = new BsonRegularExpression($"/{request.Customer.ConvertSpecialCharacters()}/i");
                filter &= Builders<CheckSim>.Filter.Regex(x => x.PhoneNumber, regex) | Builders<CheckSim>.Filter.Regex(x => x.IdCard, regex);
            }

            if (!string.IsNullOrEmpty(request.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{request.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<CheckSim>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex) | Builders<CheckSim>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex);
            }

            if (!string.IsNullOrEmpty(request.Pos))
            {
                var regex = new BsonRegularExpression($"/{request.Pos.ConvertSpecialCharacters()}/i");
                filter &= Builders<CheckSim>.Filter.Regex(x => x.PosInfo.Name, regex);
            }

            if (request.Status.HasValue)
            {
                filter &= Builders<CheckSim>.Filter.Eq(x => x.Transaction.Status, request.Status);
            }

            if (!string.IsNullOrEmpty(request.Sale))
            {
                var regex = new BsonRegularExpression($"/{request.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<CheckSim>.Filter.Regex(x => x.SaleInfo.FullName, regex) |
                    Builders<CheckSim>.Filter.Regex(x => x.SaleInfo.Code, regex);
            }

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{request.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<CheckSim>.Filter.Regex(x => x.Transaction.PartnerTransaction, regex);
            }

            filter &= Builders<CheckSim>.Filter.Gte(x => x.ModifiedDate, request.GetFromDate());
            filter &= Builders<CheckSim>.Filter.Lte(x => x.ModifiedDate, request.GetToDate());

            return filter;
        }
    }
}

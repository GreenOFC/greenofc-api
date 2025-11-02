using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using System.Linq;
using System;

namespace _24hplusdotnetcore.Repositories.Ticket
{
    public interface ITicketRepository : IMongoRepository<TicketModel>
    {
        Task<IEnumerable<GetTicketResponse>> GetAsync(IEnumerable<string> userIds, GetTicketRequest request);
        Task<GetTicketResponse> GetDetailAsync(string id);
        Task<long> CountAsync(IEnumerable<string> userIds, GetTicketRequest request);
        Task<IEnumerable<GetReportTicketResponse>> GetAsync(IEnumerable<string> userIds, GetReportTicketRequest request);
        Task<long> CountAsync(IEnumerable<string> userIds, GetReportTicketRequest request);
    }
    public class TicketRepository : MongoRepository<TicketModel>, ITicketRepository, IScopedLifetime
    {
        private readonly ICommentTicketRepository _commentTicketRepository;
        public TicketRepository(
            IMongoDbConnection mongoDbConnection,
            ICommentTicketRepository commentTicketRepository) : base(mongoDbConnection)
        {
            _commentTicketRepository = commentTicketRepository;
        }

        public async Task<IEnumerable<GetTicketResponse>> GetAsync(IEnumerable<string> userIds, GetTicketRequest request)
        {
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var filter = GetFilter(userIds, request.TextSearch, request.Project, request.Status);
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "Title", 1 },
                    { "Code", 1 },
                    { "Project", 1 },
                    { "Category", 1 },
                    { "Type", 1 },
                    { "Description", 1 },
                    { "Status", 1 },
                    { "Medias", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "ListReader", 1 },
                    { "Sale", 1 },
                    { "AssignerId", 1 },
                };

            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Limit(request.PageSize)
                    .Project(projectMapping)
                    .As<GetTicketResponse>()
                    .ToListAsync();

            if (result.Any())
            {
                var comments = await _commentTicketRepository.GetAsync(result.Select(x => x.Id));
                foreach (var item in result)
                {
                    item.Comments = comments.Where(x => x.TicketId == item.Id);
                }
            }


            return result;
        }

        public async Task<GetTicketResponse> GetDetailAsync(string id)
        {
            var filter = Builders<TicketModel>.Filter.Eq(u => u.Id, id);
            filter &= Builders<TicketModel>.Filter.Ne(u => u.IsDeleted, true);

            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "Title", 1 },
                    { "Code", 1 },
                    { "Project", 1 },
                    { "Category", 1 },
                    { "Type", 1 },
                    { "Description", 1 },
                    { "Status", 1 },
                    { "Medias", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "ListReader", 1 },
                    { "Sale", 1 },
                    { "AssignerId", 1 },
                };

            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .Lookup("Users", "Creator", "_id", "Sale")
                    .Unwind("Sale", unwindOption)
                    .Project(projectMapping)
                    .As<GetTicketResponse>()
                    .FirstOrDefaultAsync();

            if (result != null)
            {
                result.Comments = await _commentTicketRepository.GetAsync(new string[] { result.Id });
            }

            return result;
        }
        public async Task<long> CountAsync(IEnumerable<string> userIds, GetTicketRequest request)
        {
            var filter = GetFilter(userIds, request.TextSearch, request.Project, request.Status);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        public async Task<IEnumerable<GetReportTicketResponse>> GetAsync(IEnumerable<string> userIds, GetReportTicketRequest request)
        {
            var filter = GetFilter(userIds, request.TextSearch, request.Project, request.Status, request.GetFromDate(), request.GetToDate());
            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Limit(request.PageSize)
                    .As<GetReportTicketResponse>()
                    .ToListAsync();
            return result;
        }

        public async Task<long> CountAsync(IEnumerable<string> userIds, GetReportTicketRequest request)
        {
            var filter = GetFilter(userIds, request.TextSearch, request.Project, request.Status, request.GetFromDate(), request.GetToDate());
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<TicketModel> GetFilter(IEnumerable<string> userIds, string textSearch, string project, string status, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var filter = Builders<TicketModel>.Filter.Empty;
            if (!string.IsNullOrEmpty(textSearch))
            {
                filter &= Builders<TicketModel>.Filter.Regex(x => x.Title, new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i"));
            }
            if (!string.IsNullOrEmpty(project))
            {
                filter &= Builders<TicketModel>.Filter.Eq(x => x.Project.Key, project);
            }
            if (!string.IsNullOrEmpty(status))
            {
                filter &= Builders<TicketModel>.Filter.Eq(x => x.Status, status);
            }
            if (userIds?.Any() == true)
            {
                filter &= Builders<TicketModel>.Filter.Or(
                    Builders<TicketModel>.Filter.In(x => x.Creator, userIds),
                    Builders<TicketModel>.Filter.And(
                        Builders<TicketModel>.Filter.In(x => x.AssignerId, userIds),
                        Builders<TicketModel>.Filter.Ne(x => x.Status, TicketStatus.DRAFT)
                        )
                    );
            }
            if (fromDate.HasValue)
            {
                filter &= Builders<TicketModel>.Filter.Gte(x => x.CreatedDate, fromDate);
            }

            if (toDate.HasValue)
            {
                filter &= Builders<TicketModel>.Filter.Lte(x => x.CreatedDate, toDate);
            }
            return filter;
        }
    }
}

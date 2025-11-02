using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.Ticket
{
    public interface ICommentTicketRepository : IMongoRepository<CommentTicketModel>
    {
        Task<IEnumerable<GetTicketCommentResponse>> GetAsync(IEnumerable<string> ticketIds);
    }
    public class CommentTicketRepository : MongoRepository<CommentTicketModel>, ICommentTicketRepository, IScopedLifetime
    {
        public CommentTicketRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {

        }

        public async Task<IEnumerable<GetTicketCommentResponse>> GetAsync(IEnumerable<string> ticketIds)
        {
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var filter = Builders<CommentTicketModel>.Filter.In(u => u.TicketId, ticketIds);
            filter &= Builders<CommentTicketModel>.Filter.Ne(u => u.IsDeleted, true);
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "TicketId", 1 },
                    { "Comment", 1 },
                    { "Medias", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "Sale", 1},
                };
            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Lookup("Users", "Creator", "_id", "Sale")
                    .Unwind("Sale", unwindOption)
                    .Project(projectMapping)
                    .As<GetTicketCommentResponse>()
                    .ToListAsync();

            return result;
        }
    }
}

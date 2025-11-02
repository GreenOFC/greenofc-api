using System.Collections.Generic;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.Ticket
{
    [BsonCollection(MongoCollection.CommentTicket)]
    public class CommentTicketModel : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string TicketId { get; set; }
        public string Comment { get; set; }
        public IEnumerable<UploadedMedia> Medias { get; set; }
    }
}
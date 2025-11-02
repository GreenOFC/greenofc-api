using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Ticket
{
    public interface ICommentTicketModelDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        string TicketId { get; set; }
        string Comment { get; set; }
        IEnumerable<UploadedMediaDto> Medias { get; set; }
    }
    public abstract class CommentTicketModelDto : ICommentTicketModelDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string TicketId { get; set; }
        public string Comment { get; set; }
        public IEnumerable<UploadedMediaDto> Medias { get; set; }
    }
}
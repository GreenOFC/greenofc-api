using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Ticket
{
    public class CreateTicketModelDto 
    {
        public string Title { get; set; }
        public DataConfigDto Project { get; set; }
        public DataConfigDto Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssignerId { get; set; }
        public IEnumerable<UploadedMediaDto> Medias { get; set; }
    }
    public class UpdateTicketModelDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DataConfigDto Project { get; set; }
        public DataConfigDto Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssignerId { get; set; }
        public IEnumerable<UploadedMediaDto> Medias { get; set; }
    }
}
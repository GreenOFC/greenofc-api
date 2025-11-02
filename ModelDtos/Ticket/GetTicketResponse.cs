using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
namespace _24hplusdotnetcore.ModelDtos.Ticket
{
    [BsonIgnoreExtraElements]
    public class GetTicketResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public IEnumerable<GetTicketCommentResponse> Comments { get; set; }
        public IEnumerable<SaleInfomationDto> SaleInfo => new List<SaleInfomationDto> { Sale };
        public SaleInfomationDto Sale { get; set; }
        public SaleInfomationDto Handler => Comments?.OrderBy(x => x.CreatedDate)?.FirstOrDefault(x => x.Sale?.Id != Sale?.Id)?.Sale;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public DataConfigDto Project { get; set; }
        public DataConfigDto Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssignerId { get; set; }
        public IEnumerable<string> ListReader { get; set; }
        public bool IsRead { get; set; }
        public IEnumerable<UploadedMediaDto> Medias { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class GetTicketCommentResponse : CommentTicketModelDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public IEnumerable<SaleInfomationDto> SaleInfo => new List<SaleInfomationDto> { Sale };
        public SaleInfomationDto Sale { get; set; }
    }
}
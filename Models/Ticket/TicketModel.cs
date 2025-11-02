using System;
using System.Collections.Generic;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.Ticket
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.Ticket)]
    public class TicketModel : BaseEntity
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public DataConfigModel Project { get; set; }
        public DataConfigModel Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssignerId { get; set; }
        public IEnumerable<string> ListReader { get; set; }
        public bool IsRead { get; set; } = false;
        public IEnumerable<UploadedMedia> Medias { get; set; }

        public SaleInfomation Sale { get; set; }
        public SaleInfomation SaleModified { get; set; }
        public IEnumerable<TicketHistory> Histories { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class TicketHistory
    {
        public string Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public PersonInfo PersonInfo { get; set; }
    }
}
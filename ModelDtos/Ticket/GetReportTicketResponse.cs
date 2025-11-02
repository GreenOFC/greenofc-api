using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Ticket
{
    [BsonIgnoreExtraElements]
    public class GetReportTicketResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public DataConfigDto Project { get; set; }

        public string Status { get; set; }

        public SaleInfomationDto Sale { get; set; }

        public SaleInfomationDto SaleModified { get; set; }

        public IEnumerable<TicketHistoryDto> Histories { get; set; }
    }
}

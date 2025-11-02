using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.Ticket
{
    [BsonIgnoreExtraElements]
    public class TicketHistoryDto
    {
        public string Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDate { get; set; }

        public PersonInfoDto PersonInfo { get; set; }
    }
}

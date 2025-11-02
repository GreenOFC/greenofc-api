using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.History
{
    [BsonIgnoreExtraElements]
    public class GetHistoryV2Response
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public object ValueBefore { get; set; }
        public object ValueAfter { get; set; }
        public IEnumerable<SaleInfoResponse> SaleInfo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

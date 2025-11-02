using System;
using System.Collections.Generic;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.History
{
    public class GetHistoryResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public CustomerDto ValueBefore { get; set; }
        public CustomerDto ValueAfter { get; set; }
        public object OldValue { get; private set; }

        public object NewValue { get; private set; }
        public IEnumerable<SaleInfoResponse> SaleInfo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

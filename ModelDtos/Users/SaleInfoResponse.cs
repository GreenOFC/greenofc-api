
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class SaleInfoResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}

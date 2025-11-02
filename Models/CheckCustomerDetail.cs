using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.CheckCustomerDetail)]
    public class CheckCustomerDetail : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string FileId { get; set; }

        public string IdCard { get; set; }

        public IEnumerable<CheckCustomerResult> Results { get; set; }
    }

    public class CheckCustomerResult
    {
        public string GreenType { get; set; }

        public dynamic Result { get; set; }
    }
}

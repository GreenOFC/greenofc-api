using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.CheckCustomers
{
    [BsonIgnoreExtraElements]
    public class CheckCustomerDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string IdCard { get; set; }

        public IEnumerable<CheckCustomerResultDto> Results { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CheckCustomerResultDto
    {
        public string GreenType { get; set; }

        public dynamic Result { get; set; }
    }


}

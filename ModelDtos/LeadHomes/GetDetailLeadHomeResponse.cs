using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.LeadHomes
{
    [BsonIgnoreExtraElements]
    public class GetDetailLeadHomeResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string IdCard { get; set; }

        public string Phone { get; set; }

        public LeadHomeAddressDto TemporaryAddress { get; set; }

        public SaleInfoResponse SaleInfo { get; set; }

        public DateTime CreatedDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}

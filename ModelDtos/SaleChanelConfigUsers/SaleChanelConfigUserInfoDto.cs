using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers
{
    [BsonIgnoreExtraElements]
    public class SaleChanelConfigUserInfoDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UID { get; set; }

        public string ClientID { get; set; }

        public string SecretKey { get; set; }
    }
}

using _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.SaleChanels
{
    [BsonIgnoreExtraElements]
    public class SaleChanelDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public SaleChanelConfigUserInfoDto SaleChanelConfigUserInfo { get; set; }
    }
}

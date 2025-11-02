using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    [BsonIgnoreExtraElements]
    public class SaleInfomationDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
    }
}

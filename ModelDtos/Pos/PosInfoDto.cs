using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Pos
{
    public class PosInfoDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public PosManagerDto Manager { get; set; }
    }
}

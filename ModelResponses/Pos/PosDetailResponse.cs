using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelResponses.Pos
{
    [BsonIgnoreExtraElements]
    public class PosDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public PosManagerDto Manager { get; set; }
        public SaleChanelDto SaleChanelInfo { get; set; }
        public IEnumerable<PosManagerDto> ConcurrentManagers { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Pos
{
    public class UpdateUserPosDto
    {
        public string UserId { get; set; }
        public string PosId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> RoleIds { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class TeamLeadInfoDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public IEnumerable<TeamMemberDto> TeamMembers { get; set; }
    }
}

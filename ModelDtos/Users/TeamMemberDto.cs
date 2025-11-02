using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class TeamMemberDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string MAFCCode { get; set; }
        public string FullName { get; set; }
        public string IdCard { get; set; }
    }
}

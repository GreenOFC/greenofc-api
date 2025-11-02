using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.ModelResponses.Role;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.UserSuspendeds
{
    [BsonIgnoreExtraElements]
    public class GetUserInUserSuspendedResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserEmail { get; set; }

        public IEnumerable<RoleDetailResponse> Roles { get; set; }

        public TeamLeadDto TeamLeadInfo { get; set; }
    }
}

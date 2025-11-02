using _24hplusdotnetcore.ModelDtos.Permissions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Roles
{
    public class GetRoleResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public IEnumerable<PermissionDto> Permissions { get; set; }
    }
}

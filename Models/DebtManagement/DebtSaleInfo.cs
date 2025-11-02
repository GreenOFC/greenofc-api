using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.DebtManagement
{
    public class DebtSaleInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
        public DebtPosInfo Pos { get; set; }
        public DebtTeamleadInfo TeamLead { get; set; }
    }
}

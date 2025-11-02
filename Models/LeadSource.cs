using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Models.VPS;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.LeadSource)]
    [BsonKnownTypes(typeof(LeadVps), typeof(LeadVib), typeof(LeadF88), typeof(LeadHome), typeof(LeadOkVay), typeof(LeadVbi))]
    public abstract class LeadSource : BaseEntity
    {
        public string CRMId { get; set; }

        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }

        public SaleInfomation SaleInfomation { get; set; }
        public SaleChanelInfo SaleChanelInfo { get; set; }
    }
}

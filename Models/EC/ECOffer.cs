using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models.EC
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.ECOffer)]
    public class ECOfferData : BaseDocument
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string RequestId { get; set; }
    
        public string PartnerCode { get; set; }

        public string ProposalId { get; set; }

        public string ContractNumber { get; set; }

        public string RejectReason { get; set; }

        public IEnumerable<ECOfferList> OfferList { get; set; }
    }
}

using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models.EC
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.ECNotification)]
    public class ECNotification : BaseDocument
    {
        public string LoanRequestId { get; set; }
        public string ProposalId { get; set; }
        public string ContractNumber { get; set; }
        public string ApplicationStatus { get; set; }
        public string ApplicationSubCode { get; set; }
        public string DescriptionCode { get; set; }
        public IEnumerable<ECOfferList> OfferList { get; set; }
    }
}

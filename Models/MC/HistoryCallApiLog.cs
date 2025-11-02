using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models.MC
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.HistoryCallApiLog)]
    public class HistoryCallApiLog : BaseEntity
    {
        public string Action { get; set; }
        public string GreenType { get; set; }
        public string RemoteIpAddress { get; set; }

        public string AbsolutePath { get; set; }

        public string Payload { get; set; }

        public string Response { get; set; }

        public string Message { get; set; }
        
        public SaleInfomation SaleInfo { get; set; }
        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }

        public SaleChanelInfo SaleChanelInfo { get; set; }
    }
}

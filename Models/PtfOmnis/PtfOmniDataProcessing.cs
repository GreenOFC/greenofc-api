using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Models.MAFC;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models.PtfOmnis
{
    [BsonCollection(MongoCollection.PtfOmniDataProcessing)]
    [BsonIgnoreExtraElements]
    public class PtfOmniDataProcessing: BaseDocument
    {
        public string CustomerId { get; set; }

        [JsonConverter(typeof(AuditActionType))]
        [BsonRepresentation(BsonType.String)]
        public PtfOmniDataProcessingStatus Status { get; set; } = PtfOmniDataProcessingStatus.Draft;

        public string Step { get; set; } = DataProcessingStep.PTF_CREATE_LOAN;

        public IEnumerable<PayloadModel> Payloads { get; set; }
    }
}

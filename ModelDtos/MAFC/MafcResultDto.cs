using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    [BsonIgnoreExtraElements]
    public class MafcResultDto
    {
        public string ReturnStatus { get; set; }
        public string Reason { get; set; }
        public string MAFCEcontract { get; set; }
        public bool FinishedRound1 { get; set; }
    }
}

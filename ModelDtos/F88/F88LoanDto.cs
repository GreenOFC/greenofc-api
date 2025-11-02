using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.F88
{
    [BsonIgnoreExtraElements]
    public class F88LoanDto
    {
        public string Amount { get; set; }
    }
}

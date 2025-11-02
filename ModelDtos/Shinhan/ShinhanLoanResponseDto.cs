using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    [BsonIgnoreExtraElements]
    public class ShinhanLoanResponseDto
    {
        public string Product { get; set; }
    }
}

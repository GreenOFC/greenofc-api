using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBIncomeDto
    {
        [JsonProperty("declaredMonthlyAmount")]
        public decimal? DeclaredMonthlyAmount { get; set; }
    }
}

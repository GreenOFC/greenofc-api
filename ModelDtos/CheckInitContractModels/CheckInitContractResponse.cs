using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.CheckInitContractModels
{
    public class CheckInitContractResponse
    {
        public int ReturnCode { get; set; }
        public string ReturnMes { get; set; }
        public IEnumerable<CheckInitContractResultResponse> Results { get; set; }
    }

}

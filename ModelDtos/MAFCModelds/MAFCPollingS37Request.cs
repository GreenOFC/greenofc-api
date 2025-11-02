using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCPollingS37Request
    {
        [Required]
        public string IdValue { get; set; }
    }
}

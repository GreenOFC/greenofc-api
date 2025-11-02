using System.ComponentModel.DataAnnotations;
using Refit;

namespace _24hplusdotnetcore.ModelDtos.AT
{
    public class ATResponseDto
    {
        [AliasAs("status_code")]
        public string StatusCode { get; set; }
        [AliasAs("message")]
        public string Message { get; set; }
    }
}

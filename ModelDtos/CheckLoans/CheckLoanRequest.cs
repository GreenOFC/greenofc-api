using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.CheckLoans
{
    public class CheckLoanRequest
    {
        [Required]
        public string IdCard { get; set; }
    }
}

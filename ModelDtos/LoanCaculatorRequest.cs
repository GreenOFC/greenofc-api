using System;
using System.ComponentModel.DataAnnotations;
using Refit;

namespace _24hplusdotnetcore.ModelDtos
{
    public class LoanCaculatorRequest
    {
        [Required]
        [AliasAs("productId")]
        public string ProductId { get; set; }

        [Required]
        [AliasAs("term")]
        [Range(0, int.MaxValue)]
        public int Nper { get; set; } // The total number of payment periods

        [Required]
        [AliasAs("loanAmount")]
        [Range(0, double.MaxValue)]
        public double Pv { get; set; } // The present value (or lump sum)

        [Required]
        [AliasAs("income")]
        [Range(0, double.MaxValue)]
        public double Income { get; set; }
    }
}

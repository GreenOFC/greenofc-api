using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadOkVays
{
    public class UpdateLeadOkVayRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string IdCard { get; set; }

        [Required]
        public string Phone { get; set; }

        public string ExtraPhone { get; set; }

        [Required]
        public LeadOkVayAddressDto TemporaryAddress { get; set; }

        public string Debt { get; set; }
        public string DebtId { get; set; }
        public string IncomeId { get; set; }

        public string Income { get; set; }
    }
}

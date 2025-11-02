using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.DebtManagement
{
    public class GetDebtDto : PagingRequest
    {
        public string ContractCode { get; set; }
        public string UpdatedDateTime { get; set; }
    }
}
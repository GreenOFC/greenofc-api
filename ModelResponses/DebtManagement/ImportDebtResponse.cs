using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelResponses.DebtManagement
{
    public class ImportDebtResponse
    {
        public List<ImportDebtDetailResponse> Valid { get; set; }
        public List<ImportDebtDetailResponse> Invalid { get; set; }
    }
}
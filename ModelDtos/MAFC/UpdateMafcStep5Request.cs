using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class UpdateMafcStep5Request
    {
        public MafcRefereeDto Spouse { get; set; }
        public IEnumerable<MafcRefereeDto> Referees { get; set; }
        public MafcBankInfoDto BankInfo { get; set; }
        public MafcOtherInfoDto OtherInfo { get; set; }
    }
}

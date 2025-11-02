using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class UpdateLeadPtfRequest: IUpdateLeadPtf, IUpdateLeadPtfPersonal
    {
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        public LeadPtfPersonalDto Personal { get; set; }
        public LeadPtfWorkingDto Working { get; set; }
        public string FamilyBookNo { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public LeadPtfAddressDto TemporaryAddress { get; set; }
        public LeadPtfShortAddressDto ResidentAddress { get; set; }
        public IEnumerable<LeadPtfRefereeDto> Referees { get; set; }
        public LeadPtfLoanDto Loan { get; set; }
        public LeadPtfDisbursementInformationDto DisbursementInformation { get; set; }
    }
}

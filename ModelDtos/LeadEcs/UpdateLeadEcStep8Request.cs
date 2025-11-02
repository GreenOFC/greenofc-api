using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcStep8Request
    {
        public LeadEcSelectedOfferDto LeadEcSelectedOffer { get; set; }
    }

    public class LeadEcSelectedOfferDto
    {
        public string SelectedOfferId { get; set; }

        public string SelectedOfferAmount { get; set; }

        public string SelectedOfferInsuranceType { get; set; }
    }
}

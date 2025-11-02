using System;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadVibs
{
    public class CreateLeadVibRequest : LeadVibDto
    {
    }
    public class UpdateLeadVibRequest : LeadVibDto
    {
        public string Id { get; set; }
    }
}

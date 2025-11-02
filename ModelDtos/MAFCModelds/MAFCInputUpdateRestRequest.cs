using System.Collections.Generic;
using _24hplusdotnetcore.Common.Constants;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCInputUpdateRestRequest
    {
        [JsonProperty("in_channel")]
        public string In_channel { get; set; } = MAFCDataEntry.Channel;
        [JsonProperty("in_appid")]
        public int In_appid { get; set; }
        [JsonProperty("in_userid")]
        public string In_userid { get; set; } = MAFCDataEntry.UserId;
        [JsonProperty("in_schemeid")]
        public int? In_schemeid { get; set; }
        [JsonProperty("in_totalloanamountreq")]
        public int? In_totalloanamountreq { get; set; }
        [JsonProperty("in_tenure")]
        public int? In_tenure { get; set; }
        [JsonProperty("in_laa_app_ins_applicable")]
        public string In_laa_app_ins_applicable { get; set; }

        [JsonProperty("in_loanpurpose")]
        public string In_loanpurpose { get; set; }
        [JsonProperty("in_priority_c")]
        public string In_priority_c { get; set; }
        [JsonProperty("in_title")]
        public string In_title { get; set; }
        [JsonProperty("in_fname")]
        public string In_fname { get; set; }
        [JsonProperty("in_mname")]
        public string In_mname { get; set; }
        [JsonProperty("in_lname")]
        public string In_lname { get; set; }
        [JsonProperty("in_gender")]
        public string In_gender { get; set; }
        [JsonProperty("in_nationalid")]
        public string In_nationalid { get; set; }
        [JsonProperty("in_dob")]
        public string In_dob { get; set; }
        [JsonProperty("in_tax_code")]
        public string In_tax_code { get; set; }
        [JsonProperty("in_presentjobyear")]
        public int? In_presentjobyear { get; set; }
        [JsonProperty("in_presentjobmth")]
        public int? In_presentjobmth { get; set; }
        [JsonProperty("in_others")]
        public string In_others { get; set; }
        [JsonProperty("in_position")]
        public string In_position { get; set; }
        [JsonProperty("in_amount")]
        public string In_amount { get; set; }
        [JsonProperty("in_accountbank")]
        public string In_accountbank { get; set; }
        [JsonProperty("in_maritalstatus")]
        public string In_maritalstatus { get; set; }
        [JsonProperty("in_eduqualify")]
        public string In_eduqualify { get; set; }
        [JsonProperty("in_noofdependentin")]
        public string In_noofdependentin { get; set; }
        [JsonProperty("in_paymentchannel")]
        public string In_paymentchannel { get; set; }
        [JsonProperty("in_nationalidissuedate")]
        public string In_nationalidissuedate { get; set; }
        [JsonProperty("in_familybooknumber")]
        public string In_familybooknumber { get; set; }
        [JsonProperty("in_idissuer")]
        public string In_idissuer { get; set; }
        [JsonProperty("in_spousename")]
        public string In_spousename { get; set; }
        [JsonProperty("in_spouse_id_c")]
        public string In_spouse_id_c { get; set; }
        [JsonProperty("in_bankname")]
        public string In_bankname { get; set; }
        [JsonProperty("In_bankbranch")]
        public string In_bankbranch { get; set; }
        [JsonProperty("in_accno")]
        public string In_accno { get; set; }

        [JsonProperty("address")]
        public IEnumerable<MAFCInputQDEAddressDto> Address { get; set; }
        [JsonProperty("reference")]
        public IEnumerable<MAFCInputQDEReferenceDto> Reference { get; set; }

    }
}

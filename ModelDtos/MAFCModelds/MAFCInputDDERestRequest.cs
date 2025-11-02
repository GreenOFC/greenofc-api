using System.Collections.Generic;
using _24hplusdotnetcore.Common.Constants;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCInputDDERestRequest
    {
        public string In_channel { get; set; } = MAFCDataEntry.Channel;
        public int In_appid { get; set; }
        public string In_userid { get; set; } = MAFCDataEntry.UserId;
        public string In_maritalstatus { get; set; }
        public string In_qualifyingyear { get; set; } = "0";
        public string In_eduqualify { get; set; }
        public string In_noofdependentin { get; set; }
        public string In_paymentchannel { get; set; }
        public string In_nationalidissuedate { get; set; }
        public string In_familybooknumber { get; set; }
        public string In_idissuer { get; set; }
        public string In_spousename { get; set; }
        public string In_spouse_id_c { get; set; }
        public string In_categoryid { get; set; } = "DAN";
        public string In_bankname { get; set; }
        public string In_bankbranch { get; set; }
        public string In_acctype { get; set; } = "CURRENT";
        public string In_accno { get; set; }
        public string In_dueday { get; set; }
        public string In_notecode { get; set; } = "DE_MOBILE";
        public string In_notedetails { get; set; }
        [JsonProperty("note")]
        public IEnumerable<InputDDENote> Note { get; set; }
        public string MsgName { get; set; } = MAFCDataEntry.InputDDE;
    }
    public class InputDDENote
    {
        [JsonProperty("in_notecode")]
        public string In_notecode { get; set; } = "CMND_07";
        [JsonProperty("in_notedetails")]
        public string In_notedetails { get; set; }
    }
}

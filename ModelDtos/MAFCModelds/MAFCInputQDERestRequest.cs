using System.Collections.Generic;
using _24hplusdotnetcore.Common.Constants;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCInputQDERestRequest
    {
        public string In_channel { get; set; } = MAFCDataEntry.Channel;
        public int In_schemeid { get; set; }
        public int In_downpayment { get; set; } = 0;
        public int In_totalloanamountreq { get; set; }
        public int In_tenure { get; set; }
        public string In_sourcechannel { get; set; } = "ADVT";
        public string In_salesofficer { get; set; } = "14750";
        public string In_loanpurpose { get; set; }
        public string In_creditofficercode { get; set; } = MAFCDataEntry.UserId;
        public string In_bankbranchcode { get; set; } = "01";
        public string In_laa_app_ins_applicable { get; set; }
        public string In_possipbranch { get; set; } = "14";
        public string In_priority_c { get; set; }
        public string In_userid { get; set; } = MAFCDataEntry.UserId;
        public string In_fname { get; set; }
        public string In_mname { get; set; }
        public string In_lname { get; set; }
        public string In_nationalid { get; set; }
        public string In_title { get; set; }
        public string In_gender { get; set; }
        public string In_dob { get; set; }
        public int In_constid { get; set; }
        public IEnumerable<MAFCInputQDEAddressDto> Address { get; set; }
        public string In_tax_code { get; set; }
        public int In_presentjobyear { get; set; }
        public int In_presentjobmth { get; set; }
        public int In_previousjobyear { get; set; } = 0;
        public int In_previousjobmth { get; set; } = 0;
        public string In_natureofbuss { get; set; } = "hoat dong lam thue cac cong viec trong cac hgd,sx sp vat chat va dich vu tu tieu dung cua ho gia dinh";
        public string In_referalgroup { get; set; } = "3";
        public string In_addresstype { get; set; }
        public string In_addressline { get; set; }
        public int In_country { get; set; } = 189;
        public string In_city { get; set; }
        public string In_district { get; set; }
        public string In_ward { get; set; }
        public string In_phone { get; set; }
        public string In_others { get; set; }
        public string In_position { get; set; }
        public IEnumerable<MAFCInputQDEReferenceDto> Reference { get; set; }
        public string In_head { get; set; } = "NETINCOM";
        public string In_frequency { get; set; } = "MONTHLY";
        public string In_amount { get; set; }
        public string In_accountbank { get; set; }
        public string In_debit_credit { get; set; } = "P";
        public string In_per_cont { get; set; } = "100";
        public string MsgName { get; set; } = MAFCDataEntry.InputQDE;
    }
    public class MAFCInputQDEReferenceDto
    {
        [JsonProperty("in_title")]
        public string In_title { get; set; }
        [JsonProperty("in_refereename")]
        public string In_refereename { get; set; }
        [JsonProperty("in_refereerelation")]
        public string In_refereerelation { get; set; }
        [JsonProperty("in_phone_1")]
        public string In_phone_1 { get; set; }
        [JsonProperty("in_phone_2")]
        public string In_phone_2 { get; set; }
    }
    public class MAFCInputQDEAddressDto
    {
        [JsonProperty("in_addresstype")]
        public string In_addresstype { get; set; }
        [JsonProperty("in_propertystatus")]
        public string In_propertystatus { get; set; }
        [JsonProperty("in_address1stline")]
        public string In_address1stline { get; set; }
        [JsonProperty("in_country")]
        public int In_country { get; set; } = 189;
        [JsonProperty("in_city")]
        public string In_city { get; set; }
        [JsonProperty("in_district")]
        public string In_district { get; set; }
        [JsonProperty("in_ward")]
        public string In_ward { get; set; }
        [JsonProperty("in_roomno")]
        public string In_roomno { get; set; }
        [JsonProperty("in_stayduratcuradd_y")]
        public int In_stayduratcuradd_y { get; set; }
        [JsonProperty("in_stayduratcuradd_m")]
        public int In_stayduratcuradd_m { get; set; }
        [JsonProperty("in_mailingaddress")]
        public string In_mailingaddress { get; set; }
        [JsonProperty("in_mobile")]
        public string In_mobile { get; set; }
        [JsonProperty("in_landlord")]
        public string In_landlord { get; set; }
        [JsonProperty("in_landmark")]
        public string In_landmark { get; set; }
    }
}

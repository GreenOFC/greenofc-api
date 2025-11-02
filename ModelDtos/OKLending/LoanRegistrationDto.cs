using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.OKLending
{
    public class LoanRegistrationDto
    {
        [JsonProperty("msgDsCd")]
        public string MsgDsCd { get; set; }

        [JsonProperty("id_no")]
        public string IdNo { get; set; }

        [JsonProperty("agency_code")]
        public string AgencyCode { get; set; }

        [JsonProperty("mobile_no2")]
        public string MobileNo2 { get; set; }

        [JsonProperty("cust_name")]
        public string CustName { get; set; }

        [JsonProperty("rgstrtn_dt1")]
        public string RgstrtnDt1 { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }

        [JsonProperty("cust_gender")]
        public string CustGender { get; set; }

        [JsonProperty("email1")]
        public string Email1 { get; set; }

        [JsonProperty("marray_yn")]
        public string MarrayYn { get; set; }

        [JsonProperty("job_code1")]
        public string JobCode1 { get; set; }

        [JsonProperty("work_name")]
        public string WorkName { get; set; }

        [JsonProperty("work_addr_detail")]
        public string WorkAddrDetail { get; set; }

        [JsonProperty("work_addr_sheng")]
        public string WorkAddrSheng { get; set; }

        [JsonProperty("work_addr_shi")]
        public string WorkAddrShi { get; set; }

        [JsonProperty("work_addr_qu")]
        public string WorkAddrQu { get; set; }

        [JsonProperty("work_tel_no2")]
        public string WorkTelNo2 { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("employ_type")]
        public string EmployType { get; set; }

        [JsonProperty("monthly_pay")]
        public string MonthlyPay { get; set; }

        [JsonProperty("join_date")]
        public string JoinDate { get; set; }

        [JsonProperty("resi_addr_detail")]
        public string ResiAddrDetail { get; set; }

        [JsonProperty("resi_addr_sheng")]
        public string ResiAddrSheng { get; set; }

        [JsonProperty("resi_addr_shi")]
        public string ResiAddrShi { get; set; }

        [JsonProperty("resi_addr_qu")]
        public string ResiAddrQu { get; set; }

        [JsonProperty("resi_in_date")]
        public string ResiInDate { get; set; }

        [JsonProperty("resi_own_sep")]
        public string ResiOwnSep { get; set; }

        [JsonProperty("resi_house_type")]
        public string ResiHouseType { get; set; }

        [JsonProperty("resi_tel_no2")]
        public string ResiTelNo2 { get; set; }

        [JsonProperty("apply_amt")]
        public string ApplyAmt { get; set; }

        [JsonProperty("loan_period")]
        public string LoanPeriod { get; set; }

        [JsonProperty("loan_use_type")]
        public string LoanUseType { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("acct_own_name")]
        public string AcctOwnName { get; set; }

        [JsonProperty("rcpt_vrfctn")]
        public string RcptVrfctn { get; set; }

        [JsonProperty("inflow_channel")]
        public string InflowChannel { get; set; }

        [JsonProperty("sefi")]
        public string Sefi { get; set; }

        [JsonProperty("id_card_back")]
        public string IdCardBack { get; set; }

        [JsonProperty("id_card_front")]
        public string IdCardFront { get; set; }
    }
}

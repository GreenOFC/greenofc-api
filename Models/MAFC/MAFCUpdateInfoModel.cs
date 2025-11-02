using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models.MAFC
{
    [BsonCollection(MongoCollection.MAFCUpdateInfo)]
    public class MAFCUpdateInfoModel : BaseDocument
    {
        public string CustomerId { get; set; }
        public bool IsDelete { get; set; }
        // Loan Info
        public int? In_schemeid { get; set; }
        public int? In_totalloanamountreq { get; set; }
        public int? In_tenure { get; set; }
        public string In_laa_app_ins_applicable { get; set; }

        public string In_loanpurpose { get; set; }
        public string In_priority_c { get; set; }
        public string In_title { get; set; }
        public string In_fname { get; set; }
        public string In_mname { get; set; }
        public string In_lname { get; set; }
        public string In_gender { get; set; }
        public string In_nationalid { get; set; }
        public string In_dob { get; set; }
        public string In_tax_code { get; set; }
        public int? In_presentjobyear { get; set; }
        public int? In_presentjobmth { get; set; }
        public string In_others { get; set; }
        public string In_position { get; set; }
        public string In_amount { get; set; }
        public string In_accountbank { get; set; }
        public string In_maritalstatus { get; set; }
        public string In_eduqualify { get; set; }
        public string In_noofdependentin { get; set; }
        public string In_paymentchannel { get; set; }
        public string In_nationalidissuedate { get; set; }
        public string In_familybooknumber { get; set; }
        public string In_idissuer { get; set; }
        public string In_spousename { get; set; }
        public string In_spouse_id_c { get; set; }
        public string In_bankname { get; set; }
        public string In_bankbranch { get; set; }
        public string In_accno { get; set; }

        public IEnumerable<MAFCUpdateAddressInfoModel> Address { get; set; }
        public IEnumerable<MAFCUpdateReferenceInfoModel> Reference { get; set; }

        public bool CompareObject(MAFCUpdateInfoModel old)
        {
            try
            {
                bool isChange = false;

                if (old.In_schemeid.Equals(this.In_schemeid)
                    && old.In_totalloanamountreq.Equals(this.In_totalloanamountreq)
                    && old.In_tenure.Equals(this.In_tenure)
                    && old.In_laa_app_ins_applicable.Equals(this.In_laa_app_ins_applicable))
                {
                    // 11/06: keep value
                    // this.In_schemeid = null;
                    // this.In_totalloanamountreq = null;
                    // this.In_tenure = null;
                    // this.In_laa_app_ins_applicable = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_loanpurpose.Equals(this.In_loanpurpose))
                {
                    this.In_loanpurpose = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_priority_c.Equals(this.In_priority_c))
                {
                    this.In_priority_c = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_title.Equals(this.In_title))
                {
                    this.In_title = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_fname.Equals(this.In_fname))
                {
                    this.In_fname = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_mname.Equals(this.In_mname))
                {
                    this.In_mname = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_lname.Equals(this.In_lname))
                {
                    this.In_lname = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_gender.Equals(this.In_gender))
                {
                    this.In_gender = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_nationalid.Equals(this.In_nationalid))
                {
                    this.In_nationalid = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_dob.Equals(this.In_dob))
                {
                    this.In_dob = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_tax_code.Equals(this.In_tax_code))
                {
                    this.In_tax_code = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_presentjobyear == this.In_presentjobyear)
                {
                    // 11/06: keep value
                    // this.In_presentjobyear = null;
                }
                else
                {
                    isChange = true;
                }
                if (old.In_presentjobmth == this.In_presentjobmth)
                {
                    // 11/06: keep value
                    // this.In_presentjobmth = null;
                }
                else
                {
                    isChange = true;
                }
                if (old.In_others.Equals(this.In_others))
                {
                    this.In_others = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_position.Equals(this.In_position))
                {
                    this.In_position = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_amount.Equals(this.In_amount))
                {
                    this.In_amount = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_accountbank.Equals(this.In_accountbank))
                {
                    this.In_accountbank = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_maritalstatus.Equals(this.In_maritalstatus))
                {
                    this.In_maritalstatus = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_eduqualify.Equals(this.In_eduqualify))
                {
                    this.In_eduqualify = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_noofdependentin.Equals(this.In_noofdependentin))
                {
                    this.In_noofdependentin = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_paymentchannel.Equals(this.In_paymentchannel))
                {
                    this.In_paymentchannel = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_nationalidissuedate.Equals(this.In_nationalidissuedate))
                {
                    this.In_nationalidissuedate = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_familybooknumber != null && old.In_familybooknumber.Equals(this.In_familybooknumber))
                {
                    this.In_familybooknumber = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_idissuer.Equals(this.In_idissuer))
                {
                    this.In_idissuer = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_spousename.Equals(this.In_spousename))
                {
                    this.In_spousename = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_spouse_id_c.Equals(this.In_spouse_id_c))
                {
                    this.In_spouse_id_c = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_bankname.Equals(this.In_bankname))
                {
                    this.In_bankname = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_bankbranch.Equals(this.In_bankbranch))
                {
                    this.In_bankbranch = "";
                }
                else
                {
                    isChange = true;
                }
                if (old.In_accno.Equals(this.In_accno))
                {
                    this.In_accno = "";
                }
                else
                {
                    isChange = true;
                }

                if (JsonConvert.SerializeObject(old.Address).Equals(JsonConvert.SerializeObject(this.Address)))
                {
                    this.Address = null;
                }
                else
                {
                    isChange = true;
                }

                if (JsonConvert.SerializeObject(old.Reference).Equals(JsonConvert.SerializeObject(this.Reference)))
                {
                    this.Reference = null;
                }
                else
                {
                    isChange = true;
                }
                return isChange;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }

    public class MAFCUpdateAddressInfoModel
    {
        public string In_addresstype { get; set; }
        public string In_propertystatus { get; set; }
        public string In_address1stline { get; set; }
        public int In_country { get; set; } = 189;
        public string In_city { get; set; }
        public string In_district { get; set; }
        public string In_ward { get; set; }
        public string In_roomno { get; set; }
        public string In_mobile { get; set; }
    }

    public class MAFCUpdateReferenceInfoModel
    {
        public string In_title { get; set; }
        public string In_refereename { get; set; }
        public string In_refereerelation { get; set; }
        public string In_phone_1 { get; set; }
    }
}

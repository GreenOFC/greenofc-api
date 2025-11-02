using _24hplusdotnetcore.Common.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.File
{
    public class ExportTelesalesMaModel
    {
        [Export(ExportName = "modified_time")]
        public DateTime? ModifiedTime { get; set; }

        [Export(ExportName = "created_time")]
        public DateTime? CreatedTime { get; set; }

        [Export(ExportName = "sales_name")]
        public string SalesName { get; set; }

        [Export(ExportName = "sales_code")]
        public string SalesCode { get; set; }


        [Export(ExportName = "has_admin")]
        public bool? HasAdmin { get; set; }


        [Export(ExportName = "final_house")]
        public string FinalHouse { get; set; }


        [Export(ExportName = "customer_name")]
        public string CustomerName { get; set; }


        [Export(ExportName = "customer_phone")]
        public string CustomerPhone { get; set; }


        [Export(ExportName = "customer_identity_card")]
        public string CustomerIdentityCard { get; set; }


        [Export(ExportName = "product_type")]
        public string ProductType { get; set; }


        [Export(ExportName = "product_code")]
        public string ProductCode { get; set; }


        [Export(ExportName = "profile_code")]
        public string ProfileCode { get; set; }


        [Export(ExportName = "has_insurance")]
        public bool HasInsurance { get; set; }


        [Export(ExportName = "loan_amount")]
        public decimal? LoanAmount { get; set; }

        [Export(ExportName = "sales_result")]
        public string SalesResult { get; set; }


        [Export(ExportName = "profile_status")]
        public string ProfileStatus { get; set; }

        [Export(ExportName = "reason_return_application")]
        public string ReasonReturnApplication { get; set; }
    }
}

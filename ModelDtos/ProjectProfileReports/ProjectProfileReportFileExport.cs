
using _24hplusdotnetcore.Common.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.ProjectProfileReports
{
    public class ProjectProfileReportFileExport
    {
        [Export(ExportName = "project")]
        public string Project { get; set; }

        [Export(ExportName = "app_creation_date")]
        public string AppCreationDate { get; set; }

        [Export(ExportName = "app_creation_month")]
        public string AppCreationMonth { get; set; }

        [Export(ExportName = "last_status_change_date")]
        public string LastStatusChangeDate { get; set; }

        [Export(ExportName = "disbursement_date")]
        public string DisbursementDate { get; set; }

        [Export(ExportName = "contract_number")]
        public string ContractNumber { get; set; }

        [Export(ExportName = "profile_code")]
        public string ProfileCode { get; set; }

        [Export(ExportName = "customer_name")]
        public string CustomerName { get; set; }

        [Export(ExportName = "product_name")]
        public string ProductName { get; set; }

        [Export(ExportName = "bhkv")]
        public string BHKV { get; set; }

        [Export(ExportName = "suggested_amount")]
        public decimal? SuggestedAmount { get; set; }

        [Export(ExportName = "disbursement_amount")]
        public decimal? DisbursementAmount { get; set; }

        [Export(ExportName = "profile_status")]
        public string ProfileStatus { get; set; }

        [Export(ExportName = "de_error_return")]
        public string DEErrorReturned { get; set; }

        [Export(ExportName = "sale_code")]
        public string SaleCode { get; set; }

        [Export(ExportName = "sale_name")]
        public string SaleName { get; set; }

        [Export(ExportName = "team_lead_name")]
        public string TeamLeadName { get; set; }

        [Export(ExportName = "pos_name")]
        public string PosName { get; set; }

        [Export(ExportName = "sale_chanel_name")]
        public string SaleChanelName { get; set; }

        [Export(ExportName = "unit")]
        public string Unit { get; set; }
    }
}

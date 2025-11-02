using _24hplusdotnetcore.Common.Attributes;
using System;
using System.Globalization;

namespace _24hplusdotnetcore.ModelDtos.ProjectProfileReports
{
    public class ProjectProfileReportImportFileData
    {
        [Column(1)]
        public string Project { get; set; }

        [Column(2)]
        public string AppCreationDate { get; set; }
        
        [Column(3)]
        public string AppCreationMonth { get; set; }

        [Column(4)]
        public string LastStatusChangeDate { get; set; }

        [Column(5)]
        public string DisbursementDate { get; set; }

        [Column(6)]
        public string ContractNumber { get; set; }

        [Column(7)]
        public string ProfileCode { get; set; }

        [Column(8)]
        public string CustomerName { get; set; }

        [Column(9)]
        public string ProductName { get; set; }

        [Column(10)]
        public string BHKV { get; set; }

        [Column(11)]
        public decimal? SuggestedAmount { get; set; }

        [Column(12)]
        public decimal? DisbursementAmount { get; set; }

        [Column(13)]
        public string ProfileStatus { get; set; }

        [Column(14)]
        public string DEErrorReturned { get; set; }

        [Column(15)]
        public string SaleCode { get; set; }

        [Column(16)]
        public string SaleName { get; set; }

        [Column(17)]
        public string TeamLeadName { get; set; }

        [Column(18)]
        public string PosName { get; set; }

        [Column(19)]
        public string SaleChanelName { get; set; }

        [Column(20)]
        public string Unit { get; set; }
    }
}

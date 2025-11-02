using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public class LeadCimbSeedData
    {
        public static IReadOnlyList<DataConfig> Terms = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadCimbTerm, GreenType = GreenType.GreenG, Key = "3", Value = "3 tháng" },
            new DataConfig{Type = DataConfigType.LeadCimbTerm, GreenType = GreenType.GreenG, Key = "6", Value = "6 tháng" },
            new DataConfig{Type = DataConfigType.LeadCimbTerm, GreenType = GreenType.GreenG, Key = "9", Value = "9 tháng" },
            new DataConfig{Type = DataConfigType.LeadCimbTerm, GreenType = GreenType.GreenG, Key = "12", Value = "12 tháng" },
            new DataConfig{Type = DataConfigType.LeadCimbTerm, GreenType = GreenType.GreenG, Key = "18", Value = "18 tháng" },
            new DataConfig{Type = DataConfigType.LeadCimbTerm, GreenType = GreenType.GreenG, Key = "24", Value = "24 tháng" },
            new DataConfig{Type = DataConfigType.LeadCimbTerm, GreenType = GreenType.GreenG, Key = "36", Value = "36 tháng" },
        };

        public static IReadOnlyList<DataConfig> EmploymentStatus = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadCimbEmploymentStatus, GreenType = GreenType.GreenG, Key = "EPM001", Value = "Toàn thời gian" },
            new DataConfig{Type = DataConfigType.LeadCimbEmploymentStatus, GreenType = GreenType.GreenG, Key = "EPM002", Value = "Bán thời gian" },
            new DataConfig{Type = DataConfigType.LeadCimbEmploymentStatus, GreenType = GreenType.GreenG, Key = "EPM003", Value = "Tự doanh" },
            new DataConfig{Type = DataConfigType.LeadCimbEmploymentStatus, GreenType = GreenType.GreenG, Key = "EPM004", Value = "Nghỉ hưu" },
            new DataConfig{Type = DataConfigType.LeadCimbEmploymentStatus, GreenType = GreenType.GreenG, Key = "EPM005", Value = "Thất nghiệp" },
        };

        public static IReadOnlyList<DataConfig> MaritalStatus = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadCimbMaritalStatus, GreenType = GreenType.GreenG, Key = "MR001", Value = "Kết hôn" },
            new DataConfig{Type = DataConfigType.LeadCimbMaritalStatus, GreenType = GreenType.GreenG, Key = "MR002", Value = "Độc thân" },
            new DataConfig{Type = DataConfigType.LeadCimbMaritalStatus, GreenType = GreenType.GreenG, Key = "MR003", Value = "Ly hôn" },
            new DataConfig{Type = DataConfigType.LeadCimbMaritalStatus, GreenType = GreenType.GreenG, Key = "MR004", Value = "Ly thân" },
            new DataConfig{Type = DataConfigType.LeadCimbMaritalStatus, GreenType = GreenType.GreenG, Key = "MR005", Value = "Góa" },
        };

        public static IReadOnlyList<DataConfig> Educations = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadCimbEducation, GreenType = GreenType.GreenG, Key = "EDU001", Value = "THPT" },
            new DataConfig{Type = DataConfigType.LeadCimbEducation, GreenType = GreenType.GreenG, Key = "EDU002", Value = "Cao đẳng" },
            new DataConfig{Type = DataConfigType.LeadCimbEducation, GreenType = GreenType.GreenG, Key = "EDU003", Value = "Đại học" },
            new DataConfig{Type = DataConfigType.LeadCimbEducation, GreenType = GreenType.GreenG, Key = "EDU004", Value = "Cao học" },
        };

        public static IReadOnlyList<DataConfig> ReferenceContactTypes = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadCimbReferenceContactType, GreenType = GreenType.GreenG, Key = "RELATIVE", Value = "Người thân" },
            new DataConfig{Type = DataConfigType.LeadCimbReferenceContactType, GreenType = GreenType.GreenG, Key = "FRIEND", Value = "Bạn bè" },
        };

        public static IReadOnlyList<DataConfig> CustomerTypes = new DataConfig[]
        {
             new DataConfig{Type = DataConfigType.LeadCimbCustomerType, GreenType = GreenType.GreenG, Key = "NORMAL", Value = "NORMAL" },
        };

        public static IReadOnlyList<DataConfig> LoanPurposes = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadCimbLoanPurpose, GreenType = GreenType.GreenG, Key = "3008", Value = "Mua sắm" },
        };

        public static IReadOnlyList<LeadCimbLoanInfomation> LoanInfomations = new LeadCimbLoanInfomation[]
        {
            new LeadCimbLoanInfomation
            {
                MaxAmount = 25000000,
                PackageSize = "Small",
                Details = new LeadCimbLoanInfomationDetail[]
                {
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 6, InterestRatePerYear = 33.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 9, InterestRatePerYear = 33.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 12, InterestRatePerYear = 35.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 18, InterestRatePerYear = 35.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 24, InterestRatePerYear = 35.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 36, InterestRatePerYear = 35.5 }
                }
            },
            new LeadCimbLoanInfomation
            {
                MinAmount = 25000000,
                MaxAmount = 50000000,
                PackageSize = "Medium",
                Details = new LeadCimbLoanInfomationDetail[]
                {
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 6, InterestRatePerYear = 30.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 9, InterestRatePerYear = 30.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 12, InterestRatePerYear = 28.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 24, InterestRatePerYear = 26.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 36, InterestRatePerYear = 26.5 }
                }
            },
            new LeadCimbLoanInfomation
            {
                MinAmount = 50000000,
                PackageSize = "Large",
                Details = new LeadCimbLoanInfomationDetail[]
                {
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 12, InterestRatePerYear = 24.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 18, InterestRatePerYear = 24.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 24, InterestRatePerYear = 23.5 },
                    new LeadCimbLoanInfomationDetail { NumberOfMonth = 36, InterestRatePerYear = 23.5 }
                }
            }
        };
    }
}

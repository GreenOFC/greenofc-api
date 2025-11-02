using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class LeadVibSeedData
    {
        public static IReadOnlyList<DataConfig> Incomes = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadVibIncome, Key = "UnderFiveMillion", Value = "Dưới 5 triệu" },
            new DataConfig{Type = DataConfigType.LeadVibIncome, Key = "MedianIncome", Value = "Từ 5 - 8 triệu" },
            new DataConfig{Type = DataConfigType.LeadVibIncome, Key = "OverEightMillion", Value = "Trên 8 triệu" },
        };

        public static IReadOnlyList<DataConfig> IncomeStreams = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadVibIncomeStream, Key = "Salary", Value = "Nhận lương" },
            new DataConfig{Type = DataConfigType.LeadVibIncomeStream, Key = "Business", Value = "Tự doanh" },
        };
    }
}

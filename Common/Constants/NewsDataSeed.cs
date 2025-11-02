using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class NewsDataSeed
    {
        public static IReadOnlyList<DataConfig> News = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.News, Key = "Policy", Value = "Chính sách" },
            new DataConfig{Type = DataConfigType.News, Key = "ProductAndProject", Value = "Sản phẩm và dự án" },
            new DataConfig{Type = DataConfigType.News, Key = "Technology", Value = "Công nghệ" },
            new DataConfig{Type = DataConfigType.News, Key = "Other", Value = "Khác" }
        };

        public static IReadOnlyList<DataConfig> NewsTags = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.NewsTag, Key = "MA", Value = "MA" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "EC", Value = "EC" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "MC", Value = "MC" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "SH", Value = "SH" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "FC", Value = "FC" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "SIN", Value = "SIN" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "F88", Value = "F88" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "MAEdu", Value = "MA Edu" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "24HCard", Value = "24HCard" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "CIMB", Value = "CIMB" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "LOT", Value = "LOT" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "VPS", Value = "VPS" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "PTF", Value = "PTF" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "ZUT", Value = "ZUT" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "INS", Value = "BẢO HIỂM" },
            new DataConfig{Type = DataConfigType.NewsTag, Key = "RecVGR", Value = "TUYỂN DỤNG VGR" },
        };
    }
}

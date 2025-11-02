using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class MAFCDataMapping
    {
        public static readonly ReadOnlyDictionary<string, string> LOAN_PURPOSE =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"Mua hàng", "A"},
                {"Chi phí y tế", "M"},
                {"Sửa nhà", "H"},
                {"Khác", "P"},
            });

        public static readonly ReadOnlyDictionary<string, string> WORKING_PRIORITY =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"Tài khoản ngân hàng", "Bank Statement"},
                {"Tiền mặt", "Pay Slip"},
                {"Khác", "None"},
                {"Có giấy phép kinh doanh", "Business License"},
                {"Không giấy phép kinh doanh", "No Business License"},
            });
        public static readonly ReadOnlyDictionary<string, int> WORKING_CONSTI =
            new ReadOnlyDictionary<string, int>(new Dictionary<string, int>() {
                {"Từ lương", 5},
                {"Từ kinh doanh", 8},
            });
        public static readonly ReadOnlyDictionary<string, string> WORKING_INCOME_METHOD =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"Tiền mặt", "N"},
                {"Tài khoản ngân hàng", "Y"},
            });
        public static readonly ReadOnlyDictionary<string, string> ADDRESS_STATUS =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"Chủ sở hữu", "O"},
                {"Nhà thuê", "R"},
                {"Ở nhà người thân", "F"},
            });
        public static readonly ReadOnlyDictionary<string, string> PERSONAL_MARIAL_STATUS =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"Đã kết hôn", "M"},
                {"Độc thân", "S"},
                {"Góa", "W"},
                {"Ly hôn", "W"},
                {"Ly thân", "W"},
                {"Khác", "W"},
            });
        public static readonly ReadOnlyDictionary<string, string> PERSONAL_EDUCATION =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"Tiểu học", "LG"},
                {"THCS", "LG"},
                {"Phổ thông", "HG"},
                {"Trung cấp", "CE"},
                {"Cao đẳng", "CE"},
                {"Đại học", "U"},
                {"Sau đại học", "UU"},
                {"Khác", "LG"},
            });
    }
}
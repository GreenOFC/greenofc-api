using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class MCNotificationMessage
    {
        public static string[] Return = {
            "SALE Đang chờ nhập liệu bổ sung - Return",
            "DE1 Từ chối nhập liệu",
            "SALE Bổ sung/chỉnh sửa hồ sơ - Return",
            };
        public static string[] Reject = {
            "DC Từ chối khoản vay",
            "AP Từ chối khoản vay",
            };
        public static string[] Cancel = {
            "Hủy khoản vay",
            "CA Hủy khoản vay",
            "AP Hủy khoản vay",
            "SALE Hủy khoản vay",
            "POS Hủy khoản vay",
            "Hủy tự động - Hết thời hạn xử lý hồ sơ",
            "DE1 Hủy khoản vay"
            };
        public static string Succes = "Hoàn thành";
        public static string ExportContract = "POS Đang đợi hoàn thiện VKTD";
    }
}
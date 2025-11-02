using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class LeadMcSeedData
    {
        public static IReadOnlyList<DataConfig> CicTypes = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadMcCicType, GreenType = GreenType.GreenC, Key = "cic", Value = "CIC" },
            new DataConfig{Type = DataConfigType.LeadMcCicType, GreenType = GreenType.GreenC, Key = "cmnd", Value = "CMND/CCCD" },
        };

        public static IReadOnlyList<DataConfig> Occupations = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "Worker", Value = "Công nhân" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "Employee", Value = "Nhân viên công ty" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "HouseKeeping", Value = "Nội trợ" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "StateEmployee", Value = "Công chức nhà nước" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "Soldier", Value = "Quân nhân" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "BusinessMan", Value = "Kinh doanh tự do" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "Retired", Value = "Hưu trí" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "BusinessHousehold", Value = "Hộ kinh doanh cá thể" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "Student", Value = "Học sinh/sinh viên" },
            new DataConfig{Type = DataConfigType.LeadMcOccupations, GreenType = GreenType.GreenC, Key = "Osin", Value = "Phụ việc, giúp việc" }
        };

        public static IReadOnlyList<DataConfig> Incomes = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadMcIncome, GreenType = GreenType.GreenC, Key = "Cash", Value = "Tiền mặt" },
            new DataConfig{Type = DataConfigType.LeadMcIncome, GreenType = GreenType.GreenC, Key = "MBBank", Value = "Qua tài khoản MB" },
            new DataConfig{Type = DataConfigType.LeadMcIncome, GreenType = GreenType.GreenC, Key = "OtherBank", Value = "Qua tài khoản ngân hàng khác" },
        };

        public static IReadOnlyList<DataConfig> Terms = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "6", Value = "6 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "9", Value = "9 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "12", Value = "12 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "15", Value = "15 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "18", Value = "18 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "21", Value = "21 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "24", Value = "24 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "27", Value = "27 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "30", Value = "30 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "33", Value = "33 Tháng" },
            new DataConfig{Type = DataConfigType.LeadMcTerm, GreenType = GreenType.GreenC, Key = "36", Value = "36 Tháng" },
        };

        public static IReadOnlyList<DataConfig> IdCardProvinces = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "001", Value = "Thành phố Hà Nội" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "003", Value = "Tỉnh Cao Bằng" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "002", Value = "Tỉnh Hà Giang" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "004", Value = "Tỉnh Bắc Kạn" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "005", Value = "Tỉnh Tuyên Quang" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "006", Value = "Tỉnh Lào Cai" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "007", Value = "Tỉnh Điện Biên" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "008", Value = "Tỉnh Lai Châu" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "009", Value = "Tỉnh Sơn La" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "010", Value = "Tỉnh Yên Bái" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "011", Value = "Tỉnh Hoà Bình" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "012", Value = "Tỉnh Thái Nguyên" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "013", Value = "Tỉnh Lạng Sơn" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "014", Value = "Tỉnh Quảng Ninh" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "015", Value = "Tỉnh Bắc Giang" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "016", Value = "Tỉnh Phú Thọ" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "017", Value = "Tỉnh Vĩnh Phúc" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "018", Value = "Tỉnh Bắc Ninh" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "019", Value = "Tỉnh Hải Dương" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "020", Value = "Thành phố Hải Phòng" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "021", Value = "Tỉnh Hưng Yên" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "022", Value = "Tỉnh Thái Bình" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "023", Value = "Tỉnh Hà Nam" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "024", Value = "Tỉnh Nam Định" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "025", Value = "Tỉnh Ninh Bình" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "026", Value = "Tỉnh Thanh Hóa" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "027", Value = "Tỉnh Nghệ An" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "028", Value = "Tỉnh Hà Tĩnh" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "029", Value = "Tỉnh Quảng Bình" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "030", Value = "Tỉnh Quảng Trị" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "031", Value = "Tỉnh Thừa Thiên Huế" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "032", Value = "Thành phố Đà Nẵng" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "033", Value = "Tỉnh Quảng Nam" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "034", Value = "Tỉnh Quảng Ngãi" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "035", Value = "Tỉnh Bình Định" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "036", Value = "Tỉnh Phú Yên" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "037", Value = "Tỉnh Khánh Hòa" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "038", Value = "Tỉnh Ninh Thuận" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "039", Value = "Tỉnh Bình Thuận" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "040", Value = "Tỉnh Kon Tum" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "041", Value = "Tỉnh Gia Lai" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "042", Value = "Tỉnh Đắk Lắk" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "043", Value = "Tỉnh Đắk Nông" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "044", Value = "Tỉnh Lâm Đồng" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "045", Value = "Tỉnh Bình Phước" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "046", Value = "Tỉnh Tây Ninh" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "047", Value = "Tỉnh Bình Dương" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "048", Value = "Tỉnh Đồng Nai" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "049", Value = "Tỉnh Bà Rịa - Vũng Tàu" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "050", Value = "Thành phố Hồ Chí Minh" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "051", Value = "Tỉnh Long An" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "052", Value = "Tỉnh Tiền Giang" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "053", Value = "Tỉnh Bến Tre" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "054", Value = "Tỉnh Trà Vinh" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "055", Value = "Tỉnh Vĩnh Long" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "056", Value = "Tỉnh Đồng Tháp" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "057", Value = "Tỉnh An Giang" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "058", Value = "Tỉnh Kiên Giang" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "059", Value = "Thành phố Cần Thơ" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "060", Value = "Tỉnh Hậu Giang" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "061", Value = "Tỉnh Sóc Trăng" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "062", Value = "Tỉnh Bạc Liêu" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "063", Value = "Tỉnh Cà Mau" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "064", Value = "CT cục cảnh sát ĐKQL cư trú và DLQG về dân cư" },
            new DataConfig{Type = DataConfigType.LeadMcIdCardProvince, GreenType = GreenType.GreenC, Key = "065", Value = "CT cục CS QLHC về TTXH" }
        };

        public static IReadOnlyList<DataConfig> Relationships = new DataConfig[]
        {
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "CP", Value = "Bạn bè/Đồng nghiệp sống cùng tỉnh" },
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "RHH", Value = "Họ hàng cùng hộ khẩu" },
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "OTH", Value = "Khác" },
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "HW", Value = "Vợ/Chồng" },
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "PS", Value = "Cha/mẹ" },
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "CN", Value = "Con" },
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "SB", Value = "Anh/chị/em ruột" },
             new DataConfig {Type = DataConfigType.Relationship, GreenType = GreenType.GreenC, Key = "RSP", Value = "Họ hàng sống cùng tỉnh" },
        };
    }

}

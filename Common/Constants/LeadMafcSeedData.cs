using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class LeadMafcSeedData
    {
        public static IReadOnlyList<DataConfig> Categories = new DataConfig[]
        {
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "HOSPITAL", Value = "HOSPITAL" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "NEW FAST LOAN", Value = "NEW FAST LOAN" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "LEAD CL", Value = "LEAD CL" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "Employee Cash Loan", Value = "Employee Cash Loan" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "TEACHER", Value = "TEACHER" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "SURROGATE", Value = "SURROGATE" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "MOVI CL", Value = "MOVI CL" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MAFCCategory, Key = "MY FINANCE", Value = "MY FINANCE" },
        };

        public static IReadOnlyList<DataConfig> CustomerTitles = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.CustomerTitle, GreenType = GreenType.GreenA, Key = "MR.", Value = "MR." },
            new DataConfig{ Type = DataConfigType.CustomerTitle, GreenType = GreenType.GreenA, Key = "MRS.", Value = "MRS." },
            new DataConfig{ Type = DataConfigType.CustomerTitle, GreenType = GreenType.GreenA, Key = "MS.", Value = "MS." }
        };

        public static IReadOnlyList<DataConfig> MaritalStatus = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenA, Key = "M", Value = "Đã kết hôn" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenA, Key = "A", Value = "Độc thân" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenA, Key = "W", Value = "Góa" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenA, Key = "D", Value = "Ly hôn" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenA, Key = "S", Value = "Ly thân" },
            new DataConfig{ Type = DataConfigType.MaritalStatus, GreenType = GreenType.GreenA, Key = "O", Value = "Khác" }
        };

        public static IReadOnlyList<DataConfig> EducationLevels = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "TH", Value = "Tiểu học" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "THCS", Value = "THCS" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "HighSchool", Value = "Phổ thông" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "Intermediate", Value = "Trung cấp" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "Colleage", Value = "Cao đẳng" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "University", Value = "Đại học" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "Doctor", Value = "Sau đại học" },
            new DataConfig{ Type = DataConfigType.EducationLevel, GreenType = GreenType.GreenA, Key = "Other", Value = "Khác" }
        };

        public static IReadOnlyList<DataConfig> PropertyStatus = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.PropertyStatus, GreenType = GreenType.GreenA, Key = "O", Value = "Chủ sở hữu" },
            new DataConfig{ Type = DataConfigType.PropertyStatus, GreenType = GreenType.GreenA, Key = "R", Value = "Nhà thuê" },
            new DataConfig{ Type = DataConfigType.PropertyStatus, GreenType = GreenType.GreenA, Key = "F", Value = "Ở nhà người thân" }
        };

        public static IReadOnlyList<DataConfig> Relationships = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "R", Value = "Người thân" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "CA", Value = "Người đồng vay" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "WH", Value = "Vợ chồng" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "F", Value = "Bạn bè" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "C", Value = "Đồng nghiệp" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "PR", Value = "Cha/Mẹ" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "CD", Value = "Con" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "SI", Value = "Anh/Chị/Em ruột" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "GP", Value = "Ông/Bà" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "PN", Value = "Cộng sự/Đối tác" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "NE", Value = "Hàng xóm" },
            new DataConfig{ Type = DataConfigType.Relationship, GreenType = GreenType.GreenA, Key = "OT", Value = "Khác" }
        };

        public static IReadOnlyList<DataConfig> Occupations = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "Worker", Value = "Công nhân" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "Employee", Value = "Nhân viên công ty" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "HouseKeeping", Value = "Nội trợ" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "StateEmployee", Value = "Công chức nhà nước" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "Soldier", Value = "Quân nhân" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "BusinessMan", Value = "Kinh doanh tự do" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "Retired", Value = "Hưu trí" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "BusinessHousehold", Value = "Hộ kinh doanh cá thể" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "Student", Value = "Học sinh/sinh viên" },
            new DataConfig{ Type = DataConfigType.Occupations, GreenType = GreenType.GreenA, Key = "Osin", Value = "Phụ việc, giúp việc" }
        };

        public static IReadOnlyList<DataConfig> IncomeMethods = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.IncomeMethod, GreenType = GreenType.GreenA, Key = "Cash", Value = "Tiền mặt" },
            new DataConfig{ Type = DataConfigType.IncomeMethod, GreenType = GreenType.GreenA, Key = "Bank", Value = "Tài khoản ngân hàng" },
        };

        public static IReadOnlyList<DataConfig> LoanPurposes = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenA, Key = "A", Value = "Mua hàng" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenA, Key = "M", Value = "Sửa nhà" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenA, Key = "H", Value = "Chi phí y tế" },
            new DataConfig{ Type = DataConfigType.LoanPurpose, GreenType = GreenType.GreenA, Key = "P", Value = "Khác" }
        };

        public static IReadOnlyList<DataConfig> LoanPaymentDates = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.LoanPaymentDate, GreenType = GreenType.GreenA, Key = "1", Value = "1" },
            new DataConfig{ Type = DataConfigType.LoanPaymentDate, GreenType = GreenType.GreenA, Key = "5", Value = "5" },
            new DataConfig{ Type = DataConfigType.LoanPaymentDate, GreenType = GreenType.GreenA, Key = "10", Value = "10" },
            new DataConfig{ Type = DataConfigType.LoanPaymentDate, GreenType = GreenType.GreenA, Key = "15", Value = "15" },
            new DataConfig{ Type = DataConfigType.LoanPaymentDate, GreenType = GreenType.GreenA, Key = "25", Value = "25" },
        };

        public static IReadOnlyList<DataConfig> Terms = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenA, Key = "6", Value = "6" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenA, Key = "12", Value = "12" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenA, Key = "18", Value = "18" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenA, Key = "24", Value = "24" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenA, Key = "30", Value = "30" },
            new DataConfig{ Type = DataConfigType.Term, GreenType = GreenType.GreenA, Key = "36", Value = "36" }
        };

        public static IReadOnlyList<DataConfig> WorkingPriorities = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.WorkingPriority, GreenType = GreenType.GreenA, Key = "Tài khoản ngân hàng", Value = "Tài khoản ngân hàng(Bank Statement)" },
            new DataConfig{ Type = DataConfigType.WorkingPriority, GreenType = GreenType.GreenA, Key = "Tiền mặt", Value = "Tiền mặt(Pay Slip)" },
            new DataConfig{ Type = DataConfigType.WorkingPriority, GreenType = GreenType.GreenA, Key = "Khác", Value = "Khác(None)" },
            new DataConfig{ Type = DataConfigType.WorkingPriority, GreenType = GreenType.GreenA, Key = "Có giấy phép kinh doanh", Value = "Có giấy phép kinh doanh(Business License)" },
            new DataConfig{ Type = DataConfigType.WorkingPriority, GreenType = GreenType.GreenA, Key = "Không giấy phép kinh doanh", Value = "Không giấy phép kinh doanh(No Business License)" },
        };

        public static IReadOnlyList<DataConfig> Constitutions = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.Constitution, GreenType = GreenType.GreenA, Key = "SALARIED", Value = "Từ lương" },
            new DataConfig{ Type = DataConfigType.Constitution, GreenType = GreenType.GreenA, Key = "SELF EMPLOYED", Value = "Từ kinh doanh" }
        };

        public static IReadOnlyList<DataConfig> WokingStatus = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.WokingStatus, GreenType = GreenType.GreenA, Key = "HEADOFF", Value = "Trụ sở chính" },
            new DataConfig{ Type = DataConfigType.WokingStatus, GreenType = GreenType.GreenA, Key = "BCHOFF", Value = "Chi nhánh" }
        };

        public static IReadOnlyList<DataConfig> SecretWithTypes = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.SecretWithType, GreenType = GreenType.GreenA, Key = "N", Value = "Không" },
            new DataConfig{ Type = DataConfigType.SecretWithType, GreenType = GreenType.GreenA, Key = "R", Value = "Người thân" },
            new DataConfig{ Type = DataConfigType.SecretWithType, GreenType = GreenType.GreenA, Key = "WH", Value = "Vợ/Chồng" },
            new DataConfig{ Type = DataConfigType.SecretWithType, GreenType = GreenType.GreenA, Key = "O", Value = "Khác" },
        };

        public static IReadOnlyList<DataConfig> IdCardProvinces = new DataConfig[]
        {
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "001", Value = "Thành phố Hà Nội" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "003", Value = "Tỉnh Cao Bằng" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "002", Value = "Tỉnh Hà Giang" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "004", Value = "Tỉnh Bắc Kạn" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "005", Value = "Tỉnh Tuyên Quang" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "006", Value = "Tỉnh Lào Cai" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "007", Value = "Tỉnh Điện Biên" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "008", Value = "Tỉnh Lai Châu" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "009", Value = "Tỉnh Sơn La" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "010", Value = "Tỉnh Yên Bái" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "011", Value = "Tỉnh Hoà Bình" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "012", Value = "Tỉnh Thái Nguyên" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "013", Value = "Tỉnh Lạng Sơn" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "014", Value = "Tỉnh Quảng Ninh" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "015", Value = "Tỉnh Bắc Giang" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "016", Value = "Tỉnh Phú Thọ" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "017", Value = "Tỉnh Vĩnh Phúc" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "018", Value = "Tỉnh Bắc Ninh" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "019", Value = "Tỉnh Hải Dương" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "020", Value = "Thành phố Hải Phòng" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "021", Value = "Tỉnh Hưng Yên" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "022", Value = "Tỉnh Thái Bình" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "023", Value = "Tỉnh Hà Nam" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "024", Value = "Tỉnh Nam Định" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "025", Value = "Tỉnh Ninh Bình" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "026", Value = "Tỉnh Thanh Hóa" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "027", Value = "Tỉnh Nghệ An" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "028", Value = "Tỉnh Hà Tĩnh" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "029", Value = "Tỉnh Quảng Bình" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "030", Value = "Tỉnh Quảng Trị" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "031", Value = "Tỉnh Thừa Thiên Huế" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "032", Value = "Thành phố Đà Nẵng" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "033", Value = "Tỉnh Quảng Nam" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "034", Value = "Tỉnh Quảng Ngãi" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "035", Value = "Tỉnh Bình Định" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "036", Value = "Tỉnh Phú Yên" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "037", Value = "Tỉnh Khánh Hòa" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "038", Value = "Tỉnh Ninh Thuận" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "039", Value = "Tỉnh Bình Thuận" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "040", Value = "Tỉnh Kon Tum" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "041", Value = "Tỉnh Gia Lai" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "042", Value = "Tỉnh Đắk Lắk" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "043", Value = "Tỉnh Đắk Nông" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "044", Value = "Tỉnh Lâm Đồng" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "045", Value = "Tỉnh Bình Phước" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "046", Value = "Tỉnh Tây Ninh" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "047", Value = "Tỉnh Bình Dương" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "048", Value = "Tỉnh Đồng Nai" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "049", Value = "Tỉnh Bà Rịa - Vũng Tàu" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "050", Value = "Thành phố Hồ Chí Minh" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "051", Value = "Tỉnh Long An" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "052", Value = "Tỉnh Tiền Giang" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "053", Value = "Tỉnh Bến Tre" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "054", Value = "Tỉnh Trà Vinh" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "055", Value = "Tỉnh Vĩnh Long" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "056", Value = "Tỉnh Đồng Tháp" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "057", Value = "Tỉnh An Giang" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "058", Value = "Tỉnh Kiên Giang" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "059", Value = "Thành phố Cần Thơ" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "060", Value = "Tỉnh Hậu Giang" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "061", Value = "Tỉnh Sóc Trăng" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "062", Value = "Tỉnh Bạc Liêu" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "063", Value = "Tỉnh Cà Mau" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "064", Value = "CT cục cảnh sát ĐKQL cư trú và DLQG về dân cư" },
            new DataConfig{ Type = DataConfigType.IdCardProvince, GreenType = GreenType.GreenA, Key = "065", Value = "CT cục CS QLHC về TTXH" }
        };

        public static IReadOnlyList<DataConfig> CustomerType = new DataConfig[]
        {
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MafcCustomerType, Key = "NEW", Value = "Khách hàng mới" },
            new DataConfig{GreenType = GreenType.GreenA, Type = DataConfigType.MafcCustomerType, Key = "OLD", Value = "Khách hàng cũ" },
        };
    }
}

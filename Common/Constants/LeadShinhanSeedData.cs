using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class LeadShinhanSeedData
    {
        public static IReadOnlyList<DataConfig> Terms = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadShinhanTerm, GreenType = GreenType.GreenE, Key = "12", Value = "12 Tháng" },
            new DataConfig{Type = DataConfigType.LeadShinhanTerm, GreenType = GreenType.GreenE, Key = "24", Value = "24 Tháng" },
            new DataConfig{Type = DataConfigType.LeadShinhanTerm, GreenType = GreenType.GreenE, Key = "30", Value = "30 Tháng" },
            new DataConfig{Type = DataConfigType.LeadShinhanTerm, GreenType = GreenType.GreenE, Key = "36", Value = "36 Tháng" },
            new DataConfig{Type = DataConfigType.LeadShinhanTerm, GreenType = GreenType.GreenE, Key = "42", Value = "42 Tháng" },
            new DataConfig{Type = DataConfigType.LeadShinhanTerm, GreenType = GreenType.GreenE, Key = "48", Value = "48 Tháng" },
        };
        
        public static IReadOnlyList<DataConfig> Occupation = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Worker", Value = "Công nhân" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Employee", Value = "Nhân viên công ty" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "HouseKeeping", Value = "Nội trợ" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "StateEmployee", Value = "Công chức nhà nước" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Soldier", Value = "Quân nhân" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "BusinessMan", Value = "Kinh doanh tự do" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Retired", Value = "Hưu trí" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "BusinessHousehold", Value = "Hộ kinh doanh cá thể" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Student", Value = "Học sinh/sinh viên" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Osin", Value = "Phụ việc, giúp việc" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Medican", Value = "Nhân viên ngành Y tế" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Teacher", Value = "Giáo viên/giảng viên" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Sale", Value = "Nhân viên Kinh doanh" },
            new DataConfig{Type = DataConfigType.LeadShinhanOccupation, GreenType = GreenType.GreenE, Key = "Guard", Value = "Bảo vệ" },
        };
        public static IReadOnlyList<DataConfig> IncomeMethod = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadShinhanIncomeMethod, GreenType = GreenType.GreenE, Key = "Cash", Value = "Tiền mặt" },
            new DataConfig{Type = DataConfigType.LeadShinhanIncomeMethod, GreenType = GreenType.GreenE, Key = "MBBank", Value = "Qua tài khoản MB" },
            new DataConfig{Type = DataConfigType.LeadShinhanIncomeMethod, GreenType = GreenType.GreenE, Key = "OtherBank", Value = "Qua tài khoản ngân hàng khác" },
        };
        public static IReadOnlyList<DataConfig> LeadShinhanSignAddresses = new DataConfig[]
        {
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng Trệt, Tòa nhà HQC Plaza, Chung cư CC1 Khu 2, Khu tái định cư Bến Lức, Khu chức năng số 17, Đô thị Nam Thành phố, xã An Phú Tây, Huyện Bình Chánh, TP.Hồ Chí Minh",
                Value = "Tầng Trệt, Tòa nhà HQC Plaza, Chung cư CC1 Khu 2, Khu tái định cư Bến Lức, Khu chức năng số 17, Đô thị Nam Thành phố, xã An Phú Tây, Huyện Bình Chánh, TP.Hồ Chí Minh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng trệt-tầng 7, 95A Phan Đăng Lưu, P7, Q. Phú Nhuận, TP. Hồ Chí Minh",
                Value = "Tầng trệt-tầng 7, 95A Phan Đăng Lưu, P7, Q. Phú Nhuận, TP. Hồ Chí Minh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 10, Tòa Nhà Pico, 20 Cộng Hòa,P12, Q. Tân Bình, TP. Hồ Chí Minh",
                Value = "Tầng 10, Tòa Nhà Pico, 20 Cộng Hòa,P12, Q. Tân Bình, TP. Hồ Chí Minh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng trệt, 37 Tôn Đức Thắng, P.Bến Nghé, Q1, TP.Hồ Chí Minh",
                Value = "Tầng trệt, 37 Tôn Đức Thắng, P.Bến Nghé, Q1, TP.Hồ Chí Minh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1, 86 Tản Đà, P11, Q5, TP. Hồ Chí Minh",
                Value = "Tầng 1, 86 Tản Đà, P11, Q5, TP. Hồ Chí Minh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng G, 2 & 23, Trung tâm Thương mại Sài Gòn, 37 Tôn Đức Thắng, P.Bến Nghé, Q.1",
                Value = "Tầng G, 2 & 23, Trung tâm Thương mại Sài Gòn, 37 Tôn Đức Thắng, P.Bến Nghé, Q.1" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 12A, 14 & 15, Tòa nhà Pico, 20 Cộng Hòa, P.12, Q.Tân Bình",
                Value = "Tầng 12A, 14 & 15, Tòa nhà Pico, 20 Cộng Hòa, P.12, Q.Tân Bình" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Khu C6 & D4, Tầng 1, Tòa nhà Tản Đà, 86 Tản Đà, P.11, Q.5",
                Value = "Khu C6 & D4, Tầng 1, Tòa nhà Tản Đà, 86 Tản Đà, P.11, Q.5" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng trệt & 7, 95A Phan Đăng Lưu, P.7, Q.Phú Nhuận",
                Value = "Tầng trệt & 7, 95A Phan Đăng Lưu, P.7, Q.Phú Nhuận" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng trệt, 6, 7 & 9, Tòa nhà Tài Tâm, 39A Ngô Quyền, P.Hàng Bài, Q.Hoàn Kiếm, HN",
                Value = "Tầng trệt, 6, 7 & 9, Tòa nhà Tài Tâm, 39A Ngô Quyền, P.Hàng Bài, Q.Hoàn Kiếm, HN" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng trệt, Tòa nhà Lotus, 2 Duy Tân, P.Dịch Vọng Hậu, Q.Cầu Giấy, HN",
                Value = "Tầng trệt, Tòa nhà Lotus, 2 Duy Tân, P.Dịch Vọng Hậu, Q.Cầu Giấy, HN" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 6, Tòa nhà Ngọc Khánh, Số 01 Phạm Huy Thông, P.Ngọc Khánh, Q.Ba Đình, TP.Hà Nội",
                Value = "Tầng 6, Tòa nhà Ngọc Khánh, Số 01 Phạm Huy Thông, P.Ngọc Khánh, Q.Ba Đình, TP.Hà Nội" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 19, Tòa nhà Sông Đà, Khu B, Số 18 Phạm Hùng, P.Mỹ Đình I, Q.Nam Từ Liêm, Hà Nội",
                Value = "Tầng 19, Tòa nhà Sông Đà, Khu B, Số 18 Phạm Hùng, P.Mỹ Đình I, Q.Nam Từ Liêm, Hà Nội" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 6, tòa nhà Ngọc Khánh, số 1 Phạm Huy Thông, Ngọc Khánh, Ba Đình, Hà Nội",
                Value = "Tầng 6, tòa nhà Ngọc Khánh, số 1 Phạm Huy Thông, Ngọc Khánh, Ba Đình, Hà Nội" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 6, Lotus Building - số 2 Duy Tân - Cầu Giấy - Hà Nội",
                Value = "Tầng 6, Lotus Building - số 2 Duy Tân - Cầu Giấy - Hà Nội" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 6, 39A Ngô Quyền, Hoàn Kiếm, Hà Nội",
                Value = "Tầng 6, 39A Ngô Quyền, Hoàn Kiếm, Hà Nội" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1 - Lotus Building - số 2 Duy Tân - Cầu Giấy - Hà Nội",
                Value = "Tầng 1 - Lotus Building - số 2 Duy Tân - Cầu Giấy - Hà Nội" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2 & 7, Tòa nhà Bưu Điện, 271 Nguyễn Văn Linh, Q.Thanh Khê, TP.Đà Nẵng",
                Value = "Tầng 2 & 7, Tòa nhà Bưu Điện, 271 Nguyễn Văn Linh, Q.Thanh Khê, TP.Đà Nẵng" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3, Tòa nhà Vinfast, 57 Quang Trung, Thành Phố Bắc Giang",
                Value = "Tầng 3, Tòa nhà Vinfast, 57 Quang Trung, Thành Phố Bắc Giang" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2, Tòa Nhà Hoa Mai, 262 Ngô Quyền, TP Hải Dương",
                Value = "Tầng 2, Tòa Nhà Hoa Mai, 262 Ngô Quyền, TP Hải Dương" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1, Tòa Nhà Nam Định, 91 Điện Biên, Cửa Bắc, TP Nam Định",
                Value = "Tầng 1, Tòa Nhà Nam Định, 91 Điện Biên, Cửa Bắc, TP Nam Định" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3,Tòa Nhà Hoàng Gia- 259 Đường Quang Trung -Tp. Thái Nguyên",
                Value = "Tầng 3,Tòa Nhà Hoàng Gia- 259 Đường Quang Trung -Tp. Thái Nguyên" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2. Khách Sạn Lam Sơn - 253 Đường Trần Phú - Phường Lam Sơn - TP. Thanh Hóa",
                Value = "Tầng 2. Khách Sạn Lam Sơn - 253 Đường Trần Phú - Phường Lam Sơn - TP. Thanh Hóa" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1, Tòa nhà Khánh Dư, 14-16 Tôn Đức Thắng, Vĩnh Yên, Vĩnh Phúc",
                Value = "Tầng 1, Tòa nhà Khánh Dư, 14-16 Tôn Đức Thắng, Vĩnh Yên, Vĩnh Phúc" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3, Tòa nhà FPT, 42 Lê Thành Phương, Nha Trang",
                Value = "Tầng 3, Tòa nhà FPT, 42 Lê Thành Phương, Nha Trang" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 4, 52A Tăng Bạt Hổ, Tòa Nhà An Phú Thịnh , TP Qui Nhơn",
                Value = "Tầng 4, 52A Tăng Bạt Hổ, Tòa Nhà An Phú Thịnh , TP Qui Nhơn" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 4, Điện máy Quốc Thắng, 469 Hùng Vương, An Sơn, Tam Kỳ, Quảng Nam.",
                Value = "Tầng 4, Điện máy Quốc Thắng, 469 Hùng Vương, An Sơn, Tam Kỳ, Quảng Nam." 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2, 451 Quang Trung. P. Nguyễn Nghiêm, Tp. Quảng Ngãi.",
                Value = "Tầng 2, 451 Quang Trung. P. Nguyễn Nghiêm, Tp. Quảng Ngãi." 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3.57-59A CMT8, phường An Hòa,quận Ninh Kiều, thành phố Cần Thơ",
                Value = "Tầng 3.57-59A CMT8, phường An Hòa,quận Ninh Kiều, thành phố Cần Thơ" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1 ,91 Nam Kỳ Khởi Nghĩa, P3. TP Vũng Tàu.",
                Value = "Tầng 1 ,91 Nam Kỳ Khởi Nghĩa, P3. TP Vũng Tàu." 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 6, tòa nhà HCC, Số 28 đường Lý Thường Kiệt, Vĩnh Ninh, TP Huế",
                Value = "Tầng 6, tòa nhà HCC, Số 28 đường Lý Thường Kiệt, Vĩnh Ninh, TP Huế" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tòa nhà HQC Plaza, Đường Nguyễn Văn Linh, xã An Phú Tây, Huyện Bình Chánh",
                Value = "Tòa nhà HQC Plaza, Đường Nguyễn Văn Linh, xã An Phú Tây, Huyện Bình Chánh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2, Tòa Nhà Bưu Điện, 271 Nguyễn Văn Linh, Đà Nẵng",
                Value = "Tầng 2, Tòa Nhà Bưu Điện, 271 Nguyễn Văn Linh, Đà Nẵng" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 4, Tòa nhà Việt Long, Lô CC04 Lý Thái Tổ, P. Ninh Xá, Tp. Bắc Ninh",
                Value = "Tầng 4, Tòa nhà Việt Long, Lô CC04 Lý Thái Tổ, P. Ninh Xá, Tp. Bắc Ninh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 14, Tòa Nhà Pegasus, 53-55 Võ Thị Sáu,P. Quyết Thắng, TP. Biên Hòa, Đồng Nai.",
                Value = "Tầng 14, Tòa Nhà Pegasus, 53-55 Võ Thị Sáu,P. Quyết Thắng, TP. Biên Hòa, Đồng Nai." 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1, tòa nhà Aeon Citimart Bình Dương, 215A Yersin, Phường Phú Cường, Thành phố Thủ Dầu Một, Tỉnh Bình Dương",
                Value = "Tầng 1, tòa nhà Aeon Citimart Bình Dương, 215A Yersin, Phường Phú Cường, Thành phố Thủ Dầu Một, Tỉnh Bình Dương" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 14, tòa nhà Pegasus Plaza , 53-55 Võ Thị Sáu, Phường Quyết Thắng, Thành Phố Biên Hòa, Tỉnh Đồng Nai.",
                Value = "Tầng 14, tòa nhà Pegasus Plaza , 53-55 Võ Thị Sáu, Phường Quyết Thắng, Thành Phố Biên Hòa, Tỉnh Đồng Nai." 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3, Tòa nhà Toyota Cần Thơ, 57 - 59A Cách Mạng Tháng 8, Q.Ninh Kiều, TP.Cần Thơ",
                Value = "Tầng 3, Tòa nhà Toyota Cần Thơ, 57 - 59A Cách Mạng Tháng 8, Q.Ninh Kiều, TP.Cần Thơ" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1, 91 Nam Kỳ Khởi Nghĩa, P.3, TP.Vũng Tàu",
                Value = "Tầng 1, 91 Nam Kỳ Khởi Nghĩa, P.3, TP.Vũng Tàu" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3, Tòa nhà FPT, 42 Lê Thành Phương, P.Vạn Thắng, TP.Nha Trang, Tỉnh Khánh Hòa",
                Value = "Tầng 3, Tòa nhà FPT, 42 Lê Thành Phương, P.Vạn Thắng, TP.Nha Trang, Tỉnh Khánh Hòa" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1, Tòa nhà Khánh Dư, 14 - 16 Tôn Đức Thắng, P.Khai Quang, TP.Vĩnh Yên, Tỉnh Vĩnh Phúc",
                Value = "Tầng 1, Tòa nhà Khánh Dư, 14 - 16 Tôn Đức Thắng, P.Khai Quang, TP.Vĩnh Yên, Tỉnh Vĩnh Phúc" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2, toà nhà Hoa Mai, số 262 Ngô Quyền, TP. Hải Dương",
                Value = "Tầng 2, toà nhà Hoa Mai, số 262 Ngô Quyền, TP. Hải Dương" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 4, Tòa nhà Quốc Thắng, 469 Hùng Vương, TP.Tam Kỳ, Tỉnh Quảng Nam.",
                Value = "Tầng 4, Tòa nhà Quốc Thắng, 469 Hùng Vương, TP.Tam Kỳ, Tỉnh Quảng Nam." 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2, 451 Quang Trung, P.Nguyễn Nghiêm, Tỉnh Quảng Ngãi",
                Value = "Tầng 2, 451 Quang Trung, P.Nguyễn Nghiêm, Tỉnh Quảng Ngãi" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 4, Tòa nhà Việt Long, Block CC04, Lý Thái Tổ, Q.Ninh Xá, Tỉnh Bắc Ninh",
                Value = "Tầng 4, Tòa nhà Việt Long, Block CC04, Lý Thái Tổ, Q.Ninh Xá, Tỉnh Bắc Ninh" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 4, TTTM An Phú Thịnh, 52A Tăng Bạt Hổ, P.Lê Lợi, TP.Quy Nhơn",
                Value = "Tầng 4, TTTM An Phú Thịnh, 52A Tăng Bạt Hổ, P.Lê Lợi, TP.Quy Nhơn" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 2, TTTM - Khách sạn Lam Sơn, 253 Trần Phú, P.Ba Đình, Tỉnh Thanh Hóa",
                Value = "Tầng 2, TTTM - Khách sạn Lam Sơn, 253 Trần Phú, P.Ba Đình, Tỉnh Thanh Hóa" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3, Tòa nhà Hoàng Gia, 259 Quang Trung, P.Tân Thịnh, Tỉnh Thái Nguyên",
                Value = "Tầng 3, Tòa nhà Hoàng Gia, 259 Quang Trung, P.Tân Thịnh, Tỉnh Thái Nguyên" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 1, Tòa Nhà Nam Định, Số 91 Điện Biên, P.Cửa Bắc, TP.Nam Định",
                Value = "Tầng 1, Tòa Nhà Nam Định, Số 91 Điện Biên, P.Cửa Bắc, TP.Nam Định" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 6, Tòa nhà HCC Tower, 28 Lý Thường Kiệt, P.Vĩnh Ninh, TP.Huế",
                Value = "Tầng 6, Tòa nhà HCC Tower, 28 Lý Thường Kiệt, P.Vĩnh Ninh, TP.Huế" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Tầng 3, Số 57, Đường Quang Trung, TP.Bắc Giang, Tỉnh Bắc Giang",
                Value = "Tầng 3, Số 57, Đường Quang Trung, TP.Bắc Giang, Tỉnh Bắc Giang" 
            },
            new DataConfig{
                Type = DataConfigType.LeadShinhanSignAddress, 
                GreenType = GreenType.GreenE, 
                Key = "Khách sạn Mêkông Mỹ Tho, Số 1A, Đường Tết Mậu Thân, Phường 4, Thành phố Mỹ Tho, Tỉnh Tiền Giang",
                Value = "Khách sạn Mêkông Mỹ Tho, Số 1A, Đường Tết Mậu Thân, Phường 4, Thành phố Mỹ Tho, Tỉnh Tiền Giang" 
            },
        };

    }

}

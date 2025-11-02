using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public class LeadEcSeedData
    {
        public static IReadOnlyList<DataConfig> EmployeeTypes = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadEcEmployeeType, GreenType = GreenType.GreenD, Key = "E", Value = "Đi làm hưởng lương" },
            new DataConfig{Type = DataConfigType.LeadEcEmployeeType, GreenType = GreenType.GreenD, Key = "SE", Value = "Tự kinh doanh" },
            new DataConfig{Type = DataConfigType.LeadEcEmployeeType, GreenType = GreenType.GreenD, Key = "RP", Value = "Hưởng lương hưu" },
            new DataConfig{Type = DataConfigType.LeadEcEmployeeType, GreenType = GreenType.GreenD, Key = "FE", Value = "Làm nghề tự do" },
        };

        public static IReadOnlyList<DataConfig> DisbursementMethods = new DataConfig[]
        {
            new DataConfig{Type = DataConfigType.LeadEcDisbursementMethod, GreenType = GreenType.GreenD, Key = "cash", Value = "Tiền mặt" },
            new DataConfig{Type = DataConfigType.LeadEcDisbursementMethod, GreenType = GreenType.GreenD, Key = "bank", Value = "Ngân hàng" },
        };

        public static IReadOnlyList<DataConfig> JobTypes = new DataConfig[]
        {
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "FIM", Value = "Ngư dân" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "FIN", Value = "Người sống bằng lợi tức (tiền cho thuê cố định" }, 
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "BM", Value = "Nhân viên kinh doanh" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "DCC", Value = "Nhân viên thu hồi nợ các tổ chức tín dụng" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "RPT", Value = "Phóng viên/Nhà báo" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "RHR", Value = "Nhà hàng/khách sạn/quán ăn" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "INA", Value = "Đại lý bảo hiểm" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "ENA", Value = "Kỹ sư" }, 
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "POA", Value = "Công an/Quân đội" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "WOK", Value = "Công nhân" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "ACJ", Value = "Luật sư/Thư ký toà án/Thẩm phán/Chánh án/Thi hành án hoặc các vị trí liên quan đến toà án " },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "CHTEP", Value = "Nghề thủ công (cắt tóc" }, 
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "STE", Value = "Công nhân viên chức nhà nước" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "CHEF", Value = "Đầu bếp" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "TRL", Value = "Giáo viên/giảng viên" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "RTE", Value = "Hưu trí" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "PUM", Value = "Lao động phổ thông" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "PAP", Value = "Kinh doanh dịch vụ cầm đồ" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "DCP", Value = "Bác sĩ/Y tá/Dược sĩ" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "HAK", Value = "Bán hàng tự do (không có địa điểm cố định)" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "SEC", Value = "Bảo vệ" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "DRI", Value = "Tài xế /Xe ôm" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "HHG", Value = "Tạp vụ/Giúp việc nhà" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "UNT", Value = "Thất nghiệp" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "STS", Value = "Tiểu thương" }, 
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "SSTS", Value = "Tự kinh doanh dịch vụ vận tải" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "ATH", Value = "Vận động viên" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "ARS", Value = "Văn nghệ sĩ" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "OTH", Value = "Khác" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "SFF", Value = "Nhân viên văn phòng" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "HW", Value = "Nội trợ" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "FAM", Value = "Nông dân (trồng trọt/chăn nuôi)" },
            new DataConfig {Type = DataConfigType.LeadEcJobType, GreenType = GreenType.GreenD, Key = "BPR", Value = "Lĩnh vực tôn giáo" }, 
        };

        public static IReadOnlyList<DataConfig> Relationships = new DataConfig[]
        {
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "CP", Value = "Bạn bè/Đồng nghiệp sống cùng tỉnh" },
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "RHH", Value = "Họ hàng cùng hộ khẩu" },
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "OTH", Value = "Khác" },
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "HW", Value = "Vợ/Chồng" },
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "PS", Value = "Cha/mẹ" },
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "CN", Value = "Con" },
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "SB", Value = "Anh/chị/em ruột" },
             new DataConfig {Type = DataConfigType.LeadEcRelationship, GreenType = GreenType.GreenD, Key = "RSP", Value = "Họ hàng sống cùng tỉnh" },
        };

        public static IReadOnlyList<DataConfig> Purposes = new DataConfig[]
        {
             new DataConfig {Type = DataConfigType.LeadEcPurpose, GreenType = GreenType.GreenD, Key = "HR", Value = "Chi phí sửa chữa nhà ở" },
             new DataConfig {Type = DataConfigType.LeadEcPurpose, GreenType = GreenType.GreenD, Key = "ROL", Value = "Mua phương tiện đi lại, đồ dùng, trang thiết bị gia đình" },
             new DataConfig {Type = DataConfigType.LeadEcPurpose, GreenType = GreenType.GreenD, Key = "EMT", Value = "Chi phí học tập, chữa bệnh, du lịch, văn hóa, thể dục, thể thao" }
        };
        
        public static IReadOnlyList<DataConfig> MaritalStatus = new DataConfig[]
        {	
             new DataConfig {Type = DataConfigType.LeadEcMaritalStatus, GreenType = GreenType.GreenD, Key = "V", Value = "Góa" },
             new DataConfig {Type = DataConfigType.LeadEcMaritalStatus, GreenType = GreenType.GreenD, Key = "M", Value = "Đã kết hôn" },
             new DataConfig {Type = DataConfigType.LeadEcMaritalStatus, GreenType = GreenType.GreenD, Key = "D", Value = "Ly hôn" },
             new DataConfig {Type = DataConfigType.LeadEcMaritalStatus, GreenType = GreenType.GreenD, Key = "C", Value = "Độc thân" },
             new DataConfig {Type = DataConfigType.LeadEcMaritalStatus, GreenType = GreenType.GreenD, Key = "CON", Value = "Sống chung" },
        };
    }
}

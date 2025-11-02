using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class MCTrustingSocialMapping
    {
        public static readonly ReadOnlyDictionary<string, string> MESSAGE =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                { "Lay diem thanh cong tu Portal", "Lấy điểm có sẵn từ MC" },
                { "Cham diem thanh cong tu doi tac TS", "Lấy điểm thành công từ Portal" },
                { "otp is not correct", "Mã OTP không hợp lệ" },
                { "the provided MSISDN is not found", "Số điện thoại không tìm thấy" },
                { "verify count limit reached", "Quá số lần gửi OTP" },
                { "error when finding otp sms request", "Lỗi khi gửi tin nhắn OTP đến khách hàng" },
                { "error when updating verify count and status", "Lỗi khi cập nhập xác minh và trạng thái" },
                { "msisdn_not_found", "Không tìm thấy số điện thoại" },
                { "credit score doesn't exist", "Điểm tín dụng không tồn tại" },
                { "no user consent", "Thất bại" },
                { "error when encrypting from client to telco", "Thất bại" },
                { "error when finding credit score", "Thất bại" },
                { "error when encrypting id number", "Thất bại" },
                { "error when encrypting credit score", "Thất bại" },
                { "the provided MSISDN is invalid", "Số điện thoại KH cung cấp không hợp lệ" },
                { "id_type and id_value aren't consistent", "Thất bại" },
                { "id number is invalid", "Thất bại" },
                { "Invalid phone number", "Số điện thoại không hợp lệ" },
                { "Not supported telecom", "Không hỗ trợ nhà mạng" },
                { "Duplicate request", "Yêu cầu bị trùng" },
                { "Invalid OTP", "Mã OTP không hợp lệ" },
                { "Internal Server Error", "Lỗi server" },
                { "Chi tiết tham khảo sheet Others", "Lỗi khác" }
            });
    }
}

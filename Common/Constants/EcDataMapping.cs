using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class EcDataMapping
    {
        public static readonly ReadOnlyDictionary<string, string> STATUS =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                { ECReturnUpdateStatus.REJECTED, CustomerStatus.REJECT },
                { ECReturnUpdateStatus.VALIDATED, CustomerStatus.CHOOSING },
                { ECReturnUpdateStatus.APPROVED, CustomerStatus.PROCESSING },
                { ECReturnUpdateStatus.SIGNED, CustomerStatus.PROCESSING },
                { ECReturnUpdateStatus.ACTIVATED, CustomerStatus.SUCCESS },
                { ECReturnUpdateStatus.TERMINATED, CustomerStatus.CANCEL },
                { ECReturnUpdateStatus.CANCELED, CustomerStatus.CANCEL },
                { ECReturnUpdateStatus.SYSTEM_ERROR, CustomerStatus.CANCEL },
                { ECReturnUpdateStatus.NOT_ELIGIBLE, CustomerStatus.REJECT },
                { ECReturnUpdateStatus.FAIL_EKYC, CustomerStatus.REJECT },
                { ECReturnUpdateStatus.FAIL_MANUAL_KYC, CustomerStatus.REJECT },
                { ECReturnUpdateStatus.NOT_SUITABLE_OFFER, CustomerStatus.REJECT },
                { ECReturnUpdateStatus.DUPLICATED, CustomerStatus.REJECT }
            });

        public static readonly ReadOnlyDictionary<string, string> REASON =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                { "Age (out of range 20-60)", "Không đạt điều kiện" },
                { "Income (< 4.5 MVND)", "Không đạt điều kiện" },
                { "Financial capability", "Không đạt điều kiện" },
                { "Not meet policy requirement", "Không đạt điều kiện" },
                { "Loan application has been validated with offers.", "Đang chờ chọn khoản vay" },
                { "Loan application has been approved.", "Đơn vay đã được chấp nhận" },
                { "Loan application has been signed.", "Đơn vay đã được ký" },
                { "Loan application has been activated.", "Đơn vay đã được giải ngân" },
                { "Loan application has been terminated.", "Đơn vay đã tất toán" },
                { "Loan application has been canceled.", "Đơn vay bị hủy" },
                { "System error.", "Lỗi hệ thống" },
                { "Fail initial update", "Không đạt điều kiện Eligible" },
                { "Not meet requirement", "Không đạt yêu cầu" },
                { "Wrong full name", "Sai tên" },
                { "Wrong DOB", "Sai ngày tháng năn sinh" },
                { "Wrong ID number (totally)", "Sai CMND" },
                { "Wrong ID number (1-2 numbers), suspect different ID number", "Sai, khác CMND" },
                { "Wrong ID number (more than 2 numbers, not totally)", "Sai hơn 2 số CMND" },
                { "Exceeded characters", "Vượt quá ký tự" },
                { "Expired DOI", "Hết hạn DOI" },
                { "Military ID / Passport / Wrong Format", "sai CMND quân đội/passport" },
                { "Missing selfie photo or NID/PID front side/ back side", "Mất mặt trước/mặt sau" },
                { "All photos and NID/PID missing", "Mất cả mặt trước và mặt sau" },
                { "Blurred information/ photo in NID/PID", "Mờ thông tin" },
                { "Selfie photo is blurred/ not clearly/ covered", "Hình chụp khách hàng bị mờ" },
                { "Disqualified selfie photo", "Hình chụp khách hàng ko đủ chất lượng" },
                { "Wrong order of photos", "Sai thứ tự hình ảnh" },
                { "Multi Mistakes", "Sai nhiều lỗi" },
                { "Incorrect current address", "Sai địa chỉ hiện tại" },
                { "Uncontacted primary phone", "Sai số điện thọai" },
                { "Refuse to confirm", "Từ chối xác nhận" },
                { "Incorrect DOI", "Sai DOI" },
                { "Invalid ID/PID", "CMND hết hạn" },
                { "ID taken from other device", "CMND chụp từ thiết bị khác" },
                { "Phone number duplication", "Trùng SĐT" },
                { "Not suitable offer", "Không có offer phù hợp" },
                { "The application is duplicated", "Đơn vay bị trùng" }
            });

        public static readonly ReadOnlyDictionary<string, string> REJECT_REASON =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                { "REJECTED-REJ_AGE", "Độ tuổi không hợp lệ (độ tuổi hợp lệ 20 - 60)" },
                { "REJECTED-REJ_INCOME", "Thu nhập không hợp lệ (< 4.5 tr VND)" },
                { "REJECTED-REJ_FINCAP", "Vượt quá khả năng tài chính" },
                { "REJECTED-REJ_POLICY", "Không thỏa chính sách cho vay tại EC" },
                { "VALIDATED-", "Chọn số tiền đề nghị vay" },
                { "APPROVED-", "Khoản vay đã được duyệt" },
                { "SIGNED-", "Đã ký hợp đồng" },
                { "ACTIVATED-", "Đã chấp thuận giải ngân" },
                { "TERMINATED-", "Chấm dứt khoản vay " },
                { "CANCELED-", "Khoản vay bị huỷ" },
                { "SYSTEM ERROR-", "Lỗi hệ thống" },
                { "NOT_ELIGIBLE-NOT_ELIGIBLE", "Hồ sơ không đạt" },
                { "FAIL_EKYC-FAIL_EKYC", "Không đạt định danh điện tử (eKYC)" },
                { "FAIL_MANUAL_KYC-KYC_RJPOLICY", "Không thỏa chính sách cho vay tại EC" },
                { "FAIL_MANUAL_KYC-CKYC-WRI01", "Sai tên" },
                { "FAIL_MANUAL_KYC-CKYC-WRI02", "Sai ngày tháng năm sinh" },
                { "FAIL_MANUAL_KYC-CKYC-WRI03", "Sai số CMND" },
                { "FAIL_MANUAL_KYC-CKYC-WRI04", "Sai số CMND" },
                { "FAIL_MANUAL_KYC-CKYC-WRI05", "Sai số CMND" },
                { "FAIL_MANUAL_KYC-CKYC-MIS01", "Vượt quá số ký tự cho phép" },
                { "FAIL_MANUAL_KYC-CKYC-EXP01", "CMND quá hạn" },
                { "FAIL_MANUAL_KYC-CKYC-WRI06", "CMND sai định dạng, không phải CMND theo quy định (CM quân đội, hộ chiếu, thẻ quân nhân,…)" },
                { "FAIL_MANUAL_KYC-CKYC-MID01", "Thiếu hình mặt trước/sau CMND, selfie" },
                { "FAIL_MANUAL_KYC-CKYC-MID02", "Thiếu toàn bộ hình" },
                { "FAIL_MANUAL_KYC-CKYC-QUD01", "CMND bị mờ" },
                { "FAIL_MANUAL_KYC-CKYC-QUD02", "Hình selfie bị mờ, không rõ ràng, chói lóa, có vật cản,.." },
                { "FAIL_MANUAL_KYC-CKYC-WRD01", "Hình chụp KH không hợp lệ (nghiêng mặt, cúi mặt, sử dụng phần mềm chỉnh sửa, khoảng cách chụp quá xa hoặc không phải ảnh chân dung…)" },
                { "FAIL_MANUAL_KYC-CKYC-WRD03", "Sai thứ tự hình ảnh" },
                { "FAIL_MANUAL_KYC-CKYC-MM001", "Yêu cầu kiểm lại toàn bộ thông tin KH" },
                { "FAIL_MANUAL_KYC-ADD-CURR01", "Sai địa chỉ KH" },
                { "FAIL_MANUAL_KYC-CNTT-PRIM1", "Không liên hệ được KH để xác nhận thông tin" },
                { "FAIL_MANUAL_KYC-CNTT-RFTCF", "KH không đồng ý xác nhận lại thông tin" },
                { "FAIL_MANUAL_KYC-CKYC-DOI01", "Sai ngày cấp CMND/CCCD" },
                { "FAIL_MANUAL_KYC-CKYC-IVLID", "Hình CMND/CCCD vô hiệu (vd mất góc, cắt góc, đục lỗ, CMND photo, in màu…)" },
                { "FAIL_MANUAL_KYC-CKYC-OTDEV", "Hình CMND/CCCD chụp qua thiết bị khác, không chụp trực tiếp, …" },
                { "FAIL_MANUAL_KYC-CKYC-PHONE", "Trùng số điện thoại (sdt KH, người thân trùng nhau)" },
                { "FAIL_MANUAL_KYC-REJ_POLICY, ", "Không thỏa chính sách cho vay tại EC" },
                { "NOT_SUITABLE_OFFER-NOT_SUITABLE_OFFER", "Số tiền đề nghị vay không hợp lệ" },
                { "DUPLICATED-DUPLICATED", "Đã có hồ sơ vay đang xử lý" }
            });
    }
}

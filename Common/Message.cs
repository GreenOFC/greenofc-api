using System;

namespace _24hplusdotnetcore.Common
{
    public static class Message
    {
        public static string SUCCESS = "";
        public static string LOGIN_SUCCESS = "";
        public static string LOGIN_INCORRECT = "Sai {0}, Vui lòng nhập lại!";
        public static string INCORRECT_USERNAME_PASSWORD = "Sai tài khoản hoặc mật khẩu, Vui lòng nhập lại!";
        public static string LOGIN_BIDDEN = "Không thể login, Vui lòng liên hệ IT!";
        public static string LOGIN_STATUS_LOCKED = "Tài khoản đã bị khóa, Vui lòng liên hệ IT!";
        public static string IS_LOGGED_IN_ORTHER_DEVICE = "Bạn đã đăng nhập ở một nơi khác, Vui lòng đăng nhập lại!";
        public static string UNAUTHORIZED = "Bạn không có quyền!";
        public static string REQUIRED = "{0} là bắt buộc";
        public static string ERROR = "Lỗi hệ thống, Vui lòng liên hệ IT!";
        public static string NOT_FOUND_PRODUCT = "Không tìm thấy sản phẩm!";
        internal static string VERSION_IS_OLD = "Phiên bản của bạn đã cũ, Vui lòng cập nhập phiên bản mới!";
        internal static string NotificationAdd = "{0} vừa thêm mới khách hàng {1}";
        internal static string NotificationUpdate = "{0} vừa cập nhật lại thông tin khách hàng  {1}";
        public static string NotificationReject = "{0} vừa từ chối hồ sơ khách hàng {1}";
        public static string NotificationCancel = "{0} vừa hủy hồ sơ khách hàng {1}";
        public static string NotificationSuccess = "{0} vừa hoàn thành hồ sơ khách hàng {1}";
        public static string NotificationReturn = "{0} vừa trả thông tin hồ sơ khách hàng {1}";
        public static string TeamLeadApprove = "{0} vừa duyệt thành công hồ sơ khách hàng {1}";
        public const string NOT_FOUND_KIOS = "Không tìm thấy kios";
        public static string CANT_UPDATE_CUSTOMER_ERROR = "Lỗi hệ thống, Vui lòng thử lại!";
        public const string CUSTOMER_NOT_FOUND = "Không tìm thấy hồ sơ!";
        public const string INCORRECT_VERSION = "Phiên bản app đã cũ, vui lòng tải bản app mới!";
        public const string EXISTED_CUSTOMER = "Số CMND đã lên hồ sơ trên hệ thống, Vui lòng kiểm tra lại";
        public const string NOTIFICATION_TICKET_ADMIN_TEMPLATE = "Bạn nhận được Yêu cầu hỗ trợ {0}";
        public const string NOTIFICATION_TICKET_SALES_TEMPLATE = "Có phản hồi/cập nhật trạng thái trên Yêu cầu hỗ trợ {0}";
        public const string NOTIFICATION_TICKET_TITLE = "Yêu cầu hỗ trợ";
        public const string ID_CARD_INVALID = "Số CMND/CCCD không hợp lệ";
        public const string HAS_NO_DATA = "File không có dữ liệu";
        public const string SHEET_NOT_FOUND = "Không tìm thấy sheet {0}";

        // User
        public const string USER_NOT_FOUND = "Không tìm thấy user";
        public const string USER_REFERENCE_NOT_FOUND = "Không tìm thấy thông tin người giới thiệu  - {0}";
        public const string USER_PASSWORD_DOES_NOT_MATCH = "Mật khẩu cũ không chính xác!";
        public const string USER_EXISTED = "{0} đã tồn tại với tài khoản {1}";

        // F88
        public const string F88_CREATE_DUPLICATE = "Số CMND đã bị trùng";
        public const string F88_NOT_FOUND = "Không tìm thấy f88";

        // Status
        public const string INCORRECT_STATUS = "Trạng thái hồ sơ không đúng";

        //Role
        public const string ROLE_NOT_FOUND = "Không tìm thấy role";

        // Common Error
        public const string COMMON_NOT_FOUND = "Không tìm thấy {0}";
        public const string COMMON_EXISTED = "{0} đã tồn tại";
        public const string COMMON_PERMISSION = "Bạn không có quyền!";

        public const string LEAD_VIB_NOT_FOUND = "Không tìm thấy lead vib";

        public const string MC_DEBT_NOT_FOUND = "Không tìm thấy mc debt";
        public const string MC_APP_NUMBER_EXISTED = "AppNumber đã tồn tại!";

        // Ticket Error
        public const string TICKET_NOT_ASSIGN = "Ticket này không được assign cho bạn!";
        public const string TICKET_NOT_SUBMIT = "Không thể thay đổi trạng thái của ticket chưa được gửi đi!";
        public const string TICKET_UPDATE_NO_PERMISSION = "Bạn không có quyền chỉnh sửa ticket này!";
        public const string CANNOT_UPDATE = "Không thể chỉnh sửa!";
        public const string CANNOT_CREATE = "Không thể tạo ticket!";
        public const string WRONG_STATUS = "Sai trạng thái";

        // Comment Ticket Error
        public const string TICKET_STATUS_IS_DRAFT = "Ticket đang ở trạng thái nháp!";
        public const string CANNOT_ADD_COMMENT = "Không thể thêm comment!";
        public const string COMMENT_UPDATE_NO_PERMISSION = "Bạn không có quyền chỉnh sửa comment này!";
        public const string COMMENT_DELETE_NO_PERMISSION = "Bạn không có quyền xóa comment này!";
        public const string COMMON_REQUIRED = "Trường {0} là bắt buộc";
        public const string VALIDATTE_EERROR_REQUIRED = "Vui lòng bổ sung những trường bắt buộc sau:";

        // pos
        public const string POS_NOT_FOUND = "Không tìm thấy pos";
        public const string POS_EXISTED_SALE_CHANEL = "DVBH đã tồn tại ở 1 Kênh bán khác";

        //PTF
        public const string PTF_CANCEL_LOAN_APPLICATION_WRONG_STATUS = "Hồ sơ đang ở trạng thái {0}, không thể hủy!";

    }
    public enum ResponseCode : int
    {
        SUCCESS = 1,
        ERROR,
        UNAUTHORIZED,
        IS_LOGGED_IN_ORTHER_DEVICE
    }

    public static class ResponseStatus
    {
        public static string SUCCESS = "SUCCESS";
        public static string ERROR = "ERROR";
    }
    public static class CustomerErrorMessage
    {
        public const string NOT_TEAM_LEAD = "Bạn không phải Teamlead của hồ sơ này!";
    }
    public static class TransactionErrorMessage
    {
        public const string WRONG_STATUS = "Trạng thái giao dịch không hợp lệ!";
        public const string NOT_OWNER = "Bạn không có quyền sử dụng giao dịch này!";
    }
}
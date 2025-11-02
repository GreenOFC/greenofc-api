using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class PermissionSeedData
    {
        public static IReadOnlyList<Permission> Entities = new Permission[]
        {
            // Menu
            // Menu Ticket
            new Permission{ Name = "Yêu cầu hỗ trợ - Báo cáo ", Value = PermissionMenuCost.TicketReport, Group = "Menu" },
            // Menu Management
            new Permission{ Name = "Quản lý", Value = PermissionMenuCost.Management, Group = "Menu" },
            new Permission{ Name = "Quản lý - Nhân viên", Value = PermissionMenuCost.ManagementUser, Group = "Menu" },
            new Permission{ Name = "Quản lý - Nhân viên - Thêm mới", Value = PermissionMenuCost.ManagementUserAdd, Group = "Menu" },
            new Permission{ Name = "Quản lý - Nhân viên - Thêm mới (nhiều)", Value = PermissionMenuCost.ManagementUserAddMulti, Group = "Menu" },
            new Permission{ Name = "Quản lý - Nhân viên - Tất cả nhân viên", Value = PermissionMenuCost.ManagementUserGetList, Group = "Menu" },
            new Permission{ Name = "Quản lý - Nhân viên - Quản lý khóa code", Value = PermissionMenuCost.ManagementUserLockedCode, Group = "Menu" },
            new Permission{ Name = "Quản lý - Nhân viên - Nhân viên chờ duyệt", Value = PermissionMenuCost.ManagementUserWaiting, Group = "Menu" },
            new Permission{ Name = "Quản lý - Nhân viên - Quản lý phân quyền", Value = PermissionMenuCost.ManagementUserRole, Group = "Menu" },

            new Permission{ Name = "Quản lý - DVBH", Value = PermissionMenuCost.ManagementPos, Group = "Menu" },
            new Permission{ Name = "Quản lý - DVBH - Quản lý DVBH", Value = PermissionMenuCost.ManagementPosGetList, Group = "Menu" },

            new Permission{ Name = "Quản lý - Kênh bán", Value = PermissionMenuCost.ManagementSaleChanel, Group = "Menu" },
            new Permission{ Name = "Quản lý - Kênh bán - Quản lý kênh bán", Value = PermissionMenuCost.ManagementSaleChanelGetList, Group = "Menu" },
            new Permission{ Name = "Quản lý - Kênh bán - Quản lý đẩy hồ sơ PTF", Value = PermissionMenuCost.ManagementSaleChanelConfigUserPtf, Group = "Menu" },

            new Permission{ Name = "Thiết lập", Value = PermissionMenuCost.Setting, Group = "Menu" },
            new Permission{ Name = "Thiết lập - Tin tức", Value = PermissionMenuCost.SettingNews, Group = "Menu" },
            new Permission{ Name = "Thiết lập - Banner", Value = PermissionMenuCost.SettingBanner, Group = "Menu" },
            new Permission{ Name = "Thiết lập - Role", Value = PermissionMenuCost.SettingRole, Group = "Menu" },
            new Permission{ Name = "Thiết lập - Quản lý Notification", Value = PermissionMenuCost.SettingGroupNoti, Group = "Menu" },

            new Permission{ Name = "Tools", Value = PermissionMenuCost.Tools, Group = "Menu" },
            new Permission{ Name = "Tools - Kiểm tra CMND", Value = PermissionMenuCost.ToolsCheckIdCard, Group = "Menu" },
            new Permission{ Name = "Tools - Báo cáo hồ sơ", Value = PermissionMenuCost.ToolsProjectProfileReport, Group = "Menu" },

            new Permission{ Name = "Quản lý nợ", Value = PermissionMenuCost.Debt, Group = "Menu" },
            new Permission{ Name = "Quản lý nợ - Danh sách nợ", Value = PermissionMenuCost.DebtGetList, Group = "Menu" },
            new Permission{ Name = "Quản lý nợ - Upload danh sách", Value = PermissionMenuCost.DebtImport, Group = "Menu" },
            new Permission{ Name = "Quản lý nợ - Lịch sử Upload", Value = PermissionMenuCost.DebtImportHistory, Group = "Menu" },

            // new Permission{ Name = "Tín chấp - PTF", Value = PermissionMenuCost.ProjectPtf, Group = "Menu" },
            new Permission{ Name = "Tín chấp - PTF - Chấp điểm sim", Value = PermissionMenuCost.ProjectPtfCheckSim, Group = "Menu" },
            new Permission{ Name = "Tín chấp - PTF - Báo cáo chấm điểm sim", Value = PermissionMenuCost.ProjectPtfReportCheckSim, Group = "Menu" },
            new Permission{ Name = "Tín chấp - PTF - Chấp điểm income", Value = PermissionMenuCost.ProjectPtfCheckIncome, Group = "Menu" },
            new Permission{ Name = "Tín chấp - PTF - Báo cáo chấm điểm income", Value = PermissionMenuCost.ProjectPtfReportCheckIncome, Group = "Menu" },
            new Permission{ Name = "Tín chấp - MC - Báo cáo lịch sử gọi API", Value = PermissionMenuCost.ProjectMCReportHistoryApi, Group = "Menu" },
            
            new Permission{ Name = "Tín chấp - MAFC - Kiểm tra thông tin v3", Value = PermissionMenuCost.ProjectMafcCheckDup, Group = "Menu" },
            new Permission{ Name = "Tín chấp - MAFC - Báo cáo Kiểm tra thông tin v3", Value = PermissionMenuCost.ProjectMafcReportCheckDup, Group = "Menu" },

            // Common
            new Permission{ Name = "View All CIMB", Value = PermissionCost.ViewAllCimb, Group = "Common" },
            new Permission{ Name = "View All EC", Value = PermissionCost.ViewAllEc, Group = "Common" },
            new Permission{ Name = "View All F88", Value = PermissionCost.ViewAllF88, Group = "Common" },
            new Permission{ Name = "Process Ticket", Value = PermissionCost.ProcessTicket, Group = "Common" },
            new Permission{ Name = "View All MC", Value = PermissionCost.ViewAllMc, Group = "Common" },
            new Permission{ Name = "MC Check sim", Value = PermissionCost.MCCheckSim, Group = "Common" },
            new Permission{ Name = "MC Check Tool", Value = PermissionCost.MCCheckTool, Group = "Common" },
            new Permission{ Name = "MC Get All and Send Noti", Value = PermissionCost.MCSendStatus, Group = "Common" },
            new Permission{ Name = "MC View Check Sim Report", Value = PermissionCost.MCReportCheckSim, Group = "Common" },
            new Permission{ Name = "MC View Tool Precheck Report", Value = PermissionCost.MCReportToolPrecheck, Group = "Common" },
            new Permission{ Name = "View All VPS", Value = PermissionCost.ViewAllVps, Group = "Common" },
            new Permission{ Name = "View All MC - Trusting Social", Value = PermissionCost.ViewAllTrustingSocial, Group = "Common" },
            new Permission{ Name = "Resend New MC Data Processing Record", Value = PermissionCost.DataMCProsessingCreate, Group = "Common" },
            new Permission{ Name = "Customer Return Status", Value = PermissionCost.CustomerReturnStatus, Group = "Common" },
            new Permission{ Name = "Customer View History", Value = PermissionCost.CustomerViewHistory, Group = "Common" },

            // EC
            new Permission{ Name = "View All EC Data Prosessing", Value = PermissionCost.EcDataProsessingViewAll, Group = "EC" },
            new Permission{ Name = "View All EC Offer List", Value = PermissionCost.EcOfferListViewAll, Group = "EC" },
            new Permission{ Name = "Update EC record file", Value = PermissionCost.EcUpdateRecord, Group = "EC" },
            new Permission{ Name = "Update EC status", Value = PermissionCost.EcUpdateStatus, Group = "EC" },
            new Permission{ Name = "EC Get All and Send Offer", Value = PermissionCost.EcSendOffer, Group = "EC" },

            // F88
            new Permission{ Name = "F88 Get All Noti", Value = PermissionCost.F88ReportNoti, Group = "F88" },


            // Role
            new Permission{ Name = "Add role to user", Value = PermissionCost.AddUserRole, Group = "Role" },
            new Permission{ Name = "Remove role to user", Value = PermissionCost.RemoveUserRole, Group = "Role" },
            new Permission{ Name = "Create Role", Value = PermissionCost.RoleCreate, Group = "Role" },
            new Permission{ Name = "Update Role", Value = PermissionCost.RoleUpdate, Group = "Role" },
            new Permission{ Name = "Delete Role", Value = PermissionCost.RoleDelete, Group = "Role" },
            new Permission{ Name = "Get List Role", Value = PermissionCost.RoleGetList, Group = "Role" },
            new Permission{ Name = "Get Detail Role", Value = PermissionCost.RoleGet, Group = "Role" },
            new Permission{ Name = "Get List Users have Role", Value = PermissionCost.RoleGetBelongUser, Group = "Role" },

            // USER
            new Permission{ Name = "Review user profile", Value = PermissionCost.ReviewUserProfile, Group = "User" },
            new Permission{ Name = "Get All User", Value = PermissionCost.GetAllUserProfile, Group = "User" },
            new Permission{ Name = "Get Waiting User", Value = PermissionCost.GetWaitingUserProfile, Group = "User" },
            new Permission{ Name = "Get Suspended User", Value = PermissionCost.GetListSuspendedUserProfile, Group = "User" },
            new Permission{ Name = "Change Status User", Value = PermissionCost.UpdateUserStatus, Group = "User" },
            new Permission{ Name = "Reset User Password", Value = PermissionCost.ResetUesrPassword, Group = "User" },
            new Permission{ Name = "Download All User", Value = PermissionCost.DownloadAllUserProfile, Group = "User" },
            new Permission{ Name = "User Create", Value = PermissionCost.UserCreate, Group = "User" },
            new Permission{ Name = "User Create Multi", Value = PermissionCost.UserCreateMulti, Group = "User" },
            new Permission{ Name = "User View History", Value = PermissionCost.UserViewHistory, Group = "User" },

            // MAFC
            new Permission{ Name = "View All MAFC", Value = PermissionCost.ViewAllMafc, Group = "MAFC" },
            new Permission{ Name = "Update record file MAFC", Value = PermissionCost.MafcUpdateRecord, Group = "MAFC" },
            new Permission{ Name = "Generate DN file MAFC", Value = PermissionCost.MafcGenerateDNFile, Group = "MAFC" },
            new Permission{ Name = "MAFC Get Payload", Value = PermissionCost.MafcGetPayload, Group = "MAFC" },
            new Permission{ Name = "MAFC Get All and Send Defer", Value = PermissionCost.MafcSendDefer, Group = "MAFC" },
            new Permission{ Name = "MAFC Get All and Send Status", Value = PermissionCost.MafcSendStatus, Group = "MAFC" },
            new Permission{ Name = "API - MAFC - Check Dup V3", Value = PermissionCost.MafcCheckDupV3, Group = "MAFC" },

            new Permission{ Name = "Admin", Value = PermissionCost.AdminPermission.Admin, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadEc List", Value = PermissionCost.AdminPermission.Admin_LeadEcManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadVps List", Value = PermissionCost.AdminPermission.Admin_LeadVpsManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadF88 List", Value = PermissionCost.AdminPermission.Admin_LeadF88Management_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadVibs List", Value = PermissionCost.AdminPermission.Admin_LeadVibsManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Pos List", Value = PermissionCost.AdminPermission.Admin_PosManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get MC Debt List", Value = PermissionCost.AdminPermission.Admin_LeadMcDebtManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get History List", Value = PermissionCost.AdminPermission.Admin_HistoryManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadMafc List", Value = PermissionCost.AdminPermission.Admin_LeadMafcManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadMc List", Value = PermissionCost.AdminPermission.Admin_LeadMcManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadCimb List", Value = PermissionCost.AdminPermission.Admin_LeadCimbManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Shinhan List", Value = PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadHome List", Value = PermissionCost.AdminPermission.Admin_LeadHomeManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadOkVay List", Value = PermissionCost.AdminPermission.Admin_LeadOkVayManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadVBI List", Value = PermissionCost.AdminPermission.Admin_LeadVbiManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get LeadPTF List", Value = PermissionCost.AdminPermission.Admin_LeadPtfManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Lấy danh sách Hyper Lead", Value = PermissionCost.AdminPermission.HyperLead_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Ticket List", Value = PermissionCost.AdminPermission.Admin_Ticket_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get User List", Value = PermissionCost.AdminPermission.Admin_User_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Transaction List", Value = PermissionCost.AdminPermission.Admin_Transaction_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Transaction Ipn List", Value = PermissionCost.AdminPermission.Admin_Transaction_Ipn_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Check Customer List", Value = PermissionCost.AdminPermission.Admin_CheckCustomer_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Báo cáo hồ sơ dự án", Value = PermissionCost.AdminPermission.Admin_ProjectProfileReport_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Debt Management List", Value = PermissionCost.AdminPermission.Admin_DebtManagement_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Get Import File List", Value = PermissionCost.AdminPermission.Admin_ImportFile_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Báo cáo lịch sử gọi API", Value = PermissionCost.AdminPermission.LogApi_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Báo cáo lịch sử Check sim MC", Value = PermissionCost.AdminPermission.CheckSim_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Báo cáo lịch sử Check sim PTF", Value = PermissionCost.AdminPermission.CheckSim_ViewAll, Group = "Admin" },
            new Permission{ Name = "Admin - Báo cáo lịch sử Check income PTF", Value = PermissionCost.AdminPermission.CheckSim_ViewAll, Group = "Admin" },


            new Permission{ Name = "Head of Sale Admin", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadEc List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadEcManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadVps List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadVpsManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadVib List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadVibsManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadF88 List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadF88Management_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get Pos List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_PosManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get MC Debt List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMcDebtManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get History List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_HistoryManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadMafc List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMafcManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadMc List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMcManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadCimb List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadCimbManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get Shinhan List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadShinhanManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadHome List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadHomeManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadOkVay List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadOkVayManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadVBI List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadVbiManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get LeadPTF List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadPtfManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Lấy danh sách Hyper Lead", Value = PermissionCost.HeadOfSaleAdminPermission.HyperLead_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get Ticket List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_Ticket_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get User List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_User_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get Transaction List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_Transaction_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get Check Customer List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_CheckCustomer_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get Debt Management List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_DebtManagement_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Get Import File List", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_ImportFile_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Báo cáo hồ sơ dự án", Value = PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_ProjectProfileReport_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Báo cáo lịch sử gọi API", Value = PermissionCost.HeadOfSaleAdminPermission.LogApi_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Báo cáo lịch sử Check sim MC", Value = PermissionCost.HeadOfSaleAdminPermission.CheckSim_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Báo cáo lịch sử Check sim PTF", Value = PermissionCost.HeadOfSaleAdminPermission.CheckSim_ViewAll, Group = "HeadOfSaleAdmin" },
            new Permission{ Name = "Head of Sale Admin - Báo cáo lịch sử Check income PTF", Value = PermissionCost.HeadOfSaleAdminPermission.CheckSim_ViewAll, Group = "HeadOfSaleAdmin" },

            new Permission{ Name = "Pos Lead", Value = PermissionCost.PosLeadPermission.PosLead, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadEc List", Value = PermissionCost.PosLeadPermission.PosLead_LeadEcManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadVps List", Value = PermissionCost.PosLeadPermission.PosLead_LeadVpsManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadVib List", Value = PermissionCost.PosLeadPermission.PosLead_LeadVibsManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadF88 List", Value = PermissionCost.PosLeadPermission.PosLead_LeadF88Management_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get Pos List", Value = PermissionCost.PosLeadPermission.PosLead_PosManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get MC Debt List", Value = PermissionCost.PosLeadPermission.PosLead_LeadMcDebtManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get History List", Value = PermissionCost.PosLeadPermission.PosLead_HistoryManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadMafc List", Value = PermissionCost.PosLeadPermission.PosLead_LeadMafcManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadMc List", Value = PermissionCost.PosLeadPermission.PosLead_LeadMcManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadCimb List", Value = PermissionCost.PosLeadPermission.PosLead_LeadCimbManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get Shinhan List", Value = PermissionCost.PosLeadPermission.PosLead_LeadShinhanManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadHome List", Value = PermissionCost.PosLeadPermission.PosLead_LeadHomeManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadOkVay List", Value = PermissionCost.PosLeadPermission.PosLead_LeadOkVayManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadVBI List", Value = PermissionCost.PosLeadPermission.PosLead_LeadVbiManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get LeadPTF List", Value = PermissionCost.PosLeadPermission.PosLead_LeadPtfManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Lấy danh sách Hyper Lead", Value = PermissionCost.PosLeadPermission.HyperLead_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get Ticket List", Value = PermissionCost.PosLeadPermission.PosLead_Ticket_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get User List", Value = PermissionCost.PosLeadPermission.PosLead_User_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get Transaction List", Value = PermissionCost.PosLeadPermission.PosLead_Transaction_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get Check Customer List", Value = PermissionCost.PosLeadPermission.PosLead_CheckCustomer_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get Debt Management List", Value = PermissionCost.PosLeadPermission.PosLead_DebtManagement_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Get Import File List", Value = PermissionCost.PosLeadPermission.PosLead_ImportFile_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Báo cáo hồ sơ dự án", Value = PermissionCost.PosLeadPermission.PosLead_ProjectProfileReport_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Báo cáo lịch sử gọi API", Value = PermissionCost.PosLeadPermission.LogApi_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Báo cáo lịch sử check sim MC", Value = PermissionCost.PosLeadPermission.CheckSim_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Báo cáo lịch sử check sim PTF", Value = PermissionCost.PosLeadPermission.CheckSim_ViewAll, Group = "PosLead" },
            new Permission{ Name = "Pos Lead - Báo cáo lịch sử check income PTF", Value = PermissionCost.PosLeadPermission.CheckSim_ViewAll, Group = "PosLead" },

            new Permission{ Name = "Asm", Value = PermissionCost.AsmPermission.Asm, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadEc List", Value = PermissionCost.AsmPermission.Asm_LeadEcManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadVps List", Value = PermissionCost.AsmPermission.Asm_LeadVpsManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadF88 List", Value = PermissionCost.AsmPermission.Asm_LeadF88Management_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadVibs List", Value = PermissionCost.AsmPermission.Asm_LeadVibsManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get Pos List", Value = PermissionCost.AsmPermission.Asm_PosManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get MC Debt List", Value = PermissionCost.AsmPermission.Asm_LeadMcDebtManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get History List", Value = PermissionCost.AsmPermission.Asm_HistoryManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadMafc List", Value = PermissionCost.AsmPermission.Asm_LeadMafcManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadMc List", Value = PermissionCost.AsmPermission.Asm_LeadMcManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadCimb List", Value = PermissionCost.AsmPermission.Asm_LeadCimbManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get Shinhan List", Value = PermissionCost.AsmPermission.Asm_LeadShinhanManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadHome List", Value = PermissionCost.AsmPermission.Asm_LeadHomeManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadOkVay List", Value = PermissionCost.AsmPermission.Asm_LeadOkVayManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadVBI List", Value = PermissionCost.AsmPermission.Asm_LeadVbiManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get LeadPTF List", Value = PermissionCost.AsmPermission.Asm_LeadPtfManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Lấy danh sách Hyper Lead", Value = PermissionCost.AsmPermission.HyperLead_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get Ticket List", Value = PermissionCost.AsmPermission.Asm_Ticket_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get User List", Value = PermissionCost.AsmPermission.Asm_User_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get Transaction List", Value = PermissionCost.AsmPermission.Asm_Transaction_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get Check Customer List", Value = PermissionCost.AsmPermission.Asm_CheckCustomer_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Báo cáo hồ sơ dự án", Value = PermissionCost.AsmPermission.Asm_ProjectProfileReport_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get Debt Management List", Value = PermissionCost.AsmPermission.Asm_DebtManagement_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Get Import File List", Value = PermissionCost.AsmPermission.Asm_ImportFile_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Báo cáo lịch sử gọi API", Value = PermissionCost.AsmPermission.LogApi_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Báo cáo lịch sử check sim MC", Value = PermissionCost.AsmPermission.CheckSim_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Báo cáo lịch sử check sim PTF", Value = PermissionCost.AsmPermission.CheckSim_ViewAll, Group = "Asm" },
            new Permission{ Name = "Asm - Báo cáo lịch sử check income PTF", Value = PermissionCost.AsmPermission.CheckSim_ViewAll, Group = "Asm" },

            new Permission{ Name = "Team Leader", Value = PermissionCost.TeamLeaderPermission.TeamLeader, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadEc List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadEcManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadVps List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadVpsManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadF88 List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadF88Management_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadVibs List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadVibsManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get Pos List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_PosManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get MC Debt List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadMcDebtManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get History List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_HistoryManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadMafc List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadMafcManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadMc List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadMcManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadCimb List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadCimbManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get Shinhan List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadShinhanManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadHome List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadHomeManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadOkVay List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadOkVayManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadVBI List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadVbiManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get LeadPTF List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_LeadPtfManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Lấy danh sách Hyper Lead", Value = PermissionCost.TeamLeaderPermission.HyperLead_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get Ticket List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_Ticket_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get User List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_User_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get Transaction List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_Transaction_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get Check Customer List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_CheckCustomer_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Báo cáo hồ sơ dự án", Value = PermissionCost.TeamLeaderPermission.TeamLeader_ProjectProfileReport_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get Debt Management List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_DebtManagement_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Get Import File List", Value = PermissionCost.TeamLeaderPermission.TeamLeader_ImportFile_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Báo cáo lịch sử gọi API", Value = PermissionCost.TeamLeaderPermission.LogApi_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Báo cáo lịch sử check sim MC", Value = PermissionCost.TeamLeaderPermission.CheckSim_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Báo cáo lịch sử check sim PTF", Value = PermissionCost.TeamLeaderPermission.CheckSim_ViewAll, Group = "TeamLeader" },
            new Permission{ Name = "Team Leader - Báo cáo lịch sử check income PTF", Value = PermissionCost.TeamLeaderPermission.CheckSim_ViewAll, Group = "TeamLeader" },

            // POS
            new Permission{ Name = "Create Pos", Value = PermissionCost.PosCreate, Group = "Pos" },
            new Permission{ Name = "Update Pos", Value = PermissionCost.PosUpdate, Group = "Pos" },
            new Permission{ Name = "Delete Pos", Value = PermissionCost.PosDelete, Group = "Pos" },
            new Permission{ Name = "Get Pos", Value = PermissionCost.PosGet, Group = "Pos" },
            // Banner
            new Permission{ Name = "Create Banner", Value = PermissionCost.BannerCreate, Group = "Banner" },
            new Permission{ Name = "Update Banner", Value = PermissionCost.BannerUpdate, Group = "Banner" },
            new Permission{ Name = "Delete Banner", Value = PermissionCost.BannerDelete, Group = "Banner" },

            // News
            new Permission{ Name = "Create News", Value = PermissionCost.NewsCreate, Group = "News" },
            new Permission{ Name = "Update News", Value = PermissionCost.NewsUpdate, Group = "News" },
            new Permission{ Name = "Delete News", Value = PermissionCost.NewsDelete, Group = "News" },

            // Group Noti
            new Permission{ Name = "Create Group Noti", Value = PermissionCost.GroupNotiCreate, Group = "Notification" },
            new Permission{ Name = "Update Group Noti", Value = PermissionCost.GroupNotiUpdate, Group = "Notification" },
            new Permission{ Name = "Delete Group Noti", Value = PermissionCost.GroupNotiDelete, Group = "Notification" },
            new Permission{ Name = "Get Group Noti", Value = PermissionCost.GroupNotiGet, Group = "Notification" },

            new Permission{ Name = "Approve LeadOkVay", Value = PermissionCost.LeadOkVayApprove, Group = "LeadOkVay" },
            new Permission{ Name = "Reject LeadOkVay", Value = PermissionCost.LeadOkVayReject, Group = "LeadOkVay" },

            // PTF
            new Permission{ Name = "PTF Get Payload", Value = PermissionCost.PtfOmniGetPayload, Group = "PTF" },
            new Permission{ Name = "PTF Admin Edit", Value = PermissionCost.PtfOmniEditCustomer, Group = "PTF" },
            new Permission{ Name = "PTF Sync Loan", Value = PermissionCost.PtfOmniSyncLoan, Group = "PTF" },
            new Permission{ Name = "PTF Sync All Loan", Value = PermissionCost.PtfOmniSyncAllLoan, Group = "PTF" },

            //Sale Chanel
             new Permission{ Name = "Sale Chanel Get", Value = PermissionCost.SaleChanelGet, Group = "Sale Chanel" },
             new Permission{ Name = "Sale Chanel Create", Value = PermissionCost.SaleChanelCreate, Group = "Sale Chanel" },
             new Permission{ Name = "Sale Chanel Delete", Value = PermissionCost.SaleChanelDelete, Group = "Sale Chanel" },
             new Permission{ Name = "Đồng bộ user của kênh bán", Value = PermissionCost.SaleChanelSync, Group = "Sale Chanel" },

            //Sale Chanel Config User
            new Permission{ Name = "Sale Chanel Config User Get", Value = PermissionCost.SaleChanelConfigUserGet, Group = "Sale Chanel Config User" },
            new Permission{ Name = "Sale Chanel Config User Create", Value = PermissionCost.SaleChanelConfigUserCreate, Group = "Sale Chanel Config User" },
            new Permission{ Name = "Sale Chanel Config User Update", Value = PermissionCost.SaleChanelConfigUserUpdate, Group = "Sale Chanel Config User" },
            new Permission{ Name = "Sale Chanel Config User Delete", Value = PermissionCost.SaleChanelConfigUserDelete, Group = "Sale Chanel Config User" },

            new Permission{ Name = "Check Sim MC V2", Value = PermissionCost.CheckSimMcV2, Group = "Check Sim" },
            new Permission{ Name = "Get Check Sim", Value = PermissionCost.CheckSimGet, Group = "Check Sim" },
            new Permission{ Name = "Export Check Sim", Value = PermissionCost.CheckSimExport, Group = "Check Sim" },
            new Permission{ Name = "PTF Check Sim", Value = PermissionCost.CheckSimPtf, Group = "Check Sim" },

            // Transaction
            new Permission{ Name = "Transaction Refund", Value = PermissionCost.TransactionPermission.Transaction_Refund, Group = "Transaction" },
            new Permission{ Name = "Transaction View History", Value = PermissionCost.TransactionPermission.TransactionViewHistory, Group = "Transaction" },

            //Ticket
            new Permission{ Name = "Ticket Report", Value = PermissionCost.TicketReport, Group = "Ticket" },

            // DebtManagement
            new Permission{ Name = "Debt Management Create", Value = PermissionCost.DebtManagementCreate, Group = "Debt Management" },

            //ProjectProfileReport
            new Permission{ Name = "Import Báo cáo hồ sơ dự án", Value = PermissionCost.ProjectProfileReportImport, Group = "Project Profile Report" },

        };
    }
}

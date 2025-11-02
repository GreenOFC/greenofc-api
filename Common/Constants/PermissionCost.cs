namespace _24hplusdotnetcore.Common.Constants
{
    public struct PermissionCost
    {
        public const string ViewAllCimb = "cimb.view.all";
        public const string ViewAllEc = "ec.view.all";
        public const string ProcessTicket = "ticket.process";
        public const string ViewAllF88 = "f88.view.all";
        public const string ViewAllMc = "mc.view.all";
        public const string MCCheckSim = "mc.check.sim";
        public const string MCCheckTool = "mc.check.tool";
        public const string MCSendStatus = "mc.send.status";
        public const string MCReportCheckSim = "mc.report.check.sim";
        public const string MCReportToolPrecheck = "mc.report.tool.precheck";
        public const string ViewAllVps = "vps.view.all";
        public const string ViewAllTrustingSocial = "mc.sim.view.all.payload";
        public const string ViewAllMafc = "mafc.view.all";
        public const string MafcGetPayload = "mafc.get.payload";
        public const string MafcSendDefer = "mafc.send.defer";
        public const string MafcSendStatus = "mafc.send.status";
        public const string MafcUpdateRecord = "mafc.update.record";
        public const string MafcCheckDupV3 = "mafc.check.dupv3";
        public const string MafcGenerateDNFile = "mafc.generate.dn.file";

        public const string DataMCProsessingCreate = "mc.create.payload";
        public const string EcDataProsessingViewAll = "ec.view.all.payload";
        public const string EcOfferListViewAll = "ec.view.all.offer";
        public const string EcSendOffer = "ec.send.offer";
        public const string EcUpdateRecord = "ec.update.record";
        public const string EcUpdateStatus = "ec.update.status";
        public const string F88ReportNoti = "f88.report.noti";
        public const string CustomerReturnStatus = "customer.return.status";
        public const string CustomerViewHistory = "customer.view.history";

        // User
        public const string AddUserRole = "user.add.role";
        public const string RemoveUserRole = "user.remove.role";
        public const string ReviewUserProfile = "user.review.profile";
        public const string GetAllUserProfile = "user.get.all";
        public const string GetWaitingUserProfile = "user.get.waiting";
        public const string GetListSuspendedUserProfile = "user.get.list.suspended";
        public const string UpdateUserStatus = "user.update.status";
        public const string ResetUesrPassword = "user.reset.password";
        public const string DownloadAllUserProfile = "user.download.all";
        public const string UserCreate = "user.create";
        public const string UserCreateMulti = "user.create.multi";
        public const string UserViewHistory = "user.view.history";


        // Role
        public const string RoleCreate = "role.create";
        public const string RoleUpdate = "role.update";
        public const string RoleDelete = "role.delete";
        public const string RoleGet = "role.get";
        public const string RoleGetList = "role.get.list";
        public const string RoleGetBelongUser = "role.get.belong.user";

        // POS
        public const string PosCreate = "pos.create";
        public const string PosUpdate = "pos.update";
        public const string PosDelete = "pos.delete";
        public const string PosGet = "pos.get";

        // Banner
        public const string BannerCreate = "banner.create";
        public const string BannerUpdate = "banner.update";
        public const string BannerDelete = "banner.delete";

        // News
        public const string NewsCreate = "news.create";
        public const string NewsUpdate = "news.update";
        public const string NewsDelete = "news.delete";

        // Group Noti
        public const string GroupNotiCreate = "group.noti.create";
        public const string GroupNotiUpdate = "group.noti.update";
        public const string GroupNotiDelete = "group.noti.delete";
        public const string GroupNotiGet = "group.noti.get";

        public const string LeadOkVayApprove = "leadOkVay.approve";
        public const string LeadOkVayReject = "leadOkVay.reject";

        // PTF
        public const string PtfOmniGetPayload = "ptf.get.payload";
        public const string PtfOmniEditCustomer = "ptf.edit.customer";
        public const string PtfOmniSyncLoan = "ptf.sync.loan";
        public const string PtfOmniSyncAllLoan = "ptf.sync.all.loan";

        // Sale Chanel
        public const string SaleChanelGet = "salechanel.get";
        public const string SaleChanelCreate = "salechanel.create";
        public const string SaleChanelDelete = "salechanel.delete";
        public const string SaleChanelSync = "salechanel.sync";

        // Sale Chanel Config User
        public const string SaleChanelConfigUserGet = "salechanelConfigUser.get";
        public const string SaleChanelConfigUserCreate = "salechanelConfigUser.create";
        public const string SaleChanelConfigUserUpdate = "salechanelConfigUser.update";
        public const string SaleChanelConfigUserDelete = "salechanelConfigUser.delete";

        //Check Sim
        public const string CheckSimMcV2 = "check.sim.mc.v2";
        public const string CheckSimGet = "check.sim.get";
        public const string CheckSimExport = "check.sim.export";
        public const string CheckSimPtf = "check.sim.ptf";

        //Ticket
        public const string TicketReport = "ticket.report";

        // DebtManagement
        public const string DebtManagementCreate = "DebtManagement.Create";

        //ProjectProfileReport
        public const string ProjectProfileReportImport = "ProjectProfileReport.Import";

        public struct TransactionPermission
        {
            public const string Transaction = "Transaction";
            public const string Transaction_Refund = "transaction.refund";
            public const string TransactionViewHistory = "transaction.view.history";
        }
        public struct AdminPermission
        {
            public const string Admin = "Admin";

            public const string Admin_LeadEcManagement_ViewAll = "Admin.LeadEcManagement.ViewAll";
            public const string Admin_LeadVpsManagement_ViewAll = "Admin.LeadVpsManagement.ViewAll";
            public const string Admin_LeadF88Management_ViewAll = "Admin.LeadF88Management.ViewAll";
            public const string Admin_LeadVibsManagement_ViewAll = "Admin.LeadVibsManagement.ViewAll";
            public const string Admin_LeadMcDebtManagement_ViewAll = "Admin.LeadMcDebtManagement.ViewAll";
            public const string Admin_LeadMafcManagement_ViewAll = "Admin.LeadMafcManagement.ViewAll";
            public const string Admin_LeadMcManagement_ViewAll = "Admin.LeadMcManagement.ViewAll";
            public const string Admin_LeadCimbManagement_ViewAll = "Admin.LeadCimbManagement.ViewAll";
            public const string Admin_LeadShinhanManagement_ViewAll = "Admin.LeadShinhanManagement.ViewAll";
            public const string Admin_LeadHomeManagement_ViewAll = "Admin.LeadHomeManagement.ViewAll";
            public const string Admin_LeadOkVayManagement_ViewAll = "Admin.LeadOkVayManagement.ViewAll";
            public const string Admin_LeadVbiManagement_ViewAll = "Admin.LeadVbiManagement.ViewAll";
            public const string Admin_LeadPtfManagement_ViewAll = "Admin.LeadPtfManagement.ViewAll";
            public const string HyperLead_ViewAll = "Admin.HyperLead.ViewAll";
            public const string Admin_Ticket_ViewAll = "Admin.Ticket.ViewAll";
            public const string Admin_User_ViewAll = "Admin.User.ViewAll";
            public const string Admin_Transaction_ViewAll = "Admin.Transaction.ViewAll";
            public const string Admin_Transaction_Ipn_ViewAll = "Admin.Transaction.Ipn.ViewAll";

            public const string Admin_PosManagement_ViewAll = "Admin.PosManagement.ViewAll";
            public const string Admin_HistoryManagement_ViewAll = "Admin.HistoryManagement.ViewAll";
            public const string Admin_CheckCustomer_ViewAll = "Admin.CheckCustomer.ViewAll";
            public const string Admin_ProjectProfileReport_ViewAll = "Admin.ProjectProfileReport.ViewAll";

            public const string Admin_DebtManagement_ViewAll = "Admin.DebtManagement.ViewAll";
            public const string Admin_ImportFile_ViewAll = "Admin.ImportFile.ViewAll";
            public const string LogApi_ViewAll = "Admin.LogApi.ViewAll";
            public const string CheckSim_ViewAll = "Admin.CheckSim.ViewAll";
        }

        public struct HeadOfSaleAdminPermission
        {
            public const string HeadOfSaleAdmin = "HeadOfSaleAdmin";

            public const string HeadOfSaleAdmin_LeadEcManagement_ViewAll = "HeadOfSaleAdmin.LeadEcManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadVpsManagement_ViewAll = "HeadOfSaleAdmin.LeadVpsManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadF88Management_ViewAll = "HeadOfSaleAdmin.LeadF88Management.ViewAll";
            public const string HeadOfSaleAdmin_LeadVibsManagement_ViewAll = "HeadOfSaleAdmin.LeadVibsManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadMcDebtManagement_ViewAll = "HeadOfSaleAdmin.LeadMcDebtManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadMafcManagement_ViewAll = "HeadOfSaleAdmin.LeadMafcManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadMcManagement_ViewAll = "HeadOfSaleAdmin.LeadMcManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadCimbManagement_ViewAll = "HeadOfSaleAdmin.LeadCimbManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadShinhanManagement_ViewAll = "HeadOfSaleAdmin.LeadShinhanManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadHomeManagement_ViewAll = "HeadOfSaleAdmin.LeadHomeManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadOkVayManagement_ViewAll = "HeadOfSaleAdmin.LeadOkVayManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadVbiManagement_ViewAll = "HeadOfSaleAdmin.LeadVbiManagement.ViewAll";
            public const string HeadOfSaleAdmin_LeadPtfManagement_ViewAll = "HeadOfSaleAdmin.LeadPtfManagement.ViewAll";
            public const string HyperLead_ViewAll = "HeadOfSaleAdmin.HyperLead.ViewAll";
            public const string HeadOfSaleAdmin_Ticket_ViewAll = "HeadOfSaleAdmin.Ticket.ViewAll";
            public const string HeadOfSaleAdmin_User_ViewAll = "HeadOfSaleAdmin.User.ViewAll";
            public const string HeadOfSaleAdmin_Transaction_ViewAll = "HeadOfSaleAdmin.Transaction.ViewAll";

            public const string HeadOfSaleAdmin_PosManagement_ViewAll = "HeadOfSaleAdmin.PosManagement.ViewAll";
            public const string HeadOfSaleAdmin_HistoryManagement_ViewAll = "HeadOfSaleAdmin.HistoryManagement.ViewAll";
            public const string HeadOfSaleAdmin_CheckCustomer_ViewAll = "HeadOfSaleAdmin.CheckCustomer.ViewAll";
            public const string HeadOfSaleAdmin_ProjectProfileReport_ViewAll = "HeadOfSaleAdmin.ProjectProfileReport.ViewAll";

            public const string HeadOfSaleAdmin_DebtManagement_ViewAll = "HeadOfSaleAdmin.DebtManagement.ViewAll";
            public const string HeadOfSaleAdmin_ImportFile_ViewAll = "HeadOfSaleAdmin.ImportFile.ViewAll";
            public const string LogApi_ViewAll = "HeadOfSaleAdmin.LogApi.ViewAll";
            public const string CheckSim_ViewAll = "HeadOfSaleAdmin.CheckSim.ViewAll";
        }

        public struct PosLeadPermission
        {
            public const string PosLead = "PosLead";

            public const string PosLead_LeadEcManagement_ViewAll = "PosLead.LeadEcManagement.ViewAll";
            public const string PosLead_LeadVpsManagement_ViewAll = "PosLead.LeadVpsManagement.ViewAll";
            public const string PosLead_LeadF88Management_ViewAll = "PosLead.LeadF88Management.ViewAll";
            public const string PosLead_LeadVibsManagement_ViewAll = "PosLead.LeadVibsManagement.ViewAll";
            public const string PosLead_LeadMcDebtManagement_ViewAll = "PosLead.LeadMcDebtManagement.ViewAll";
            public const string PosLead_LeadMafcManagement_ViewAll = "PosLead.LeadMafcManagement.ViewAll";
            public const string PosLead_LeadMcManagement_ViewAll = "PosLead.LeadMcManagement.ViewAll";
            public const string PosLead_LeadCimbManagement_ViewAll = "PosLead.LeadCimbManagement.ViewAll";
            public const string PosLead_LeadShinhanManagement_ViewAll = "PosLead.LeadShinhanManagement.ViewAll";
            public const string PosLead_LeadHomeManagement_ViewAll = "PosLead.LeadHomeManagement.ViewAll";
            public const string PosLead_LeadOkVayManagement_ViewAll = "PosLead.LeadOkVayManagement.ViewAll";
            public const string PosLead_LeadVbiManagement_ViewAll = "PosLead.LeadVbiManagement.ViewAll";
            public const string PosLead_LeadPtfManagement_ViewAll = "PosLead.LeadPtfManagement.ViewAll";
            public const string HyperLead_ViewAll = "PosLead.HyperLead.ViewAll";
            public const string PosLead_Ticket_ViewAll = "PosLead.Ticket.ViewAll";
            public const string PosLead_User_ViewAll = "PosLead.User.ViewAll";
            public const string PosLead_Transaction_ViewAll = "PosLead.Transaction.ViewAll";

            public const string PosLead_PosManagement_ViewAll = "PosLead.PosManagement.ViewAll";
            public const string PosLead_HistoryManagement_ViewAll = "PosLead.HistoryManagement.ViewAll";
            public const string PosLead_CheckCustomer_ViewAll = "PosLead.CheckCustomer.ViewAll";
            public const string PosLead_ProjectProfileReport_ViewAll = "PosLead.ProjectProfileReport.ViewAll";

            public const string PosLead_DebtManagement_ViewAll = "PosLead.DebtManagement.ViewAll";
            public const string PosLead_ImportFile_ViewAll = "PosLead.ImportFile.ViewAll";
            public const string LogApi_ViewAll = "PosLead.LogApi.ViewAll";
            public const string CheckSim_ViewAll = "PosLead.CheckSim.ViewAll";
        }

        public struct AsmPermission
        {
            public const string Asm = "ASM";

            public const string Asm_LeadEcManagement_ViewAll = "Asm.LeadEcManagement.ViewAll";
            public const string Asm_LeadVpsManagement_ViewAll = "Asm.LeadVpsManagement.ViewAll";
            public const string Asm_LeadF88Management_ViewAll = "Asm.LeadF88Management.ViewAll";
            public const string Asm_LeadVibsManagement_ViewAll = "Asm.LeadVibsManagement.ViewAll";
            public const string Asm_LeadMcDebtManagement_ViewAll = "Asm.LeadMcDebtManagement.ViewAll";
            public const string Asm_LeadMafcManagement_ViewAll = "Asm.LeadMafcManagement.ViewAll";
            public const string Asm_LeadMcManagement_ViewAll = "Asm.LeadMcManagement.ViewAll";
            public const string Asm_LeadCimbManagement_ViewAll = "Asm.LeadCimbManagement.ViewAll";
            public const string Asm_LeadShinhanManagement_ViewAll = "Asm.LeadShinhanManagement.ViewAll";
            public const string Asm_LeadHomeManagement_ViewAll = "Asm.LeadHomeManagement.ViewAll";
            public const string Asm_LeadOkVayManagement_ViewAll = "Asm.LeadOkVayManagement.ViewAll";
            public const string Asm_LeadVbiManagement_ViewAll = "Asm.LeadVbiManagement.ViewAll";
            public const string Asm_LeadPtfManagement_ViewAll = "Asm.LeadPtfManagement.ViewAll";
            public const string HyperLead_ViewAll = "Asm.HyperLead.ViewAll";
            public const string Asm_Ticket_ViewAll = "Asm.Ticket.ViewAll";
            public const string Asm_User_ViewAll = "Asm.User.ViewAll";
            public const string Asm_Transaction_ViewAll = "Asm.Transaction.ViewAll";

            public const string Asm_PosManagement_ViewAll = "Asm.PosManagement.ViewAll";
            public const string Asm_HistoryManagement_ViewAll = "Asm.HistoryManagement.ViewAll";
            public const string Asm_CheckCustomer_ViewAll = "Asm.CheckCustomer.ViewAll";
            public const string Asm_ProjectProfileReport_ViewAll = "Asm.ProjectProfileReport.ViewAll";

            public const string Asm_DebtManagement_ViewAll = "Asm.DebtManagement.ViewAll";
            public const string Asm_ImportFile_ViewAll = "Asm.ImportFile.ViewAll";
            public const string LogApi_ViewAll = "Asm.LogApi.ViewAll";
            public const string CheckSim_ViewAll = "Asm.CheckSim.ViewAll";
        }
        public struct TeamLeaderPermission
        {
            public const string TeamLeader = "TeamLeader";
            public const string TeamLead = "TL";

            public const string TeamLeader_LeadEcManagement_ViewAll = "TeamLeader.LeadEcManagement.ViewAll";
            public const string TeamLeader_LeadVpsManagement_ViewAll = "TeamLeader.LeadVpsManagement.ViewAll";
            public const string TeamLeader_LeadF88Management_ViewAll = "TeamLeader.LeadF88Management.ViewAll";
            public const string TeamLeader_LeadVibsManagement_ViewAll = "TeamLeader.LeadVibsManagement.ViewAll";
            public const string TeamLeader_LeadMcDebtManagement_ViewAll = "TeamLeader.LeadMcDebtManagement.ViewAll";
            public const string TeamLeader_LeadMafcManagement_ViewAll = "TeamLeader.LeadMafcManagement.ViewAll";
            public const string TeamLeader_LeadMcManagement_ViewAll = "TeamLeader.LeadMcManagement.ViewAll";
            public const string TeamLeader_LeadCimbManagement_ViewAll = "TeamLeader.LeadCimbManagement.ViewAll";
            public const string TeamLeader_LeadShinhanManagement_ViewAll = "TeamLeader.LeadShinhanManagement.ViewAll";
            public const string TeamLeader_LeadHomeManagement_ViewAll = "TeamLeader.LeadHomeManagement.ViewAll";
            public const string TeamLeader_LeadOkVayManagement_ViewAll = "TeamLeader.LeadOkVayManagement.ViewAll";
            public const string TeamLeader_LeadVbiManagement_ViewAll = "TeamLeader.LeadVbiManagement.ViewAll";
            public const string TeamLeader_LeadPtfManagement_ViewAll = "TeamLeader.LeadPtfManagement.ViewAll";
            public const string HyperLead_ViewAll = "TeamLeader.HyperLead.ViewAll";
            public const string TeamLeader_Ticket_ViewAll = "TeamLeader.Ticket.ViewAll";
            public const string TeamLeader_User_ViewAll = "TeamLeader.User.ViewAll";
            public const string TeamLeader_Transaction_ViewAll = "TeamLeader.Transaction.ViewAll";

            public const string TeamLeader_PosManagement_ViewAll = "TeamLeader.PosManagement.ViewAll";
            public const string TeamLeader_HistoryManagement_ViewAll = "TeamLeader.HistoryManagement.ViewAll";
            public const string TeamLeader_CheckCustomer_ViewAll = "TeamLeader.CheckCustomer.ViewAll";
            public const string TeamLeader_ProjectProfileReport_ViewAll = "TeamLeader.ProjectProfileReport.ViewAll";

            public const string TeamLeader_DebtManagement_ViewAll = "TeamLeader.DebtManagement.ViewAll";
            public const string TeamLeader_ImportFile_ViewAll = "TeamLeader.ImportFile.ViewAll";
            public const string LogApi_ViewAll = "TeamLeader.LogApi.ViewAll";
            public const string CheckSim_ViewAll = "TeamLeader.CheckSim.ViewAll";
        }
    }
}

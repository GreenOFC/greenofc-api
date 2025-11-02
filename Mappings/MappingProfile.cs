using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.AT;
using _24hplusdotnetcore.ModelDtos.Banners;
using _24hplusdotnetcore.ModelDtos.CheckInitContractModels;
using _24hplusdotnetcore.ModelDtos.CheckLoans;
using _24hplusdotnetcore.ModelDtos.File;
using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using _24hplusdotnetcore.ModelDtos.LeadVibs;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.ModelDtos.MCDebts;
using _24hplusdotnetcore.ModelDtos.News;
using _24hplusdotnetcore.ModelDtos.Otps;
using _24hplusdotnetcore.ModelDtos.Permissions;
using _24hplusdotnetcore.ModelDtos.Roles;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.ModelDtos.UserSuspendeds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace _24hplusdotnetcore.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region CRM to Customer

            CreateMap<Record, Personal>()
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Potentialname))
                .ForMember(dest => dest.Gender, src => src.MapFrom(x => x.Cf1026))
                .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.Cf854))
                .ForMember(dest => dest.IdCard, src => src.MapFrom(x => x.Cf1050))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Cf1028))
                .ForMember(dest => dest.DateOfBirth, src => src.MapFrom(x => x.Cf948))
                .ForMember(dest => dest.MaritalStatus, src => src.MapFrom(x => x.Cf1030));

            CreateMap<Record, Working>()
                .ForMember(dest => dest.Job, src => src.MapFrom(x => x.Cf1246))
                .ForMember(dest => dest.Income, src => src.MapFrom(x => x.Cf884))
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Cf962))
                .ForMember(dest => dest.CompanyAddress, src => src.MapFrom(x => new Address { FullAddress = x.Cf1020 }))
                .ForMember(dest => dest.CompanyPhone, src => src.MapFrom(x => x.Cf976))
                .ForMember(dest => dest.Position, src => src.MapFrom(x => x.Cf982))
                .ForMember(dest => dest.WorkPeriod, src => src.MapFrom(x => x.Cf984))
                .ForMember(dest => dest.TypeOfContract, src => src.MapFrom(x => x.Cf986))
                .ForMember(dest => dest.HealthCardInssurance, src => src.MapFrom(x => x.Cf988));

            CreateMap<Record, Loan>()
                .ForMember(dest => dest.Amount, src => src.MapFrom(x => x.Cf968))
                .ForMember(dest => dest.Product, src => src.MapFrom(x => x.Cf1032))
                .ForMember(dest => dest.Term, src => src.MapFrom(x => x.Cf990));

            CreateMap<Record, Models.Result>()
                .ForMember(dest => dest.Note, src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Cf1184));

            CreateMap<Record, Customer>()
                .ForMember(dest => dest.Personal, src => src.MapFrom(x => x))
                .ForMember(dest => dest.Working, src => src.MapFrom(x => x))
                .ForMember(dest => dest.TemporaryAddress, src => src.MapFrom(x => new Address { FullAddress = x.Cf1020 }))
                .ForMember(dest => dest.Loan, src => src.MapFrom(x => x))
                .ForMember(dest => dest.Result, src => src.MapFrom(x => x))
                .AfterMap((src, dest) => dest.UserName = src.Modifiedby.Label.Split("-")[0])
                .ForMember(dest => dest.CRMId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Id, src => src.Ignore());

            #endregion

            #region CRM to LeadCRM

            CreateMap<AssignedUserId, LeadCrmUser>();

            CreateMap<Record, LeadCrm>()
                .ForMember(dest => dest.LeadCrmId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Id, src => src.Ignore());

            #endregion

            #region LeadCRM to CRM

            CreateMap<LeadCrmUser, AssignedUserId>();

            CreateMap<LeadCrm, CRMRequestDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.LeadCrmId))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => x.AssignedUserId != null ? x.AssignedUserId.Value : "-"))
                .ForMember(dest => dest.Cf1190, src => src.MapFrom(x => x.GetStatusMessage()))
                .ForMember(dest => dest.Cf1174, src => src.MapFrom(x => x.PostbackMA != null ? x.PostbackMA.DcLastNote : x.Cf1174))
                .ForMember(dest => dest.Cf884, src => src.MapFrom(x => x.Cf884 ?? "-"))
                .ForMember(dest => dest.Cf892, src => src.MapFrom(x => x.Cf892 ?? "-"))
                .ForMember(dest => dest.Cf990, src => src.MapFrom(x => x.Cf990 ?? "-"))
                .ForMember(dest => dest.Cf1002, src => src.MapFrom(x => x.Cf1002 ?? "-"))
                .ForMember(dest => dest.Cf1188, src => src.MapFrom(x => x.Cf1188 ?? "-"))
                .ForMember(dest => dest.Cf1196, src => src.MapFrom(x => x.Cf1196 ?? "-"))
                .ForMember(dest => dest.Cf1210, src => src.MapFrom(x => x.Cf1210 ?? "-"))
                .ForMember(dest => dest.Cf1264, src => src.MapFrom(x => x.Cf1264 ?? "-"))
                .ForMember(dest => dest.Cf1404, src => src.MapFrom(x => x.Cf1404 ?? "-"))
                .ForMember(dest => dest.Cf968, src => src.MapFrom(x => x.Cf968 ?? "-"))
                .ForMember(dest => dest.Cf1026, src => src.MapFrom(x => x.Cf1026 ?? "-"))
                .ForMember(dest => dest.Cf1036, src => src.MapFrom(x => x.Cf1036 ?? "-"))
                .ForMember(dest => dest.Cf1040, src => src.MapFrom(x => x.Cf1040 ?? "-"))
                .ForMember(dest => dest.Cf1178, src => src.MapFrom(x => x.Cf1178 ?? "-"))
                .ForMember(dest => dest.Cf1434, src => src.MapFrom(x => "Khách hàng mới"))
                .ForMember(dest => dest.Cf1436, src => src.MapFrom(x => "?????"));

            #endregion

            #region LeadCRM to MA

            CreateMap<LeadCrm, MARequestDataModel>()
                .ForMember(dest => dest.LEAD_ID, src => src.MapFrom(x => x.PotentialNo))
                .ForMember(dest => dest.ID_NO, src => src.MapFrom(x => x.Cf1050))
                .ForMember(dest => dest.CONTACT_NAME, src => src.MapFrom(x => x.Potentialname))
                .ForMember(dest => dest.PHONE, src => src.MapFrom(x => x.Cf854))
                .ForMember(dest => dest.CURRENT_ADDRESS, src => src.MapFrom(x => x.Cf892))
                .ForMember(dest => dest.PRODUCT, src => src.MapFrom(x => x.Cf1032))
                .ForMember(dest => dest.T_STATUS_DATE, src => src.MapFrom(x => x.Cf1266))
                .ForMember(dest => dest.APPOINTMENT_DATE, src => src.MapFrom(x => x.Cf1052))
                .ForMember(dest => dest.APPOINTMENT_ADDRESS, src => src.MapFrom(x => x.Cf892))
                .ForMember(dest => dest.TSA_IN_CHARGE, src => src.MapFrom(x => "24H-TE0001"))
                .ForMember(dest => dest.TST_TEAM, src => src.MapFrom(x => x.Cf972))
                .ForMember(dest => dest.REQUEST_DOCUMENT, src => src.MapFrom(x => x.Cf1036))
                .ForMember(dest => dest.DOB, src => src.MapFrom(x => x.Cf948))
                .ForMember(dest => dest.GENDER, src => src.MapFrom(x => x.Cf1026))
                .ForMember(dest => dest.COMPANY_NAME, src => src.MapFrom(x => x.Cf962))
                .ForMember(dest => dest.COMPANY_ADDR, src => src.MapFrom(x => x.Cf1020))
                .ForMember(dest => dest.TEL_COMPANY, src => src.MapFrom(x => x.Cf976))
                .ForMember(dest => dest.AREA, src => src.MapFrom(x => x.Cf1020))
                .ForMember(dest => dest.MARITAL_STATUS, src => src.MapFrom(x => x.Cf1030))
                .ForMember(dest => dest.OWNER, src => src.MapFrom(x => x.Cf978))
                .ForMember(dest => dest.INCOME, src => src.MapFrom(x => x.Cf884))
                .ForMember(dest => dest.POSITION, src => src.MapFrom(x => x.Cf982))
                .ForMember(dest => dest.WORK_PERIOD, src => src.MapFrom(x => x.Cf984))
                .ForMember(dest => dest.TYPE_OF_CONTRACT, src => src.MapFrom(x => x.Cf986))
                .ForMember(dest => dest.HEALTH_CARD, src => src.MapFrom(x => x.Cf988))
                .ForMember(dest => dest.LOAN_AMOUNT, src => src.MapFrom(x => x.Cf968))
                .ForMember(dest => dest.TENURE, src => src.MapFrom(x => x.Cf990))
                .ForMember(dest => dest.APP_DATE, src => src.MapFrom(x => ""))
                .ForMember(dest => dest.DISBURSAL_DATE, src => src.MapFrom(x => ""))
                .ForMember(dest => dest.GENERATE_TO_LEAD, src => src.MapFrom(x => ""))
                .ForMember(dest => dest.FOLLOWED_DATE, src => src.MapFrom(x => ""))
                .ForMember(dest => dest.PERMANENT_ADDR, src => src.MapFrom(x => x.Cf1002))
                .ForMember(dest => dest.TSA_NAME, src => src.MapFrom(x => x.GetSalesStaffName()))
                .ForMember(dest => dest.TSA_CAMPAIN, src => src.MapFrom(x => ""))
                .ForMember(dest => dest.TSA_GROUP, src => src.MapFrom(x => x.Cf1008))
                .ForMember(dest => dest.TSA_LAST_NOTES, src => src.MapFrom(x => x.Cf1196))
                .ForMember(dest => dest.OCCUPATION, src => src.MapFrom(x => x.Cf1246))
                .ForMember(dest => dest.ROUTE, src => src.MapFrom(x => x.Cf1404));

            #endregion

            #region MA to LeadCRM

            CreateMap<MAPostBackRequestModel, PostbackMA>();

            CreateMap<MAPostBackRequestModel, LeadCrm>()
                .ForMember(dest => dest.Cf1184, src => src.MapFrom(x => x.GetStatusMessage()))
                .ForMember(dest => dest.Cf1392, src => src.MapFrom(x => x.GetDetailStatusMessage()))
                .ForMember(dest => dest.PostbackMA, src => src.MapFrom(x => x));

            #endregion

            CreateMap<GetCaseRequestDto, GetCaseMCRequestDto>()
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status.ToString()));


            #region Checklist
            CreateMap<GroupDtoModel, GroupDocument>();
            CreateMap<DocumentDtoModel, DocumentUpload>();
            #endregion

            CreateMap<MCCheckCICInfoResponseDto, MCCheckCICModel>();

            CreateMap<FIBOResquestDto, LeadCrm>()
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => string.IsNullOrEmpty(x.ContactName) ? "Khách hàng tiềm năng" : x.ContactName))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Cf1242, src => src.MapFrom(x => x.Province))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => string.IsNullOrEmpty(x.Cmnd) ? "-" : x.Cmnd))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => x.LeadSource));

            CreateMap<ATResquestDto, LeadCrm>()
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => string.IsNullOrEmpty(x.Name) ? "Khách hàng tiềm năng" : x.Name))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Phone))
                .ForMember(dest => dest.Cf948, src => src.MapFrom(x => x.Dob))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => string.IsNullOrEmpty(x.IdCard) ? "-" : x.IdCard))
                .ForMember(dest => dest.Cf968, src => src.MapFrom(x => x.LoanAmount))
                .ForMember(dest => dest.Cf884, src => src.MapFrom(x => x.Income))
                .ForMember(dest => dest.Cf1028, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.Cf1002, src => src.MapFrom(x => x.Address))
                .ForMember(dest => dest.Cf1242, src => src.MapFrom(x => x.Province))
                .ForMember(dest => dest.Cf1496, src => src.MapFrom(x => x.ProvidedDoc))
                .ForMember(dest => dest.Cf1492, src => src.MapFrom(x => x.DocumentType))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => x.LeadSource));

            CreateMap<ModelDtos.StorageModels.StorageFileResponse, OCRFile>();
            CreateMap<OCRTranferResponse, OCRResponse>();
            CreateMap<OCRResponse, OCRResponseModel>();
            CreateMap<OCR, OCRInfoResponse>()
                .ForMember(dest => dest.Result, src => src.MapFrom(x => x.Result.Detail));

            CreateMap<OCRResultDetail, OCRResultDetailResponse>();

            CreateMap<UserRequestDto, User>();

            CreateMap<Customer, CRMRequestDto>()
                .ForMember(dest => dest.Cf1178, src => src.MapFrom(x => x.GetFinanceCompany()))
                .ForMember(dest => dest.Leadsource, src => src.MapFrom(x => x.GetLeadSource()))
                .ForMember(dest => dest.Potentialname, src => src.MapFrom(x => x.Personal.Name))
                .ForMember(dest => dest.OpportunityType, src => src.MapFrom(x => x.Loan.ProductType))
                .ForMember(dest => dest.Cf1050, src => src.MapFrom(x => x.Personal.IdCard))
                .ForMember(dest => dest.Cf1350, src => src.MapFrom(x => $"{x.Personal.GetIdCardDate():yyyy-MM-dd}"))
                .ForMember(dest => dest.Cf1408, src => src.MapFrom(x => x.Personal.IdCardProvince))
                .ForMember(dest => dest.Cf1026, src => src.MapFrom(x => x.Personal != null && !string.IsNullOrEmpty(x.Personal.Gender) ? x.Personal.Gender : "-"))
                .ForMember(dest => dest.Cf948, src => src.MapFrom(x => $"{x.Personal.GetDateOfBirth():yyyy-MM-dd}"))
                .ForMember(dest => dest.Cf854, src => src.MapFrom(x => x.Personal.Phone))
                .ForMember(dest => dest.Cf1002, src => src.MapFrom(x => x.ResidentAddress.GetFullAddress()))
                .ForMember(dest => dest.Cf1238, src => src.MapFrom(x => x.IsTheSameResidentAddress ? "1" : "0"))
                .ForMember(dest => dest.Cf892, src => src.MapFrom(x => x.IsTheSameResidentAddress ? x.ResidentAddress.GetFullAddress() : x.TemporaryAddress.GetFullAddress()))
                .ForMember(dest => dest.Cf1246, src => src.MapFrom(x => x.Working.Job))
                .ForMember(dest => dest.Cf964, src => src.MapFrom(x => x.Working.CompanyAddress.GetAddress()))
                .ForMember(dest => dest.Cf884, src => src.MapFrom(x => x.Working != null && !string.IsNullOrEmpty(x.Working.Income) ? x.Working.Income.Replace(",", string.Empty) : "-"))
                .ForMember(dest => dest.Cf1410, src => src.MapFrom(x => x.Working.IncomeMethod))
                .ForMember(dest => dest.Cf1412, src => src.MapFrom(x => x.Working.OtherLoans.Replace(",", string.Empty)))
                .ForMember(dest => dest.Cf990, src => src.MapFrom(x => x.Loan.Term))
                .ForMember(dest => dest.Cf1210, src => src.MapFrom(x => x.Loan.Category))
                .ForMember(dest => dest.Cf1040, src => src.MapFrom(x => x.Loan.Product))
                .ForMember(dest => dest.Cf1220, src => src.MapFrom(x => x.Loan.BuyInsurance ? "1" : "0"))
                .ForMember(dest => dest.Cf1224, src => src.MapFrom(x => x.ProductLine))
                .ForMember(dest => dest.Cf968, src => src.MapFrom(x => x.Loan != null ? x.Loan.GetAmount() ?? 0 : 0))
                .ForMember(dest => dest.Cf1424, src => src.MapFrom(x => x.Loan.GetTotalAmount()))
                .ForMember(dest => dest.Cf1054, src => src.MapFrom(x => x.Loan.SignAddress))
                .ForMember(dest => dest.Cf1414, src => src.MapFrom(x => x.SaleInfo.Name))
                .ForMember(dest => dest.Cf1422, src => src.MapFrom(x => x.SaleInfo.Code))
                .ForMember(dest => dest.Cf1418, src => src.MapFrom(x => x.SaleInfo.Phone))
                .ForMember(dest => dest.Cf1196, src => src.MapFrom(x => x.SaleInfo.Note + " "))
                .ForMember(dest => dest.Cf1208, src => src.MapFrom(x => x.ContractCode))
                .ForMember(dest => dest.Cf1420, src => src.MapFrom(x => x.Result.Reason))
                .ForMember(dest => dest.Cf1036, src => src.MapFrom(x => x.GetDocumentType()))
                .ForMember(dest => dest.Cf1426, src => src.MapFrom(x => x.GetDocumentUri()))
                .ForMember(dest => dest.Cf1440, src => src.MapFrom(x => x.GetReturnDocumentUri()))
                .ForMember(dest => dest.Cf1432, src => src.MapFrom(x => x.RecordFile.Uri ?? string.Empty))
                .ForMember(dest => dest.Cf1512, src => src.MapFrom(x => x.RecordFileBackup.Uri ?? string.Empty))
                .ForMember(dest => dest.SalesStage, src => src.MapFrom(x => "1.KH mới"))
                .ForMember(dest => dest.Cf1184, src => src.MapFrom(x => x.Result.ReturnStatus ?? string.Empty))
                .ForMember(dest => dest.Cf1430, src => src.MapFrom(x => x.Status))
                .ForMember(dest => dest.AssignedUserId, src => src.MapFrom(x => "19x3017"))
                .ForMember(dest => dest.Cf1230, src => src.MapFrom(x => string.Empty))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => string.IsNullOrEmpty(x.CRMId) ? null : x.CRMId))
                .ForMember(dest => dest.Cf1434, src => src.MapFrom(x => "Khách hàng mới"))
                .ForMember(dest => dest.Cf1436, src => src.MapFrom(x => "?????"))
                .ForMember(dest => dest.Cf1206, src => src.MapFrom(x => "1"))
                .ForMember(dest => dest.Cf1446, src => src.MapFrom(x => x.Result.GetRejectCode() ?? string.Empty))
                .ForMember(dest => dest.Cf1494, src => src.MapFrom(x => x.ECRequest))
                .ForMember(dest => dest.Cf1498, src => src.MapFrom(x => x.SaleInfo.ApprovedByAdmin == true ? "1" : "0"))
                .ForMember(dest => dest.Cf1638, src => src.MapFrom(x => x.SaleInfo.ApprovedByASM == true ? "1" : "0"))
                .ForMember(dest => dest.Cf1502, src => src.MapFrom(x => x.Result.ApprovedAmount ?? 0))
                .ForMember(dest => dest.Cf1504, src => src.MapFrom(x => x.Result.ContractNumber ?? string.Empty))
                .ForMember(dest => dest.Cf1506, src => src.MapFrom(x => x.Result.ApprovedDate != null ? $"{x.Result.ApprovedDate:dd/MM/yyyy}" : ""))
                .ForMember(dest => dest.Cf1042, src => src.MapFrom(x => x.Spouse.Name))
                .ForMember(dest => dest.Cf1044, src => src.MapFrom(x => x.Spouse.IdCard))
                .ForMember(dest => dest.Cf1046, src => src.MapFrom(x => x.Spouse.Phone))
                .ForMember(dest => dest.Cf1448, src => src.MapFrom(x => x.Personal.Title))
                .ForMember(dest => dest.Cf1450, src => src.MapFrom(x => x.Personal.OldIdCard))
                .ForMember(dest => dest.Cf1452, src => src.MapFrom(x => x.Personal.EducationLevel))
                .ForMember(dest => dest.Cf1454, src => src.MapFrom(x => x.Personal.NoOfDependent))
                .ForMember(dest => dest.Cf1456, src => src.MapFrom(x => x.TemporaryAddress.PropertyStatus))
                .ForMember(dest => dest.Cf1458, src => src.MapFrom(x => x.TemporaryAddress.DurationYear))
                .ForMember(dest => dest.Cf1460, src => src.MapFrom(x => x.TemporaryAddress.DurationMonth))
                .ForMember(dest => dest.Cf1462, src => src.MapFrom(x => x.FamilyBookNo))
                .ForMember(dest => dest.Cf1464, src => src.MapFrom(x => x.Working.DueDay))
                .ForMember(dest => dest.Cf1466, src => src.MapFrom(x => x.Working.IncomeMethod))
                .ForMember(dest => dest.Cf1468, src => src.MapFrom(x => x.Working.Constitution))
                .ForMember(dest => dest.Cf1470, src => src.MapFrom(x => x.Working.CompanyAddress.PropertyStatus))
                .ForMember(dest => dest.Cf1472, src => src.MapFrom(x => x.Referees.FirstOrDefault().Name))
                .ForMember(dest => dest.Cf1474, src => src.MapFrom(x => x.Referees.FirstOrDefault().Phone))
                .ForMember(dest => dest.Cf1476, src => src.MapFrom(x => x.Referees.FirstOrDefault().Relationship))
                .ForMember(dest => dest.Cf1478, src => src.MapFrom(x => x.Referees.Skip(1).FirstOrDefault().Name))
                .ForMember(dest => dest.Cf1480, src => src.MapFrom(x => x.Referees.Skip(1).FirstOrDefault().Phone))
                .ForMember(dest => dest.Cf1482, src => src.MapFrom(x => x.Referees.Skip(1).FirstOrDefault().Relationship))
                .ForMember(dest => dest.Cf1484, src => src.MapFrom(x => x.BankInfo.Name))
                .ForMember(dest => dest.Cf1486, src => src.MapFrom(x => x.BankInfo.Branch))
                .ForMember(dest => dest.Cf1488, src => src.MapFrom(x => x.BankInfo.AccountNo))
                .ForMember(dest => dest.Cf1490, src => src.MapFrom(x => x.Loan.Purpose))
                .ForMember(dest => dest.Cf1514, src => src.MapFrom(x => x.PosInfo.Name))
                .ForMember(dest => dest.Cf1516, src => src.MapFrom(x => x.PosInfo.Manager.Name))
                .ForMember(dest => dest.Cf1518, src => src.MapFrom(x => x.TeamLeadInfo.FullName))
                .ForMember(dest => dest.Cf1532, src => src.MapFrom(x => $"{x.Personal.GetOldIdCardDate():yyyy-MM-dd}"))
                .ForMember(dest => dest.Cf1534, src => src.MapFrom(x => x.Personal.OldIdCardProvince))
                .ForMember(dest => dest.Cf1538, src => src.MapFrom(x => x.Loan.TimeToBeAbleToAnswerThePhone))
                .ForMember(dest => dest.Cf1540, src => src.MapFrom(x => x.Working.SocialAccount))
                .ForMember(dest => dest.Cf1542, src => src.MapFrom(x => x.Working.SocialAccountDetail))
                .ForMember(dest => dest.Cf1560, src => src.MapFrom(x => x.TemporaryAddress.Province))
                .ForMember(dest => dest.Cf1580, src => src.MapFrom(x => x.TemporaryAddress.District))
                .ForMember(dest => dest.Cf1576, src => src.MapFrom(x => x.TemporaryAddress.Street))
                .ForMember(dest => dest.Cf1584, src => src.MapFrom(x => x.ResidentAddress.Street))
                .ForMember(dest => dest.Cf1088, src => src.MapFrom(x => x.ResidentAddress.District))
                .ForMember(dest => dest.Cf1586, src => src.MapFrom(x => x.ResidentAddress.Province))
                .ForMember(dest => dest.Cf1030, src => src.MapFrom(x => x.Personal.MaritalStatus))
                .ForMember(dest => dest.Cf1092, src => src.MapFrom(x => x.Personal.GetDependentType()))
                .ForMember(dest => dest.Cf982, src => src.MapFrom(x => x.Working.Position))
                .ForMember(dest => dest.Cf976, src => src.MapFrom(x => x.Working.CompanyPhone))
                .ForMember(dest => dest.Cf962, src => src.MapFrom(x => x.Working.CompanyName))
                .ForMember(dest => dest.Cf984, src => src.MapFrom(x => x.Working.GetDateStartWork()))
                .ForMember(dest => dest.Cf1226, src => src.MapFrom(x => x.Working.TaxCode))
                .ForMember(dest => dest.Cf970, src => src.MapFrom(x => x.Working.CompanyAddress.Province))
                .ForMember(dest => dest.Cf1286, src => src.MapFrom(x => x.Working.CompanyAddress.District))
                .ForMember(dest => dest.Cf1004, src => src.MapFrom(x => x.Working.CompanyAddress.Ward))
                .ForMember(dest => dest.Cf1006, src => src.MapFrom(x => x.Working.CompanyAddress.Street))
                .ForMember(dest => dest.Cf1020, src => src.MapFrom(x => x.Loan.TotalPaymentsToCreditInstitution))
                .ForMember(dest => dest.Cf1090, src => src.MapFrom(x => x.Loan.NumberOfCreditInstitutionsInDebt))
                .ForMember(dest => dest.Cf1488, src => src.MapFrom(x => x.DisbursementInformation.BankAccount))
                .ForMember(dest => dest.Cf1078, src => src.MapFrom(x => x.DisbursementInformation.SipCode))
                .ForMember(dest => dest.Cf1484, src => src.MapFrom(x => x.DisbursementInformation.BankCode))
                .ForMember(dest => dest.Cf1604, src => src.MapFrom(x => x.DisbursementInformation.Province))
                .ForMember(dest => dest.Cf1070, src => src.MapFrom(x => x.DisbursementInformation.PartnerBranch))
                .ForMember(dest => dest.Cf1612, src => src.MapFrom(x => x.Loan.HadInsuranceInThePast ? "1" : "0"))
                .ForMember(dest => dest.Cf1486, src => src.MapFrom(x => x.DisbursementInformation.BankBranchCode))
                .ForMember(dest => dest.Cf1606, src => src.MapFrom(x => x.DisbursementInformation.PartnerName))
                .ForMember(dest => dest.Cf1618, src => src.MapFrom(x => x.DisbursementInformation.DisbursementMethod))
                .ForMember(dest => dest.Cf1536, src => src.MapFrom(x => x.TemporaryAddress.FixedPhone))
                .ForMember(dest => dest.Cf1028, src => src.MapFrom(x => x.TemporaryAddress.Email))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Loan.Note))
                .ForMember(dest => dest.Cf1018, src => src.MapFrom(x => x.ResidentAddress.Ward))
                .ForMember(dest => dest.Nextstep, src => src.MapFrom(x => x.TemporaryAddress.Ward))
                .ForMember(dest => dest.Cf1012, src => src.MapFrom(x => x.Loan.InsuranceService))
                .ForMember(dest => dest.Cf1244, src => src.MapFrom(x => x.ProductLine))
                .ForMember(dest => dest.Closingdate, src => src.MapFrom(x => $"{x.CreatedDate:yyyy-MM-dd}"))
                .ForMember(dest => dest.Cf1108, src => src.MapFrom(x => $"{x.ModifiedDate:yyyy-MM-dd}"))
                .ForMember(dest => dest.Cf1024, src => src.MapFrom(x => x.Result.Note))
                .ForMember(dest => dest.Cf1630, src => src.MapFrom(x => $"{x.SaleChanelInfo.Code} - {x.SaleChanelInfo.Name}"));


            CreateMap<OCRReceiveDetailResponse, OCRResultDetail>();
            CreateMap<OCRReceiveResponse, OCRResult>()
                .ForMember(dest => dest.Detail, src => src.MapFrom(x => x.JsonContent));

            CreateMap<Customer, CheckInitContractRestRequest>()
                .ForMember(dest => dest.customerName, src => src.MapFrom(x => x.Personal.Name))
                .ForMember(dest => dest.citizenId, src => src.MapFrom(x => x.Personal.IdCard))
                .ForMember(dest => dest.loanAmount, src => src.MapFrom(x => x.Loan.GetAmount()))
                .ForMember(dest => dest.loanTenor, src => src.MapFrom(x => x.Loan.GetTerm()))
                .ForMember(dest => dest.customerIncome, src => src.MapFrom(x => x.Working.GetIncome()))
                .ForMember(dest => dest.dateOfBirth, src => src.MapFrom(x => x.Personal.DateOfBirth))
                .ForMember(dest => dest.gender, src => src.MapFrom(x => x.Personal.Gender == "Nam" ? "M" : x.Personal.Gender == "Nữ" ? "F" : string.Empty))
                .ForMember(dest => dest.issuePlace, src => src.MapFrom(x => x.ResidentAddress.GetFullAddress()))
                .ForMember(dest => dest.issueDateCitizenID, src => src.MapFrom(x => x.Personal.IdCardDate))
                .ForMember(dest => dest.hasInsurance, src => src.MapFrom(x => x.Loan.BuyInsurance == true ? 1 : 0));

            CreateMap<Product, CheckInitContractRestRequest>()
                .ForMember(dest => dest.productId, src => src.MapFrom(x => x.ProductIdMC));

            CreateMap<MAFCBankDto, MAFCBank>();
            CreateMap<MAFCBank, MAFCBankResponse>();

            CreateMap<MAFCSchemeDto, MAFCScheme>();
            CreateMap<MAFCScheme, MAFCSchemeResponse>();

            CreateMap<MAFCSaleOfficeDto, MAFCSaleOffice>();
            CreateMap<MAFCSaleOffice, MAFCSaleOfficeResponse>();

            CreateMap<MAFCCityDto, MAFCCity>();
            CreateMap<MAFCCity, MAFCCityResponse>();

            CreateMap<MAFCDistrictDto, MAFCDistrict>();
            CreateMap<MAFCDistrict, MAFCDistrictResponse>();

            CreateMap<MAFCWardDto, MAFCWard>();
            CreateMap<MAFCWard, MAFCWardResponse>();

            CreateMap<MAFCCheckCustomerRequest, MAFCCheckCustomerRestRequest>()
                .ForMember(dest => dest.CMND, src => src.MapFrom(x => x.SearchVal))
                .ForMember(dest => dest.TaxCode, src => src.MapFrom(x => x.SearchVal))
                .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.SearchVal));
            CreateMap<MAFCCheckCustomerV3Request, MAFCCheckCustomerV3RestRequest>();
            CreateMap<MAFCCheckCustomerRestResponse, MAFCCheckCustomerResponse>();
            CreateMap<KiosModel, MCKios>().ReverseMap();

            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<UpdateCurrentUserRequest, User>();
            CreateMap<UpdateDocCurrentUserRequest, User>();
            CreateMap<UserSendResetPasswordResponse, User>().ReverseMap();
            CreateMap<User, GetUserTeamLeadResponse>();
            CreateMap<User, TeamMemberDto>();
            CreateMap<RegisterUserRequest, User>();
            CreateMap<RegisterUserRequestDto, User>();
            CreateMap<User, RegisterUserResponse>();
            CreateMap<UserSuspensionHistory, UserSuspensionHistoryDto>().ReverseMap();
            CreateMap<User, UserVerifyResponse>()
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Id));
            CreateMap<UserBankInfoDto, UserBankInfo>();
            CreateMap<UserWorkingDto, UserWorking>();
            CreateMap<UserAddressDto, UserAddress>();
            CreateMap<User, ReferrerInfo>();
            CreateMap<User, GetUserReferralResponse>();

            CreateMap<GroupDocument, GroupDocumentDto>().ReverseMap();
            CreateMap<DocumentUpload, DocumentUploadDto>().ReverseMap();
            CreateMap<UploadedMedia, UploadedMediaDto>().ReverseMap();

            CreateMap<Customer, CustomerInfoPdfDto>();
            CreateMap<Personal, PersonalInfoPdfDto>();
            CreateMap<Sale, SaleInfoPdfDto>();
            CreateMap<Address, AddressInfoPdfDto>();
            CreateMap<Working, WorkingInfoPdfDto>();
            CreateMap<Loan, LoanInfoPdfDto>();
            CreateMap<Referee, RefereeInfoPdfDto>();
            CreateMap<BankInfo, BankInfoPdfDto>();
            CreateMap<OtherInfo, OtherInfoPdfDto>();

            CreateMap<DataConfig, GetDataConfigResponse>();

            CreateMap<Role, GetRoleResponse>();
            CreateMap<Permission, PermissionDto>();
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();

            CreateMap<DataConfigModel, DataConfigDto>();
            CreateMap<DataConfigDto, DataConfigModel>();
            CreateMap<DataConfig, DataConfig>()
                .ForMember(dest => dest.Id, src => src.Ignore());

            CreateMap<CreateLeadVibRequest, LeadVib>();
            CreateMap<UpdateLeadVibRequest, LeadVib>();
            CreateMap<LeadVib, GetDetailLeadVibResponse>();
            CreateMap<LeadVib, GetLeadVibResponse>();
            CreateMap<LeadVibAddress, LeadVibAddressDto>().ReverseMap();

            CreateMap<SaleInfoResponse, User>().ReverseMap();
            CreateMap<RestApprovedInfoResponse, MCDebt>()
                .ForMember(dest => dest.HasCourier, src => src.MapFrom(x => x.HasCourier == "0" ? false : true))
                .ForMember(dest => dest.HasInsurrance, src => src.MapFrom(x => x.HasInsurrance == "0" ? false : true));

            CreateMap<CreateBannerRequest, Banner>();
            CreateMap<UpdateBannerRequest, Banner>();
            CreateMap<Banner, GetDetailBannerResponse>();

            CreateMap<GetMCNotiResponse, MCNotificationModel>().ReverseMap();

            CreateMap<News, GetNewsResponse>();
            CreateMap<News, GetDetailNewsResponse>();
            CreateMap<CreateNewsRequest, News>();
            CreateMap<UpdateNewsRequest, News>();
            
            CreateMap<CreateUserSuspensionHistory, UserSuspensionHistory>();
            
            CreateMap<User, Sale>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));

            CreateMap<DataMAFCProcessingModel, DataMAFCProcessingDto>();
            CreateMap<PayloadModel, DataMAFCProcessingPayloadDto>();
            CreateMap<UpdateDataMAFCProcessingRequest, DataMAFCProcessingModel>();

            CreateMap<DataMCProcessing, DataMCProcessingResponse>()
                .ForMember(dest => dest.PayLoad, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<JObject>(src.PayLoad)))
                .ForMember(dest => dest.Response, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<JObject>(src.Response)));

            CreateMap<Permission, Permission>()
                .ForMember(dest => dest.Id, src => src.Ignore());

            CreateMap<User, TeamLeadInfo>();
            CreateMap<POS, PosInfo>();
            CreateMap<User, SaleInfomation>().ReverseMap();
            CreateMap<User, PersonInfo>();
            CreateMap<SaleInfomation, SaleInfomationDto>();
            CreateMap<ReferrerInfo, ReferrerInfoDto>();

            CreateMap<CreateUserSuspended, UserSuspended>();
            CreateMap<UpdateUserSuspended, UserSuspended>();
            CreateMap<CreateUserSuspensionHistory, UserSuspended>();
            CreateMap<TeamLeadInfo, TeamLeadDto>();

            CreateMap<ModelDtos.Otps.SendOtpRequest, OtpGenerationHistory>();
            CreateMap<ModelDtos.Otps.SendOtpRequest, OtpHistory>();
            CreateMap<ModelDtos.Otps.VerifyOtpRequest, OtpHistory>();

            CreateMap<Customer, CheckLoanResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Personal.Name))
                .ForMember(dest => dest.IdCard, opt => opt.MapFrom(src => src.Personal.IdCard))
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.GetFinanceCompany()))
                .ForMember(dest => dest.ReturnReason, opt => opt.MapFrom(src => src.Result.Reason))
                .ForMember(dest => dest.TeamLeadNote, opt => opt.MapFrom(src => src.Result.Note))
                .ForMember(dest => dest.ResultStatus, opt => opt.MapFrom(src => src.Result.ReturnStatus));
        }
    }
}

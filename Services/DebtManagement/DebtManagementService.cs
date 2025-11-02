using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.DebtManagement;
using _24hplusdotnetcore.ModelDtos.File;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.ModelResponses.DebtManagement;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.DebtManagement;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.DebtManagement;
using _24hplusdotnetcore.Services.FCM;
using _24hplusdotnetcore.Services.Files;
using _24hplusdotnetcore.Validators.DebtManagement;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.DebtManagement
{

    public interface IDebtManagementService
    {
        Task<BaseResponse<DebtDetailResponse>> Create(CreateDebtDto model);
        Task<BaseResponse<DebtDetailResponse>> Update(string id, UpdateDebtDto model);
        Task<BaseResponse<DebtDetailResponse>> Delete(string id);
        Task<BaseResponse<DebtDetailResponse>> GetDetail(string id);
        Task<BaseResponse<PagingResponse<DebtDetailResponse>>> GetList(GetDebtDto request, string type);
        Task<BaseResponse<List<ImportDebtDetailResponse>>> ImportExcel(IFormFile file);
        Task<BaseResponse<List<ImportDebtDetailResponse>>> ImportOverDueDate(IFormFile file);

        Task<BaseResponse<FileResponse>> ExportExcelFile(GetDebtDto request);
        Task<BaseResponse<FileResponse>> ExportOverDueDateExcelFile(GetDebtDto request);


    }

    public class DebtManagementService : IDebtManagementService, IScopedLifetime
    {
        private readonly ILogger<DebtManagementService> _logger;
        private readonly IMapper _mapper;
        private readonly IDebtManagementRepository _debtManageRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserServices _userServices;
        private readonly IExcelImportService _excelImport;
        private readonly IImportFileService _importFileService;
        private readonly IPushNotiService _pushNotiService;

        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationServices _notificationServices;

        private readonly UserLoginServices _userLoginServices;

        private readonly IExcelService _excelService;

        public DebtManagementService(ILogger<DebtManagementService> logger,
                   IMapper mapper,
                   IDebtManagementRepository debtManageRepository,
                   IUserLoginService userLoginService,
                   IUserServices userServices,
                   IExcelImportService excelImport,
                   ICustomerRepository customerRepository,
                   IUserRepository userRepository,
                   IImportFileService importFileService,
                   IPushNotiService pushNotiService,
                   UserLoginServices userLoginServices,
                   INotificationServices notificationServices,
                   IExcelService excelService
                   )
        {

            _logger = logger;
            _mapper = mapper;
            _debtManageRepository = debtManageRepository;
            _userLoginService = userLoginService;
            _userServices = userServices;
            _excelImport = excelImport;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _importFileService = importFileService;
            _pushNotiService = pushNotiService;
            _userLoginServices = userLoginServices;
            _notificationServices = notificationServices;
            _excelService = excelService;
        }

        public async Task<BaseResponse<DebtDetailResponse>> Create(CreateDebtDto request)
        {
            var response = new BaseResponse<DebtDetailResponse>();
            var validator = new CreateDebtManageValidation();
            ValidationResult result = validator.Validate(request);
            response.MappingFluentValidation(result);

            if (!response.IsSuccess)
            {
                return response;
            }

            var insertedDebtManage = _mapper.Map<DebtManagementModel>(request);

            var data = await _debtManageRepository.Create(insertedDebtManage);
            response.Data = _mapper.Map<DebtDetailResponse>(data);

            return response;
        }

        public async Task<BaseResponse<DebtDetailResponse>> Delete(string id)
        {
            try
            {
                var result = new BaseResponse<DebtDetailResponse>();

                var detail = await _debtManageRepository.GetDetailAsync(id);

                if (detail == null)
                {
                    return result.ReturnWithMessage(DebtManageMessage.NotFound);
                }

                await _debtManageRepository.Delete(id);

                result.Data = _mapper.Map<DebtDetailResponse>(detail);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<DebtDetailResponse>> GetDetail(string id)
        {
            try
            {
                var result = new BaseResponse<DebtDetailResponse>();

                var detail = await _debtManageRepository.GetDetailAsync(id);

                if (detail == null)
                {
                    return result.ReturnWithMessage(DebtManageMessage.NotFound);
                }

                result.Data = _mapper.Map<DebtDetailResponse>(detail);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<PagingResponse<DebtDetailResponse>>> GetList(GetDebtDto request, string type)
        {
            try
            {
                var filterCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_DebtManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_DebtManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_DebtManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_DebtManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_DebtManagement_ViewAll);

                var response = new BaseResponse<PagingResponse<DebtDetailResponse>>();
                var result = await _debtManageRepository.GetList(request, filterCreatorIds, type);
                var count = await _debtManageRepository.Count(request, filterCreatorIds, type);

                response.Data = new PagingResponse<DebtDetailResponse>
                {
                    Data = _mapper.Map<DebtDetailResponse[]>(result).OrderBy( x => x.ModifiedDate).ThenBy( x=> x.RowNumber).ToList(),
                    TotalRecord = count
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<DebtDetailResponse>> Update(string id, UpdateDebtDto request)
        {
            var response = new BaseResponse<DebtDetailResponse>();
            var validator = new UpdateDebtManageValidation();
            ValidationResult result = validator.Validate(request);
            response.MappingFluentValidation(result);

            if (!response.IsSuccess)
            {
                return response;
            }

            var detail = await _debtManageRepository.GetDetailAsync(id);

            if (detail == null)
            {
                return response.ReturnWithMessage(DebtManageMessage.NotFound);
            }

            var updatedDebtManage = _mapper.Map<DebtManagementModel>(request);

            await _debtManageRepository.UpdateAsync(id, updatedDebtManage);
            response.Data = _mapper.Map<DebtDetailResponse>(detail);

            return response;
        }

        public async Task<BaseResponse<List<ImportDebtDetailResponse>>> ImportExcel(IFormFile file)
        {
            try
            {
                var result = new BaseResponse<List<ImportDebtDetailResponse>>()
                {
                    Data = new List<ImportDebtDetailResponse>()
                };

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        var debts = _excelImport.DrawSheetToObjects<ImportDebtDto>(package.Workbook.Worksheets[0]).ToList();
                        var validator = new ImportDebtValidation();
                        var contractCodeGroup = debts.GroupBy(x => x.ContractCode).ToList();
                        var contractCodes = debts.Select(x => x.ContractCode.Trim()).Distinct().ToList();
                        var saleCodes = debts.Select(x => x.Code.Trim()).Distinct().ToList();
                        var users = (await _userRepository.FilterByAsync(x => saleCodes.Contains(x.UserName))).ToList();

                        foreach (var debt in debts)
                        {
                            var debtDetail = _mapper.Map<ImportDebtDetailResponse>(debt);

                            ValidationResult validateResult = validator.Validate(debt);

                            if (!validateResult.IsValid)
                            {
                                debtDetail.Message = string.Join(", ", validateResult.Errors);
                                debtDetail.IsValid = false;
                            }

                            if (!debtDetail.IdCard.IsEmpty() && !debtDetail.IdCard.IsNumber())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidIdCard;
                            }

                            if (!debtDetail.Phone.IsEmpty() && !debtDetail.Phone.IsNumber())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidPhoneNumber;
                            }

                            if (!debtDetail.DisbursementDate.IsEmpty() && !debtDetail.DisbursementDate.IsValidDateString())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidDisbursementDate;
                            }

                            if (!debtDetail.Period.IsEmpty() && !debtDetail.Period.IsValidDateString())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidPeriod;
                            }

                            if (!debtDetail.PaymentDueDate.IsEmpty() && !debtDetail.PaymentDueDate.IsValidDateString())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidPaymentDueDate;
                            }

                            var isValidAmount = decimal.TryParse(debtDetail.Amount, out decimal amount);

                            if (!debtDetail.Amount.IsEmpty() && !isValidAmount)
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidAmount;
                            }

                            var isDuplicate = contractCodeGroup.Where(x => x.Key == debt.ContractCode).SelectMany(x => x.ToList());

                            if (isDuplicate.Count() > 1)
                            {
                                debtDetail.IsDuplicated = true;
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.DuplicateContractNumber;
                            }

                            if (!users.Any(x => x.UserName == debt.Code))
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.NotFoundSaleCode;
                            }

                            result.Data.Add(debtDetail);
                        }

                        var creator = _userLoginService.GetUserId();
                        var user = await _userRepository.FindOneAsync(x => x.Id == creator);
                        bool isSuccess = result.Data.All(x => x.IsValid);

                        var importFileModel = new ImportFileDto
                        {
                            FileName = file.FileName,
                            Extensions = Path.GetExtension(file.FileName),
                            FileSize = file.Length,
                            ImportType = ImportType.DEBT_MANAGEMENT_COMING,
                            IsSuccess = isSuccess,
                            SaleInfomation = _mapper.Map<SaleInfomation>(user),
                            TotalRecords = result.Data.Count,
                            Creator = creator
                        };

                        _logger.LogInformation("Inserting debt excel file", importFileModel);

                        // save import history
                        var importResult = await _importFileService.Create(importFileModel);

                        if (!isSuccess)
                        {
                            return result.ReturnWithMessage(DebtManageMessage.InvalidExcelData);
                        }

                        int count = 0;
                        // import to database
                        var debtModels = result.Data
                            .Select(x => new DebtManagementModel
                            {
                                RowNumber = ++count,
                                Type = DebtManagementImportType.COMMING.ToString(),
                                ContractCode = x.ContractCode,
                                GreenType = x.GreenType,
                                Personal = new DebtPersonal
                                {
                                    IdCard = x.IdCard,
                                    Name = x.Name,
                                    Phone = x.Phone
                                },
                                Loan = new DebtLoan
                                {
                                    Amount = x.Amount,
                                    DisbursementDate = x.DisbursementDate.ToDate(),
                                    PaymentDueDate = x.PaymentDueDate.ToDate(),
                                    Period = x.Period.ToDate(),
                                    Term = x.Term
                                },
                                SaleInfo = new DebtSaleInfo
                                {
                                    Code = x.Code,
                                    FullName = users.First(y => y.UserName == x.Code).FullName,
                                    UserName = users.First(y => y.UserName == x.Code).UserName,
                                    Id = users.First(y => y.UserName == x.Code).Id,
                                    Pos = new DebtPosInfo
                                    {
                                        Id = users.First(y => y.UserName == x.Code).PosInfo?.Id,
                                        Name = users.First(y => y.UserName == x.Code).PosInfo?.Name,
                                    },
                                    TeamLead = new DebtTeamleadInfo
                                    {
                                        Id = users.First(y => y.UserName == x.Code).TeamLeadInfo?.Id,
                                        FullName = users.First(y => y.UserName == x.Code).TeamLeadInfo?.FullName,
                                        UserName = users.First(y => y.UserName == x.Code).TeamLeadInfo?.UserName,
                                    }
                                },
                                ModifierInfo = new DebtSaleInfo
                                {
                                    FullName = user?.FullName,
                                    UserName = user?.UserName,
                                    Id = user?.Id
                                },
                                ModifiedDate = DateTime.Now,
                                Modifier = creator,
                                ActualUpdatedDate = x.ActualUpdatedDate,
                                IsValid = x.IsValid,
                                IsDuplicated = x.IsDuplicated,
                                Message = x.Message
                            }).ToList();

                        var response = await _debtManageRepository.Import(debtModels);
                        var groupSale = response.Where(x => !x.Loan.Amount.IsEmpty() && decimal.Parse(x.Loan.Amount) > 0).GroupBy(x => x.SaleInfo.Code);

                        // send notification to online sales
                        foreach (var item in groupSale)
                        {
                            var saveRecord = item.ToList().First();

                            var objNoti = new Notification
                            {
                                GreenType = GreenType.Debt,
                                RecordId = saveRecord.SaleInfo.Id,
                                Type = NotificationType.DebtComing,
                                UserName = saveRecord.SaleInfo.UserName,
                                UserId = saveRecord.SaleInfo.Id,
                                Title = DebtManageMessage.DebtMessageTitle,
                                Message = DebtManageMessage.DebtMessageBody,
                            };
                            await _notificationServices.CreateOneAsync(objNoti);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<FileResponse>> ExportOverDueDateExcelFile(GetDebtDto request)
        {
            try
            {
                var response = new BaseResponse<FileResponse>();

                var result = new FileResponse
                {
                    ContentType = "application/vnd.ms-excel",
                };

                var filterCreatorIds = await _userServices.GetMemberByPermission(
                                                    PermissionCost.AdminPermission.Admin_DebtManagement_ViewAll,
                                                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_DebtManagement_ViewAll,
                                                    PermissionCost.PosLeadPermission.PosLead_DebtManagement_ViewAll,
                                                    PermissionCost.AsmPermission.Asm_DebtManagement_ViewAll,
                                                    PermissionCost.TeamLeaderPermission.TeamLeader_DebtManagement_ViewAll);

                var data = await _debtManageRepository.GetExportData(request, filterCreatorIds, DebtManagementImportType.OVERDUE.ToString());

                var exportData = data.Select(x => new ExportOverDueDateDebt
                {
                    Code = x.SaleInfo.Code,
                    ActualUpdatedDate = x.ActualUpdatedDate,
                    Amount = x.Loan.Amount.ToDecimal(),
                    ContractCode = x.ContractCode,
                    DisbursementDate = x.Loan.DisbursementDate,
                    GreenType = x.GreenType,
                    IdCard = x.Personal.IdCard,
                    Name = x.Personal.Name,
                    PaymentDueDate = x.Loan.PaymentDueDate,
                    Period = x.Loan.Period,
                    Phone = x.Personal.Phone,
                    Term = x.Loan.Term,
                    FirstReferee = x.Personal?.FirstReferee,
                    SecondReferee = x.Personal?.SecondReferee,
                    NumberOverDueDate = x.NumberOverDueDate,
                    OverDueDate = x.OverDueDate
                })
                .OrderBy(x => x.ModifiedDate)
                .ThenBy(x => x.RowNumber)
                .ToList();

                result.FileContents = _excelService.Generate("Templates/OVERDUEDATE_TEMPLATE.xlsx", exportData);
                result.FileName = string.Format("OVERDUEDATE_DEBT_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
                response.Data = result;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<FileResponse>> ExportExcelFile(GetDebtDto request)
        {
            try
            {
                var response = new BaseResponse<FileResponse>();

                var result = new FileResponse
                {
                    ContentType = "application/vnd.ms-excel",
                };

                var filterCreatorIds = await _userServices.GetMemberByPermission(
                                    PermissionCost.AdminPermission.Admin_DebtManagement_ViewAll,
                                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_DebtManagement_ViewAll,
                                    PermissionCost.PosLeadPermission.PosLead_DebtManagement_ViewAll,
                                    PermissionCost.AsmPermission.Asm_DebtManagement_ViewAll,
                                    PermissionCost.TeamLeaderPermission.TeamLeader_DebtManagement_ViewAll);

                var data = await _debtManageRepository.GetExportData(request, filterCreatorIds, DebtManagementImportType.COMMING.ToString());

                var exportData = data.Select(x => new ExportDebtManagement
                {
                    Code = x.SaleInfo.Code,
                    ActualUpdatedDate = x.ActualUpdatedDate,
                    Amount = x.Loan.Amount.ToDecimal(),
                    ContractCode = x.ContractCode,
                    DisbursementDate = x.Loan.DisbursementDate,
                    GreenType = x.GreenType,
                    IdCard = x.Personal.IdCard,
                    Name = x.Personal.Name,
                    PaymentDueDate = x.Loan.PaymentDueDate,
                    Period = x.Loan.Period,
                    Phone = x.Personal.Phone,
                    Term = x.Loan.Term
                })
                .OrderBy(x => x.ModifiedDate)
                .ThenBy(x => x.RowNumber)
                .ToList();

                result.FileContents = _excelService.Generate("Templates/DEBT_MANAGEMENT_TEMPLATE.xlsx", exportData);
                result.FileName = string.Format("DEBT_MANAGEMENT_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
                response.Data = result;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<List<ImportDebtDetailResponse>>> ImportOverDueDate(IFormFile file)
        {
            try
            {
                var result = new BaseResponse<List<ImportDebtDetailResponse>>()
                {
                    Data = new List<ImportDebtDetailResponse>()
                };

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        var debts = _excelImport.DrawSheetToObjects<ImportOverDueDateDto>(package.Workbook.Worksheets[0]).ToList();
                        var validator = new ImportOverDueDateValidation();
                        var contractCodeGroup = debts.GroupBy(x => x.ContractCode).ToList();
                        var contractCodes = debts.Select(x => x.ContractCode.Trim()).Distinct().ToList();
                        var saleCodes = debts.Select(x => x.Code.Trim()).Distinct().ToList();
                        var users = (await _userRepository.FilterByAsync(x => saleCodes.Contains(x.UserName))).ToList();

                        foreach (var debt in debts)
                        {
                            var debtDetail = _mapper.Map<ImportDebtDetailResponse>(debt);

                            ValidationResult validateResult = validator.Validate(debt);

                            if (!validateResult.IsValid)
                            {
                                debtDetail.Message = string.Join(", ", validateResult.Errors);
                                debtDetail.IsValid = false;
                            }

                            if (!debtDetail.IdCard.IsEmpty() && !debtDetail.IdCard.IsNumber())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidIdCard;
                            }

                            if (!debtDetail.Phone.IsEmpty() && !debtDetail.Phone.IsNumber())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidPhoneNumber;
                            }

                            if (!debtDetail.DisbursementDate.IsEmpty() && !debtDetail.DisbursementDate.IsValidDateString())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidDisbursementDate;
                            }

                            if (!debtDetail.Period.IsEmpty() && !debtDetail.Period.IsValidDateString())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidPeriod;
                            }

                            if (!debtDetail.OverDueDate.IsEmpty() && !debtDetail.OverDueDate.IsValidDateString())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidOverDueDate;
                            }

                            if (!debtDetail.NumberOverDueDate.IsEmpty() && !debtDetail.NumberOverDueDate.IsNumber())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidNumberOverDueDate;
                            }

                            if (!debtDetail.PaymentDueDate.IsEmpty() && !debtDetail.PaymentDueDate.IsValidDateString())
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidPaymentDueDate;
                            }

                            var isValidAmount = decimal.TryParse(debtDetail.Amount, out decimal amount);

                            if (!debtDetail.Amount.IsEmpty() && !isValidAmount)
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.InvalidAmount;
                            }

                            var isDuplicate = contractCodeGroup.Where(x => x.Key == debt.ContractCode).SelectMany(x => x.ToList());

                            if (isDuplicate.Count() > 1)
                            {
                                debtDetail.IsDuplicated = true;
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.DuplicateContractNumber;
                            }

                            if (!users.Any(x => x.UserName == debt.Code))
                            {
                                debtDetail.IsValid = false;
                                debtDetail.Message = DebtManageMessage.NotFoundSaleCode;
                            }

                            result.Data.Add(debtDetail);
                        }

                        var creator = _userLoginService.GetUserId();
                        var user = await _userRepository.FindOneAsync(x => x.Id == creator);
                        bool isSuccess = result.Data.All(x => x.IsValid);

                        var importFileModel = new ImportFileDto
                        {
                            FileName = file.FileName,
                            Extensions = Path.GetExtension(file.FileName),
                            FileSize = file.Length,
                            ImportType = ImportType.DEBT_MANAGEMENT_OVERDUE,
                            IsSuccess = isSuccess,
                            SaleInfomation = _mapper.Map<SaleInfomation>(user),
                            TotalRecords = result.Data.Count,
                            Creator = creator
                        };

                        _logger.LogInformation("Inserting over due date excel file", importFileModel);

                        // save import history
                        var importResult = await _importFileService.Create(importFileModel);

                        if (!isSuccess)
                        {
                            return result.ReturnWithMessage(DebtManageMessage.InvalidExcelData);
                        }

                        int count = 0;
                        // import to database
                        var debtModels = result.Data
                            .Select(x => new DebtManagementModel
                            {
                                RowNumber = ++count,
                                Type = DebtManagementImportType.OVERDUE.ToString(),
                                ContractCode = x.ContractCode,
                                GreenType = x.GreenType,
                                Personal = new DebtPersonal
                                {
                                    IdCard = x.IdCard,
                                    Name = x.Name,
                                    Phone = x.Phone,
                                    FirstReferee = x.FirstReferee,
                                    SecondReferee = x.SecondReferee
                                },
                                Loan = new DebtLoan
                                {
                                    Amount = x.Amount,
                                    DisbursementDate = x.DisbursementDate.ToDate(),
                                    PaymentDueDate = x.PaymentDueDate.ToDate(),
                                    Period = x.Period.ToDate(),
                                    Term = x.Term
                                },
                                SaleInfo = new DebtSaleInfo
                                {
                                    Code = x.Code,
                                    FullName = users.First(y => y.UserName == x.Code).FullName,
                                    UserName = users.First(y => y.UserName == x.Code).UserName,
                                    Id = users.First(y => y.UserName == x.Code).Id,
                                    Pos = new DebtPosInfo
                                    {
                                        Id = users.First(y => y.UserName == x.Code).PosInfo?.Id,
                                        Name = users.First(y => y.UserName == x.Code).PosInfo?.Name,
                                    },
                                    TeamLead = new DebtTeamleadInfo
                                    {
                                        Id = users.First(y => y.UserName == x.Code).TeamLeadInfo?.Id,
                                        FullName = users.First(y => y.UserName == x.Code).TeamLeadInfo?.FullName,
                                        UserName = users.First(y => y.UserName == x.Code).TeamLeadInfo?.UserName,
                                    }
                                },
                                ModifierInfo = new DebtSaleInfo
                                {
                                    FullName = user?.FullName,
                                    UserName = user?.UserName,
                                    Id = user?.Id
                                },
                                OverDueDate = x.OverDueDate.ToDate(),
                                NumberOverDueDate = x.NumberOverDueDate.ToNumber(),
                                ModifiedDate = DateTime.Now,
                                Modifier = creator,
                                ActualUpdatedDate = x.ActualUpdatedDate,
                                IsValid = x.IsValid,
                                IsDuplicated = x.IsDuplicated,
                                Message = x.Message

                            }).ToList();

                        var response = await _debtManageRepository.ImportOverDueDate(debtModels);
                        var groupSale = response.Where(x => !x.Loan.Amount.IsEmpty() && decimal.Parse(x.Loan.Amount) > 0).GroupBy(x => x.SaleInfo.Code);

                        // send notification to online sales
                        foreach (var item in groupSale)
                        {
                            var saveRecord = item.ToList().First();

                            var objNoti = new Notification
                            {
                                GreenType = GreenType.Debt,
                                RecordId = saveRecord.SaleInfo.Id,
                                Type = NotificationType.DebtOverDue,
                                UserName = saveRecord.SaleInfo.UserName,
                                UserId = saveRecord.SaleInfo.Id,
                                Title = DebtManageMessage.DebtOverDueMessageTitle,
                                Message = DebtManageMessage.DebtOverDueMessageBody,
                            };
                            await _notificationServices.CreateOneAsync(objNoti);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}


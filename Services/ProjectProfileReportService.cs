using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.File;
using _24hplusdotnetcore.ModelDtos.ProjectProfileReports;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.Files;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IProjectProfileReportService
    {
        Task<ProjectProfileReportImportResponse> ImportAsync(IFormFile formFile);
        Task<PagingResponse<ProjectProfileReportResponse>> GetAsync(PagingRequest pagingRequest);
        Task<FileResponse> ExportAsync(string projectProfileReportId);
        Task<ProjectProfileReportDetailResponse> GetDetailAsync(string id);
    }

    public class ProjectProfileReportService : IProjectProfileReportService, IScopedLifetime
    {
        private const string _sheetName = "(2)";
        private readonly ILogger<ProjectProfileReportService> _logger;
        private readonly IExcelImportService _excelImportService;
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IMapper _mapper;
        private readonly IProjectProfileReportRepository _projectProfileReportRepository;
        private readonly IMongoRepository<ProjectProfileReportDetail> _projectProfileReportDetailRepository;
        private readonly ISaleChanelRepository _saleChanelRepository;
        private readonly IMongoRepository<POS> _posRepository;
        private readonly IUserServices _userService;
        private readonly IExcelService _excelService;

        public ProjectProfileReportService(
            ILogger<ProjectProfileReportService> logger,
            IExcelImportService excelImportService,
            IUserRepository userRepository,
            IUserLoginService userLoginService,
            IMapper mapper,
            IProjectProfileReportRepository projectProfileReportRepository,
            IMongoRepository<ProjectProfileReportDetail> projectProfileReportDetailRepository,
            ISaleChanelRepository saleChanelRepository,
            IMongoRepository<POS> posRepository,
            IUserServices userService,
            IExcelService excelService)
        {
            _logger = logger;
            _excelImportService = excelImportService;
            _userRepository = userRepository;
            _userLoginService = userLoginService;
            _mapper = mapper;
            _projectProfileReportRepository = projectProfileReportRepository;
            _projectProfileReportDetailRepository = projectProfileReportDetailRepository;
            _saleChanelRepository = saleChanelRepository;
            _posRepository = posRepository;
            _userService = userService;
            _excelService = excelService;
        }

        public async Task<ProjectProfileReportImportResponse> ImportAsync(IFormFile formFile)
        {
            try
            {
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using MemoryStream stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == _sheetName);
                if (worksheet == null)
                {
                    return new ProjectProfileReportImportResponse { IsSuccess = false, ErrorMessage = string.Format(Message.SHEET_NOT_FOUND, _sheetName) };
                }

                var data = _excelImportService.DrawSheetToObjects<ProjectProfileReportImportFileData>(worksheet, startRow: 1, false).ToList();
                if (!data.Any())
                {
                    return new ProjectProfileReportImportResponse { IsSuccess = false, ErrorMessage = Message.HAS_NO_DATA };
                }
                var listSaleCodes = data.Select(x => x.SaleCode);
                var users = await _userRepository.FilterByAsync(x => listSaleCodes.Contains(x.UserName));

                var saleDict = users.GroupBy(x => x.UserName).ToDictionary(x => x.Key, x => x.First());

                var projectProfileReportDetails = new List<ProjectProfileReportDetail>();
                var rowErrors = new List<ProjectProfileErrorDto>();
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    var errorColumnNames = new List<string>();

                    var projectProfileReportDetail = _mapper.Map<ProjectProfileReportDetail>(item);

                    DateTime.TryParse(projectProfileReportDetail.AppCreationDate, out DateTime appCreationDate);
                    projectProfileReportDetail.AppCreationDate = $"{appCreationDate.Day}/{appCreationDate.Month}/{appCreationDate.Year}";

                    DateTime.TryParse(projectProfileReportDetail.LastStatusChangeDate, out DateTime lastStatusChangeDate);
                    projectProfileReportDetail.LastStatusChangeDate = $"{lastStatusChangeDate.Day}/{lastStatusChangeDate.Month}/{lastStatusChangeDate.Year}";

                    if (saleDict.TryGetValue(item.SaleCode, out User sale))
                    {
                        projectProfileReportDetail.SaleInfomation = _mapper.Map<SaleInfomation>(sale);
                        projectProfileReportDetail.PosInfo = _mapper.Map<PosInfo>(sale.PosInfo);
                        projectProfileReportDetail.SaleChanelInfo = _mapper.Map<SaleChanelInfo>(sale.SaleChanelInfo);
                        projectProfileReportDetail.AsmInfo = _mapper.Map<TeamLeadInfo>(sale.AsmInfo);
                        if (sale.RoleName == "TL")
                        {
                            projectProfileReportDetail.TeamLeadInfo = _mapper.Map<TeamLeadInfo>(sale);
                        }
                        else
                        {
                            projectProfileReportDetail.TeamLeadInfo = _mapper.Map<TeamLeadInfo>(sale.TeamLeadInfo);
                        }
                    }
                    else
                    {
                        errorColumnNames.Add("Không tìm thấy Sale Code: " + item.SaleCode);
                    }

                    if (errorColumnNames.Any())
                    {
                        rowErrors.Add(new ProjectProfileErrorDto
                        {
                            RowNo = i + 2,
                            CustomerName = item.CustomerName,
                            ErrorColumnNames = errorColumnNames
                        });
                    }
                    else
                    {
                        projectProfileReportDetails.Add(projectProfileReportDetail);
                    }
                }

                if (rowErrors.Any())
                {
                    return new ProjectProfileReportImportResponse { IsSuccess = false, Errors = rowErrors };
                }

                var userId = _userLoginService.GetUserId();
                var user = await _userRepository.FindByIdAsync(userId);

                var projectProfileReport = new ProjectProfileReport
                {
                    Creator = userId,
                    FileName = formFile.FileName,
                    SaleInfomation = _mapper.Map<SaleInfomation>(user),
                    TotalRecords = data.Count,
                    TotalSuccessfulRecords = projectProfileReportDetails.Count,
                    TotalFailedRecords = data.Count - projectProfileReportDetails.Count,
                };
                await _projectProfileReportRepository.InsertOneAsync(projectProfileReport);
                projectProfileReportDetails.ForEach(x => x.ProjectProfileReportId = projectProfileReport.Id);
                await _projectProfileReportDetailRepository.InsertManyAsync(projectProfileReportDetails);

                return new ProjectProfileReportImportResponse { IsSuccess = true, ProjectProfileReportId = projectProfileReport.Id };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<ProjectProfileReportResponse>> GetAsync(PagingRequest pagingRequest)
        {
            try
            {
                var data = await _projectProfileReportRepository.GetAsync(pagingRequest.TextSearch, pagingRequest.PageIndex, pagingRequest.PageSize);

                var total = await _projectProfileReportRepository.CountAsync(pagingRequest.TextSearch);

                var result = new PagingResponse<ProjectProfileReportResponse>
                {
                    TotalRecord = total,
                    Data = data
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ProjectProfileReportDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var data = await _projectProfileReportRepository.GetDetailAsync(id);

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<FileResponse> ExportAsync(string projectProfileReportId)
        {
            try
            {
                var filterByCreators = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_ProjectProfileReport_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_ProjectProfileReport_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_ProjectProfileReport_ViewAll,
                    PermissionCost.AsmPermission.Asm_ProjectProfileReport_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_ProjectProfileReport_ViewAll);

                var projectProfileReportDetails = await _projectProfileReportDetailRepository.FilterByAsync(x => x.ProjectProfileReportId == projectProfileReportId &&
                    (!filterByCreators.Any() || filterByCreators.Contains(x.SaleInfomation.Id)));

                var models = _mapper.Map<IEnumerable<ProjectProfileReportFileExport>>(projectProfileReportDetails);

                var result = new FileResponse
                {
                    ContentType = "application/vnd.ms-excel",
                    FileContents = _excelService.Generate("Templates/Template_Report_Project.xlsx", models.ToList(), workSheetName: _sheetName),
                    FileName = string.Format("Báo cáo hồ sơ dự án_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"))
                };

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

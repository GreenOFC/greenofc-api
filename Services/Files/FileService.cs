using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.File;
using _24hplusdotnetcore.ModelDtos.StorageModels;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.Storage;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Files
{
    public interface IFileService
    {
        Task<FileResponse> DownloadAsync(FileRequestDto fileRequest);
        Task<StorageFileResponse> GenarateDNFile(string customerId);
    }
    public class FileService : IFileService, IScopedLifetime
    {
        private readonly ILogger<FileService> _logger;
        private readonly IPdfService _pdfService;
        private readonly CustomerQueryService _customerQueryService;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private readonly IExcelService _excelService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;

        public FileService(
            ILogger<FileService> logger,
            IPdfService pdfService,
            CustomerQueryService customerQueryService,
            IMapper mapper,
            IStorageService storageService,
            IExcelService excelService,
            ICustomerRepository customerRepository,
            IUserLoginService userLoginService,
            IUserRepository userRepository)
        {
            _logger = logger;
            _pdfService = pdfService;
            _customerQueryService = customerQueryService;
            _mapper = mapper;
            _storageService = storageService;
            _excelService = excelService;
            _customerRepository = customerRepository;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
        }

        public async Task<FileResponse> DownloadAsync(FileRequestDto fileRequest)
        {
            try
            {
                var result = new FileResponse
                {
                    ContentType = "application/pdf",
                };

                switch (fileRequest.Type)
                {
                    case FileType.CashLoan:
                        {
                            Customer customer = await _customerQueryService.GetCustomerAsync(fileRequest.customerId);
                            if (customer == null)
                            {
                                throw new ArgumentException(Common.Message.CUSTOMER_NOT_FOUND);
                            }
                            result.FileContents = _pdfService.GenerateCashLoan(customer);
                            result.FileName = string.Format("cash_loan_infomation_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
                            break;
                        }
                    case FileType.InfomationCollectionForm:
                        {
                            Customer customer = await _customerQueryService.GetCustomerAsync(fileRequest.customerId);
                            if (customer == null)
                            {
                                throw new ArgumentException(Common.Message.CUSTOMER_NOT_FOUND);
                            }
                            var customerDto = _mapper.Map<CustomerInfoPdfDto>(customer);
                            result.FileContents = _pdfService.GenerateInfomationCollectionForm(customerDto);
                            result.FileName = string.Format("DN_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
                            await _storageService.UploadFileAsync(fileRequest.customerId, result.FileName, result.FileContents);
                            break;
                        }
                    case FileType.TelesalesMa:
                        {
                            var models = GetTelesalesMas();
                            result.FileContents = _excelService.Generate("Templates/TELESALES_MA.xlsx", models);
                            result.FileName = string.Format("TELESALES_MA_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
                            await _storageService.UploadFileAsync(fileRequest.customerId, result.FileName, result.FileContents);
                            break;
                        }
                    case FileType.Cimb:
                        {
                            var filterByCreatorIds = new List<string>();
                            if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllEc))
                            {
                                filterByCreatorIds.Add(_userLoginService.GetUserId());
                                var teamMembers = _userRepository.FilterBy(x => x.TeamLeadInfo.Id == _userLoginService.GetUserId());
                                filterByCreatorIds.AddRange(teamMembers.Select(x => x.Id));
                            }
                            var models = await _customerRepository.GetExportAsync(GreenType.GreenG, filterByCreatorIds);
                            result.FileContents = _excelService.Generate("Templates/CIMB.xlsx", models?.ToList());
                            result.FileName = string.Format("CIMB_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
                            await _storageService.UploadFileAsync(fileRequest.customerId, result.FileName, result.FileContents);
                            break;
                        }
                    default:
                        throw new NotImplementedException($"Type {fileRequest.Type} is not implemented");
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<StorageFileResponse> GenarateDNFile(string customerId)
        {
            try
            {
                Customer customer = await _customerQueryService.GetCustomerAsync(customerId);
                if (customer == null)
                {
                    throw new ArgumentException(Common.Message.CUSTOMER_NOT_FOUND);
                }
                var customerDto = _mapper.Map<CustomerInfoPdfDto>(customer);
                var fileContents = _pdfService.GenerateInfomationCollectionForm(customerDto);
                string fileName = string.Format("DN_{0}.pdf", DateTime.Now.ToString("yyyyMMddHHmmss"));
                return await _storageService.UploadFileAsync(customerId, fileName, fileContents);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private IList<ExportTelesalesMaModel> GetTelesalesMas()
        {
            return new List<ExportTelesalesMaModel>
                {
                    new ExportTelesalesMaModel
                    {
                        ModifiedTime = DateTime.UtcNow,
                        CreatedTime = DateTime.UtcNow,
                        SalesName = "TRẦN TRỌNG NGHĨA",
                        SalesCode = "GRC09191CMA",
                        HasAdmin = true,
                        FinalHouse = "MIRAE ASSET",
                        CustomerName = "TRƯƠNG THỊ LINH EM",
                        CustomerPhone = "0332938008",
                        CustomerIdentityCard = "351944884",
                        ProductType = "Employee Cash Loan",
                        ProductCode = "EMPLOYEE NON-AT 211 - MT",
                        ProfileCode = "MA-1890369",
                        HasInsurance = true,
                        LoanAmount = 52750000,
                        SalesResult = "POR",
                        ProfileStatus = "RETURN",
                        ReasonReturnApplication = "D6.0 | F1 NHẬP SAI SỐ ĐIỆN THOẠI THAM CHIẾU 1 THEO APP MỚI ; S1 | ĐÃ CẬP NHẬT THEO Y/C CỦA SALE; D6.0 | NHẬP LẠI ĐÚNG SỐ ĐIỆN THOẠI THAM CHIẾU THEO APP MỚI; S1 | Đã bổ sung"
                    },
                    new ExportTelesalesMaModel
                    {
                        ModifiedTime = DateTime.UtcNow,
                        CreatedTime = DateTime.UtcNow,
                        SalesName = "TRẦN TRỌNG NGHĨA 2",
                        SalesCode = "GRC09191CMA",
                        HasAdmin = true,
                        FinalHouse = "MIRAE ASSET",
                        CustomerName = "TRƯƠNG THỊ LINH EM 4",
                        CustomerPhone = "0332938008",
                        CustomerIdentityCard = "351944884",
                        ProductType = "Employee Cash Loan",
                        ProductCode = "EMPLOYEE NON-AT 211 - MT 5",
                        ProfileCode = "MA-1890369",
                        HasInsurance = true,
                        LoanAmount = 52750000,
                        SalesResult = "POR",
                        ProfileStatus = "RETURN",
                        ReasonReturnApplication = "D6.0 | F1 NHẬP SAI SỐ ĐIỆN THOẠI THAM CHIẾU 1 THEO APP MỚI ; S1 | ĐÃ CẬP NHẬT THEO Y/C CỦA SALE; D6.0 | NHẬP LẠI ĐÚNG SỐ ĐIỆN THOẠI THAM CHIẾU THEO APP MỚI; S1 | Đã bổ sung"
                    }
                };
        }
    }
}

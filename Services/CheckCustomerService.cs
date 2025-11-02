using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckCustomers;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Services.PtfOmnis;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ICheckCustomerService
    {
        Task CheckAsync(IFormFile formFile);

        Task CheckAsync(string fileId);

        Task<PagingResponse<CheckCustomerResponse>> GetAsync(PagingRequest pagingRequest);

        Task<PagingResponse<CheckCustomerDetailResponse>> GetDetailAsync(string fileId, PagingRequest pagingRequest);
    }
    public class CheckCustomerService : ICheckCustomerService, IScopedLifetime
    {
        private readonly ILogger<CheckCustomerService> _logger;
        private readonly IUserLoginService _userLoginService;
        private readonly IMAFCCheckCustomerService _mAFCCheckCustomerService;
        private readonly CheckInfoServices _checkInfoServices;
        private readonly IPtfOmniService _ptfOmniService;
        private readonly ICheckCustomerDetailRepository _checkCustomerDetailRepository;
        private readonly ICheckCustomerRepository _checkCustomerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserServices _userService;

        public CheckCustomerService(
            ILogger<CheckCustomerService> logger,
            IUserLoginService userLoginService,
            IMAFCCheckCustomerService mAFCCheckCustomerService,
            CheckInfoServices checkInfoServices,
            IPtfOmniService ptfOmniService,
            ICheckCustomerDetailRepository checkCustomeDetailRepository,
            ICheckCustomerRepository checkCustomerRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IUserServices userService)
        {
            _logger = logger;
            _userLoginService = userLoginService;
            _mAFCCheckCustomerService = mAFCCheckCustomerService;
            _checkInfoServices = checkInfoServices;
            _ptfOmniService = ptfOmniService;
            _checkCustomerDetailRepository = checkCustomeDetailRepository;
            _checkCustomerRepository = checkCustomerRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task CheckAsync(IFormFile formFile)
        {
            try
            {
                var idCards = await GetIdCardAsync(formFile);
                if(idCards.Any(x => x.Length != 9 && x.Length != 12)) throw new ArgumentException(Message.ID_CARD_INVALID);

                var userId = _userLoginService.GetUserId();
                var user = await _userRepository.FindByIdAsync(userId);
                
                var checkCustomer = new CheckCustomer
                {
                    Creator = userId,
                    FileName = formFile.FileName,
                    SaleInfomation = _mapper.Map<SaleInfomation>(user),
                    TotalIdCards = idCards.Count()
                };
                await _checkCustomerRepository.InsertOneAsync(checkCustomer);
                
                var histories = idCards.Select(idCard => new CheckCustomerDetail
                {
                    Creator = userId,
                    FileId = checkCustomer.Id,
                    IdCard = idCard
                }).ToList();
                await _checkCustomerDetailRepository.InsertManyAsync(histories);

                BackgroundJob.Enqueue<ICheckCustomerService>(x => x.CheckAsync(checkCustomer.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CheckAsync(string fileId)
        {
            try
            {
                _logger.LogInformation($"[CheckCustomerService] - [CheckAsync] start");

                var checkCustomerHistories = await _checkCustomerDetailRepository.FilterByAsync(x => x.FileId == fileId);

                foreach (var item in checkCustomerHistories)
                {
                    _logger.LogInformation($"[CheckCustomerService] - [CheckAsync] - check: {item.IdCard}");

                    var result = await CheckIdCardAsync(item.IdCard);
                    item.ModifiedDate = DateTime.Now;

                    var update = Builders<CheckCustomerDetail>.Update
                        .Set(x => x.ModifiedDate, DateTime.Now)
                        .Set(x => x.Results, result);
                    await _checkCustomerDetailRepository.UpdateOneAsync(x => x.Id == item.Id, update);
                    await Task.Delay(30000);
                }

                _logger.LogInformation($"[CheckCustomerService] - [CheckAsync] end");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<PagingResponse<CheckCustomerResponse>> GetAsync(PagingRequest pagingRequest)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_CheckCustomer_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_CheckCustomer_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_CheckCustomer_ViewAll,
                    PermissionCost.AsmPermission.Asm_CheckCustomer_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_CheckCustomer_ViewAll);

                var checkCustomer = await _checkCustomerRepository.GetAsync(pagingRequest.TextSearch, filterByCreatorIds, pagingRequest.PageIndex, pagingRequest.PageSize);

                var total = await _checkCustomerRepository.CountAsync(pagingRequest.TextSearch, filterByCreatorIds);

                var result = new PagingResponse<CheckCustomerResponse>
                {
                    TotalRecord = total,
                    Data = checkCustomer
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<CheckCustomerDetailResponse>> GetDetailAsync(string fileId, PagingRequest pagingRequest)
        {
            try
            {
                var checkCustomer = await _checkCustomerDetailRepository.GetAsync(pagingRequest.TextSearch, fileId, pagingRequest.PageIndex, pagingRequest.PageSize);

                var total = await _checkCustomerDetailRepository.CountAsync(pagingRequest.TextSearch, fileId);

                var result = new PagingResponse<CheckCustomerDetailResponse>
                {
                    TotalRecord = total,
                    Data = checkCustomer
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<IEnumerable<string>> GetIdCardAsync(IFormFile formFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using MemoryStream stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
            var rowCount = worksheet.Dimension.Rows;

            var idCards = new List<string>();
            for (int row = 2; row <= rowCount; row++)
            {
                var idCard = worksheet.Cells[row, 1].Value?.ToString().Trim();
                if (!string.IsNullOrEmpty(idCard)) idCards.Add(idCard);
            }
            return idCards;
        }

        private async Task<IEnumerable<CheckCustomerResult>> CheckIdCardAsync(string idCard)
        {
            var mafcTask = CheckMafcAsync(idCard);
            // var mcTask = CheckMcAsync(idCard);
            var ptfTask = CheckPtfAsync(idCard);
            await Task.WhenAll(mafcTask, ptfTask);

            return new List<CheckCustomerResult>
            {
                new CheckCustomerResult
                {
                    GreenType = GreenType.GreenA,
                    Result = mafcTask.Result
                },
                // new CheckCustomerResult
                // {
                //     GreenType = GreenType.GreenC,
                //     Result = mcTask.Result
                // },
                new CheckCustomerResult
                {
                    GreenType = GreenType.GreenP,
                    Result = ptfTask.Result
                },
            };
        }

        private async Task<dynamic> CheckMafcAsync(string idCard)
        {
            try
            {
                var result = await _mAFCCheckCustomerService.CheckCustomerAsync(new MAFCCheckCustomerRequest { SearchVal = idCard, Partner = MAFCDataEntry.UserId });
                return result;
            }
            catch (Refit.ApiException ex)
            {
                return new { ex.Content };
            }
            catch (Exception ex)
            {
                return new { ex.Message };
            }
        }

        private async Task<dynamic> CheckMcAsync(string idCard)
        {
            try
            {
                var result = await _checkInfoServices.CheckCitizendAsync(idCard, HistoryCallApiAction.ToolsCheckIdCard);
                return result;
            }
            catch (Refit.ApiException ex)
            {
                return new { ex.Content };
            }
            catch (Exception ex)
            {
                return new { ex.Message };
            }
        }

        private async Task<dynamic> CheckPtfAsync(string idCard)
        {
            try
            {
                var result = await _ptfOmniService.CheckValidLoanAsync(new PtfOmniCheckValidLoanRequest { IdCards = new[] { idCard } });
                return result;
            }
            catch (Refit.ApiException ex)
            {
                return new { ex.Content };
            }
            catch (Exception ex)
            {
                return new { ex.Message };
            }
        }
    }
}

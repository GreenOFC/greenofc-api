using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadOkVays;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadOkVayService
    {
        Task<PagingResponse<GetLeadOkVayResponse>> GetAsync(GetLeadOkVayRequest getLeadOkVayRequest);
        Task CreateAsync(CreateLeadOkVayRequest createLeadOkVayRequest);
        Task UpdateAsync(string id, UpdateLeadOkVayRequest updateLeadOkVayRequest);
        Task<GetDetailLeadOkVayResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
        Task MarkApproveAsync(string id);
        Task MarkRejectAsync(string id, string reason);
    }

    public class LeadOkVayService : ILeadOkVayService, IScopedLifetime
    {
        private readonly ILogger<LeadOkVayService> _logger;
        private readonly ILeadOkVayRepository _leadOkVayRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadOkVayService(
            ILogger<LeadOkVayService> logger,
            ILeadOkVayRepository leadOkVayRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _leadOkVayRepository = leadOkVayRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _userService = userService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _posRepository = posRepository;
        }

        public async Task<PagingResponse<GetLeadOkVayResponse>> GetAsync(GetLeadOkVayRequest getLeadOkVayRequest)
        {
            try
            {
                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                var filterCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadOkVayManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadOkVayManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadOkVayManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadOkVayManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadOkVayManagement_ViewAll);

                var leadOkVays = await _leadOkVayRepository.GetAsync(filterCreatorIds, getLeadOkVayRequest);
                var total = await _leadOkVayRepository.CountAsync(filterCreatorIds, getLeadOkVayRequest);

                return new PagingResponse<GetLeadOkVayResponse>
                {
                    TotalRecord = total,
                    Data = leadOkVays
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateLeadOkVayRequest createLeadOkVayRequest)
        {
            try
            {
                var leadOkVay = _mapper.Map<LeadOkVay>(createLeadOkVayRequest);

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                leadOkVay.TeamLeadInfo = user.TeamLeadInfo;
                leadOkVay.AsmInfo = user.AsmInfo;
                leadOkVay.PosInfo = user.PosInfo;
                leadOkVay.SaleInfomation = _mapper.Map<SaleInfomation>(user);
                leadOkVay.Result ??= new LeadOkVayResult();
                leadOkVay.Result.MarkReview();

                leadOkVay.Creator = _userLoginService.GetUserId();
                await _leadOkVayRepository.InsertOneAsync(leadOkVay);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadSourceId = leadOkVay.Id,
                    LeadSource = LeadSourceType.LeadSource
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateLeadOkVayRequest updateLeadOkVayRequest)
        {
            try
            {
                var leadSource = await _leadOkVayRepository.FindByIdAsync(id);

                if (leadSource == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(leadSource)));
                }

                if (leadSource is LeadOkVay leadOkVay)
                {
                    _mapper.Map(updateLeadOkVayRequest, leadOkVay);
                    leadOkVay.Modifier = _userLoginService.GetUserId();
                    leadOkVay.ModifiedDate = DateTime.Now;
                    leadOkVay.Result ??= new LeadOkVayResult();
                    leadOkVay.Result.MarkReview();
                    await _leadOkVayRepository.ReplaceOneAsync(leadOkVay);

                    _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                    {
                        LeadSourceId = id,
                        LeadSource = LeadSourceType.LeadSource
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetDetailLeadOkVayResponse> GetDetailAsync(string id)
        {
            try
            {
                var leadOkVay = await _leadOkVayRepository.GetDetailAsync(id);

                if (leadOkVay == null)

                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(LeadOkVay)));
                }

                return leadOkVay;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var leadOkVay = await _leadOkVayRepository.FindByIdAsync(id);

                if (leadOkVay == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(LeadOkVay)));
                }

                leadOkVay.IsDeleted = true;
                leadOkVay.DeletedDate = DateTime.Now;
                leadOkVay.DeletedBy = _userLoginService.GetUserId();

                await _leadOkVayRepository.ReplaceOneAsync(leadOkVay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task MarkApproveAsync(string id)
        {
            try
            {
                var leadSource = await _leadOkVayRepository.FindByIdAsync(id);

                if (leadSource == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(LeadOkVay)));
                }

                if (leadSource is LeadOkVay leadOkVay)
                {
                    leadOkVay.Result ??= new LeadOkVayResult();
                    leadOkVay.Result.MarkApprove();
                    leadOkVay.Modifier = _userLoginService.GetUserId();
                    leadOkVay.ModifiedDate = DateTime.Now;

                    await _leadOkVayRepository.ReplaceOneAsync(leadOkVay);

                    _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                    {
                        LeadSourceId = id,
                        LeadSource = LeadSourceType.LeadSource
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task MarkRejectAsync(string id, string reason)
        {
            try
            {
                var leadSource = await _leadOkVayRepository.FindByIdAsync(id);

                if (leadSource == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(LeadOkVay)));
                }

                if (leadSource is LeadOkVay leadOkVay)
                {
                    leadOkVay.Result ??= new LeadOkVayResult();
                    leadOkVay.Result.MarkReject(reason);
                    leadOkVay.Modifier = _userLoginService.GetUserId();
                    leadOkVay.ModifiedDate = DateTime.Now;

                    await _leadOkVayRepository.ReplaceOneAsync(leadOkVay);

                    _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                    {
                        LeadSourceId = id,
                        LeadSource = LeadSourceType.LeadSource
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

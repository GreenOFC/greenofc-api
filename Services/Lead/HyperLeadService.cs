using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Lead;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.Lead;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Lead;
using _24hplusdotnetcore.Repositories.VPS;
using _24hplusdotnetcore.Services.CRM;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IHyperLeadService
    {
        Task CreateAsync(CreateHyperLeadDto request);
        Task<PagingResponse<HyperLead>> GetList(PagingRequest pagingRequest);
    }

    public class HyperLeadService : IHyperLeadService, IScopedLifetime
    {
        private readonly ILogger<HyperLeadService> _logger;
        private readonly IMapper _mapper;
        private readonly IHyperLeadRepository _leadRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;

        public HyperLeadService(
            ILogger<HyperLeadService> logger,
            IMapper mapper,
            ILeadVpsRepository leadVpsRepository,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IHyperLeadRepository leadRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _leadRepository = leadRepository;
            _userRepository = userRepository;
            _userLoginService = userLoginService;
            _userService = userService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
        }

        public async Task CreateAsync(CreateHyperLeadDto request)
        {
            try
            {
                var lead = _mapper.Map<HyperLead>(request);
                var currentLead = await _leadRepository.FindOneAsync(x => x.TransactionId == lead.TransactionId);
                if (currentLead != null)
                {
                    _mapper.Map(lead, currentLead);
                    currentLead.ModifiedDate = DateTime.Now;
                    await _leadRepository.ReplaceOneAsync(currentLead);
                }
                else
                {
                    var user = await _userRepository.FindOneAsync(x => x.UserName == request.aff_sub1);
                    if (user != null)
                    {
                        lead.TeamLeadInfo = user.TeamLeadInfo;
                        lead.AsmInfo = user.AsmInfo;
                        lead.PosInfo = user.PosInfo;
                        lead.SaleChanelInfo = user.SaleChanelInfo;
                        lead.SaleInfomation = _mapper.Map<SaleInfomation>(user);
                        lead.Creator = user.Id;
                    }
                    await _leadRepository.Create(lead);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public async Task<PagingResponse<HyperLead>> GetList(PagingRequest pagingRequest)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.HyperLead_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HyperLead_ViewAll,
                    PermissionCost.PosLeadPermission.HyperLead_ViewAll,
                    PermissionCost.AsmPermission.HyperLead_ViewAll,
                    PermissionCost.TeamLeaderPermission.HyperLead_ViewAll);

                var result = await _leadRepository.GetList(filterByCreatorIds, pagingRequest);
                var rowCount = await _leadRepository.CountLead(filterByCreatorIds, pagingRequest);

                return new PagingResponse<HyperLead>
                {
                    TotalRecord = rowCount,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

    }
}

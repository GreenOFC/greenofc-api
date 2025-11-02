using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.F88;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.F88
{
    public interface ILeadF88Service
    {
        Task CreateNotification(F88PostBackDto dto);
        Task CreateAsync(CreateLeadF88Request createLeadF88Request);
        Task<PagingResponse<GetLeadF88Response>> GetAsync(GetLeadF88Request getLeadF88Request);
        Task<GetLeadF88Response> GetDetailAsync(string id);
        Task UpdateAsync(string id, UpdateLeadF88Request updateLeadF88Request);

        Task<PagingResponse<GetF88NotiResponse>> GetNotiAsync(GetF88NotiRequest getF88NotiRequest);

    }
    public class LeadF88Service : ILeadF88Service, IScopedLifetime
    {
        private readonly ILogger<LeadF88Service> _logger;
        private readonly IMapper _mapper;
        private readonly ILeadF88Repository _leadF88Repository;
        private readonly IF88NotificationRepository _f88NotiRepository;
        private readonly IMongoRepository<DataF88Processing> _dataF88ProcessingRepository;
        private readonly IMongoRepository<F88Notification> _f88Notification;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserServices _userService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadF88Service(
            ILogger<LeadF88Service> logger,
            IMapper mapper,
            ILeadF88Repository leadF88Repository,
            IF88NotificationRepository f88NotiRepository,
            IMongoRepository<DataF88Processing> dataF88ProcessingRepository,
            IMongoRepository<F88Notification> f88Notification,
            IUserLoginService userLoginService,
            IUserServices userService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IMongoRepository<User> userRepository,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _leadF88Repository = leadF88Repository;
            _f88NotiRepository = f88NotiRepository;
            _dataF88ProcessingRepository = dataF88ProcessingRepository;
            _f88Notification = f88Notification;
            _userLoginService = userLoginService;
            _userService = userService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _userRepository = userRepository;
            _posRepository = posRepository;
        }

        public async Task CreateNotification(F88PostBackDto dto)
        {
            try
            {
                F88Notification noti = _mapper.Map<F88Notification>(dto);

                await _f88Notification.InsertOneAsync(noti);

                LeadF88 lead = await _leadF88Repository.GetDetailByF88Id(noti.TransactionId);

                if (lead != null)
                {
                    lead.PostBack = new PostBack()
                    {
                        Status = dto.Status,
                        DetailStatus = dto.DetailStatus,
                        LoanAmount = dto.LoanAmount,
                    };
                    if (dto.Status == "2")
                    {
                        lead.Status = CustomerStatus.SUCCESS;
                    }
                    else if (dto.Status == "3")
                    {
                        lead.Status = CustomerStatus.REJECT;
                    }
                    await _leadF88Repository.ReplaceOneAsync(lead);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateLeadF88Request createLeadF88Request)
        {
            try
            {
                // var leadf88 = await _leadF88Repository.FindOneAsync(x => x.IdCard == createLeadF88Request.IdCard
                // && (x.Status == CustomerStatus.SUBMIT || x.Status == CustomerStatus.PROCESSING));
                // if (leadf88 != null)
                // {
                //     throw new ArgumentException(Common.Message.F88_CREATE_DUPLICATE);
                // }

                LeadF88 newLeadf88 = _mapper.Map<LeadF88>(createLeadF88Request);

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                newLeadf88.TeamLeadInfo = user.TeamLeadInfo;
                newLeadf88.AsmInfo = user.AsmInfo;
                newLeadf88.PosInfo = user.PosInfo;
                newLeadf88.SaleChanelInfo = user.SaleChanelInfo;
                newLeadf88.SaleInfomation = _mapper.Map<SaleInfomation>(user);

                newLeadf88.Creator = _userLoginService.GetUserId();
                newLeadf88.ModifiedDate = DateTime.Now;

                await _leadF88Repository.CreateAsync(newLeadf88);

                if (newLeadf88.Status == CustomerStatus.SUBMIT)
                {
                    await CreateDataF88ProcessingAsync(newLeadf88.Id);

                    _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                    {
                        LeadSourceId = newLeadf88.Id,
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

        public async Task<PagingResponse<GetLeadF88Response>> GetAsync(GetLeadF88Request getLeadF88Request)
        {
            try
            {

                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadF88Management_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadF88Management_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadF88Management_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadF88Management_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadF88Management_ViewAll);

                var leadF88s = await _leadF88Repository.GetAsync(filterByCreatorIds, getLeadF88Request);

                var total = await _leadF88Repository.CountAsync(filterByCreatorIds, getLeadF88Request);

                var leadF88Dtos = _mapper.Map<IEnumerable<GetLeadF88Response>>(leadF88s);

                var result = new PagingResponse<GetLeadF88Response>
                {
                    TotalRecord = total,
                    Data = leadF88Dtos
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task CreateDataF88ProcessingAsync(string leadF88Id)
        {
            DataF88Processing dataF88Processing = await _dataF88ProcessingRepository
                .FindOneAsync(x => x.LeadF88Id == leadF88Id && x.Status == DataF88ProcessingStatus.Draft);
            if (dataF88Processing != null)
            {
                return;
            }

            dataF88Processing = new DataF88Processing
            {
                LeadF88Id = leadF88Id,
                Status = DataF88ProcessingStatus.Draft
            };
            await _dataF88ProcessingRepository.InsertOneAsync(dataF88Processing);
        }

        public async Task<GetLeadF88Response> GetDetailAsync(string id)
        {
            try
            {
                var leadF88 = await _leadF88Repository.GetRelatedDetailAsync(id);

                if (leadF88 == null)
                {
                    throw new ArgumentException(Message.F88_NOT_FOUND);
                }

                return leadF88;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateLeadF88Request updateLeadF88Request)
        {
            try
            {
                var leadF88 = await _leadF88Repository.GetDetail(id);

                if (leadF88 == null)
                {
                    throw new ArgumentException(Common.Message.F88_NOT_FOUND);
                }
                else if (leadF88.Status != CustomerStatus.DRAFT)
                {
                    throw new ArgumentException(Common.Message.INCORRECT_STATUS);
                }

                // var leadf88Duplicate = await _leadF88Repository.FindOneAsync(x => x.Id != id
                //     && x.IdCard == updateLeadF88Request.IdCard
                //     && (x.Status == CustomerStatus.SUBMIT || x.Status == CustomerStatus.PROCESSING));
                // if (leadf88Duplicate != null)
                // {
                //     throw new ArgumentException(Common.Message.F88_CREATE_DUPLICATE);
                // }

                _mapper.Map(updateLeadF88Request, leadF88);
                leadF88.Modifier = _userLoginService.GetUserId();
                leadF88.ModifiedDate = DateTime.Now;

                await _leadF88Repository.ReplaceOneAsync(leadF88);

                if (leadF88.Status == CustomerStatus.SUBMIT)
                {
                    await CreateDataF88ProcessingAsync(id);

                    _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                    {
                        LeadSourceId = leadF88.Id,
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

        public async Task<PagingResponse<GetF88NotiResponse>> GetNotiAsync(GetF88NotiRequest getF88NotiRequest)
        {
            try
            {
                var leadF88s = await _f88NotiRepository.GetNotiAsync(
                   getF88NotiRequest.TextSearch,
                   getF88NotiRequest.Status,
                   getF88NotiRequest.PageIndex,
                   getF88NotiRequest.PageSize);

                var total = await _f88NotiRepository.CountAsync(getF88NotiRequest.TextSearch, getF88NotiRequest.Status);

                var leadF88Dtos = _mapper.Map<IEnumerable<GetF88NotiResponse>>(leadF88s);

                var result = new PagingResponse<GetF88NotiResponse>
                {
                    TotalRecord = total,
                    Data = leadF88Dtos
                };

                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ISaleChanelService
    {
        Task<PagingResponse<SaleChanelResponse>> GetAsync(PagingRequest request);
        Task<SaleChanelCreateResponse> CreateAsync(SaleChanelCreateRequest saleChanelCreateRequest);
        Task UpdateAsync(string id, SaleChanelUpdateRequest saleChanelUpdateRequest);
        Task<SaleChanelDetailResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
        Task AddPosAsync(string id, string posId);
        Task DeletePosAsync(string id, string posId);
        Task<PagingResponse<SaleChanelPosInfoDto>> GetPosAsync(string id, SaleChanelPosGetListRequest pagingRequest);
        Task SyncUserAsync();
    }

    public class SaleChanelService : ISaleChanelService, IScopedLifetime
    {
        private readonly ILogger<SaleChanelService> _logger;
        private readonly ISaleChanelRepository _saleChanelRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IPosRepository _posRepository;
        private readonly ISaleChanelPosDomainService _saleChanelPosDomainService;
        private readonly ISaleChanelConfigUserRepository _saleChanelConfigUserRepository;

        public SaleChanelService(
            ILogger<SaleChanelService> logger,
            ISaleChanelRepository saleChanelRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IPosRepository posRepository,
            ISaleChanelPosDomainService saleChanelPosDomainService,
            ISaleChanelConfigUserRepository saleChanelConfigUserRepository)
        {
            _logger = logger;
            _saleChanelRepository = saleChanelRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _posRepository = posRepository;
            _saleChanelPosDomainService = saleChanelPosDomainService;
            _saleChanelConfigUserRepository = saleChanelConfigUserRepository;
        }

        public async Task<PagingResponse<SaleChanelResponse>> GetAsync(PagingRequest request)
        {
            try
            {
                var saleChanels = await _saleChanelRepository.GetAsync(request.TextSearch, request.PageIndex, request.PageSize);
                var total = await _saleChanelRepository.CountAsync(request.TextSearch);

                return new PagingResponse<SaleChanelResponse>
                {
                    TotalRecord = total,
                    Data = saleChanels
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<SaleChanelCreateResponse> CreateAsync(SaleChanelCreateRequest saleChanelCreateRequest)
        {
            try
            {
                var saleChanel = _mapper.Map<SaleChanel>(saleChanelCreateRequest);
                saleChanel.Creator = _userLoginService.GetUserId();
                var user = await _userRepository.FindByIdAsync(saleChanel.Creator);
                saleChanel.SaleInfo = _mapper.Map<SaleInfomation>(user);
                if (!string.IsNullOrEmpty(saleChanelCreateRequest.SaleChanelConfigUserId))
                {
                    var saleChannelConfigUser = await _saleChanelConfigUserRepository.FindByIdAsync(saleChanelCreateRequest.SaleChanelConfigUserId);
                    saleChanel.SaleChanelConfigUserInfo = _mapper.Map<SaleChanelConfigUserInfo>(saleChannelConfigUser);
                }
                if (!string.IsNullOrEmpty(saleChanelCreateRequest.HeadOfSaleAdminId))
                {
                    var headOfSaleAdmin = await _userRepository.FindByIdAsync(saleChanelCreateRequest.HeadOfSaleAdminId);
                    saleChanel.HeadOfSaleAdmin = _mapper.Map<SaleInfomation>(headOfSaleAdmin);
                }
                await _saleChanelRepository.InsertOneAsync(saleChanel);
                return _mapper.Map<SaleChanelCreateResponse>(saleChanel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, SaleChanelUpdateRequest saleChanelUpdateRequest)
        {
            try
            {
                var saleChanel = await _saleChanelRepository.FindByIdAsync(id);

                if (saleChanel == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
                }
                var isUpdateSaleChanelConfigUser = !string.Equals(saleChanelUpdateRequest.SaleChanelConfigUserId, saleChanel.SaleChanelConfigUserId);
                var isUpdateHeadOfSaleAdmin = !string.Equals(saleChanelUpdateRequest.HeadOfSaleAdminId, saleChanel.HeadOfSaleAdmin?.Id);
                _mapper.Map(saleChanelUpdateRequest, saleChanel);
                if (isUpdateSaleChanelConfigUser)
                {
                    if (string.IsNullOrEmpty(saleChanelUpdateRequest.SaleChanelConfigUserId))
                    {
                        saleChanel.SaleChanelConfigUserInfo = null;
                    }
                    else
                    {
                        var saleChannelConfigUser = await _saleChanelConfigUserRepository.FindByIdAsync(saleChanelUpdateRequest.SaleChanelConfigUserId);
                        saleChanel.SaleChanelConfigUserInfo = _mapper.Map<SaleChanelConfigUserInfo>(saleChannelConfigUser);
                    }
                }
                if (isUpdateHeadOfSaleAdmin)
                {
                    if (string.IsNullOrEmpty(saleChanelUpdateRequest.HeadOfSaleAdminId))
                    {
                        saleChanel.HeadOfSaleAdmin = null;
                    }
                    else
                    {
                        var headOfSaleAdmin = await _userRepository.FindByIdAsync(saleChanelUpdateRequest.HeadOfSaleAdminId);
                        saleChanel.HeadOfSaleAdmin = _mapper.Map<SaleInfomation>(headOfSaleAdmin);
                    }
                }
                saleChanel.Modifier = _userLoginService.GetUserId();
                var user = await _userRepository.FindByIdAsync(saleChanel.Modifier);
                saleChanel.UserModified = _mapper.Map<SaleInfomation>(user);
                saleChanel.ModifiedDate = DateTime.Now;
                await _saleChanelRepository.ReplaceOneAsync(saleChanel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<SaleChanelDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var saleChanel = await _saleChanelRepository.GetDetailAsync(id);

                if (saleChanel == null)

                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
                }

                return saleChanel;
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
                var update = Builders<SaleChanel>.Update
                    .Set(x => x.IsDeleted, true)
                    .Set(x => x.DeletedDate, DateTime.Now)
                    .Set(x => x.DeletedBy, _userLoginService.GetUserId());

                await _saleChanelRepository.UpdateOneAsync(x => x.Id == id, update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AddPosAsync(string id, string posId)
        {
            try
            {
                await _saleChanelPosDomainService.AddAsync(id, posId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeletePosAsync(string id, string posId)
        {
            try
            {
                await _saleChanelPosDomainService.DeleteAsync(id, posId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<SaleChanelPosInfoDto>> GetPosAsync(string id, SaleChanelPosGetListRequest pagingRequest)
        {
            try
            {
                var saleChanel = await _saleChanelRepository.FindByIdAsync(id);

                if (saleChanel == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
                }

                var posesFilter = (saleChanel.Poses ?? new List<SaleChanelPosInfo>())
                    .WhereIf(!string.IsNullOrEmpty(pagingRequest.TextSearch), x => x.Name.ToLower().Contains(pagingRequest.TextSearch.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(pagingRequest.CreatorName), x => x.SaleInfo != null &&
                        (
                            (!string.IsNullOrEmpty(x.SaleInfo.UserName) && x.SaleInfo.UserName.ToLower().Contains(pagingRequest.CreatorName.ToLower())) ||
                            (!string.IsNullOrEmpty(x.SaleInfo.FullName) && x.SaleInfo.FullName.ToLower().Contains(pagingRequest.CreatorName.ToLower()))
                        ));

                var poses = posesFilter
                    .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                    .Take(pagingRequest.PageSize);

                var posDtos = _mapper.Map<IEnumerable<SaleChanelPosInfoDto>>(poses);

                return new PagingResponse<SaleChanelPosInfoDto>
                {
                    TotalRecord = posesFilter.Count(),
                    Data = posDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SyncUserAsync()
        {
            try
            {
                var saleChanels = (await _saleChanelRepository.FilterByAsync(x => !x.IsDeleted && x.Poses.Any())).ToList();

                if (saleChanels == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
                }
                foreach (var saleChanel in saleChanels)
                {
                    var posIds = saleChanel.Poses.Select(x => x.Id);

                    var saleChanelInfo = _mapper.Map<SaleChanelInfo>(saleChanel);
                    await _posRepository.UpdateListSaleChanelAsync(posIds, saleChanelInfo, _userLoginService.GetUserId());
                    await _userRepository.UpdateSaleChanelByListPosIdAsync(posIds, saleChanelInfo, _userLoginService.GetUserId());
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

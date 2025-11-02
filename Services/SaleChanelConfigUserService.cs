using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ISaleChanelConfigUserService
    {
        Task<PagingResponse<SaleChanelConfigUserResponse>> GetAsync(PagingRequest request);
        Task<SaleChanelConfigUserCreateResponse> CreateAsync(SaleChanelConfigUserCreateRequest saleChanelConfigUserCreateRequest);
        Task UpdateAsync(string id, SaleChanelConfigUserUpdateRequest saleChanelConfigUserUpdateRequest);
        Task<SaleChanelConfigUserDetailResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
    }
    public class SaleChanelConfigUserService: ISaleChanelConfigUserService, IScopedLifetime
    {
        private readonly ILogger<SaleChanelConfigUserService> _logger;
        private readonly ISaleChanelConfigUserRepository _saleChanelConfigUserRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IPosRepository _posRepository;
        private readonly ISaleChanelPosDomainService _saleChanelPosDomainService;
        private readonly ISaleChanelRepository _saleChanelRepository;

        public SaleChanelConfigUserService(
            ILogger<SaleChanelConfigUserService> logger,
            ISaleChanelConfigUserRepository saleChanelConfigUserRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IPosRepository posRepository,
            ISaleChanelPosDomainService saleChanelPosDomainService,
            ISaleChanelRepository saleChanelRepository)
        {
            _logger = logger;
            _saleChanelConfigUserRepository = saleChanelConfigUserRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _posRepository = posRepository;
            _saleChanelPosDomainService = saleChanelPosDomainService;
            _saleChanelRepository = saleChanelRepository;
        }

        public async Task<PagingResponse<SaleChanelConfigUserResponse>> GetAsync(PagingRequest request)
        {
            try
            {
                var saleChanelConfigUsers = await _saleChanelConfigUserRepository.GetAsync(request.TextSearch, request.PageIndex, request.PageSize);
                var total = await _saleChanelConfigUserRepository.CountAsync(request.TextSearch);

                return new PagingResponse<SaleChanelConfigUserResponse>
                {
                    TotalRecord = total,
                    Data = saleChanelConfigUsers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<SaleChanelConfigUserCreateResponse> CreateAsync(SaleChanelConfigUserCreateRequest saleChanelConfigUserCreateRequest)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                var saleChanelConfigUser = _mapper.Map<SaleChanelConfigUser>(saleChanelConfigUserCreateRequest);
                saleChanelConfigUser.Creator = _userLoginService.GetUserId();
                saleChanelConfigUser.SaleInfo = _mapper.Map<SaleInfomation>(user);

                await _saleChanelConfigUserRepository.InsertOneAsync(saleChanelConfigUser);
                return _mapper.Map<SaleChanelConfigUserCreateResponse>(saleChanelConfigUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, SaleChanelConfigUserUpdateRequest saleChanelConfigUserUpdateRequest)
        {
            try
            {
                var saleChanelConfigUser = await _saleChanelConfigUserRepository.FindByIdAsync(id);

                if (saleChanelConfigUser == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanelConfigUser)));
                }

                _mapper.Map(saleChanelConfigUserUpdateRequest, saleChanelConfigUser);
                saleChanelConfigUser.Modifier = _userLoginService.GetUserId();
                var user = await _userRepository.FindByIdAsync(saleChanelConfigUser.Modifier);
                saleChanelConfigUser.UserModified = _mapper.Map<SaleInfomation>(user);
                saleChanelConfigUser.ModifiedDate = DateTime.Now;
                await _saleChanelConfigUserRepository.ReplaceOneAsync(saleChanelConfigUser);
                await _saleChanelRepository.UpdateSaleChanelConfigUserInfoAsync(id, _mapper.Map<SaleChanelConfigUserInfo>(saleChanelConfigUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<SaleChanelConfigUserDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var saleChanelConfigUser = await _saleChanelConfigUserRepository.GetDetailAsync(id);

                if (saleChanelConfigUser == null)

                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanelConfigUser)));
                }

                return saleChanelConfigUser;
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
                var update = Builders<SaleChanelConfigUser>.Update
                    .Set(x => x.IsDeleted, true)
                    .Set(x => x.DeletedDate, DateTime.Now)
                    .Set(x => x.DeletedBy, _userLoginService.GetUserId());

                await _saleChanelConfigUserRepository.UpdateOneAsync(x => x.Id == id, update);
                await _saleChanelRepository.UpdateSaleChanelConfigUserInfoAsync(id, saleChanelConfigUserInfo: null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

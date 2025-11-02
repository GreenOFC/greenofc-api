using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Banners;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IBannerService
    {
        Task<PagingResponse<GetBannerResponse>> GetAsync(GetBannerRequest getBannerRequest);
        IEnumerable<string> GetAllUrl();
        Task<GetDetailBannerResponse> GetDetailAsync(string id);
        Task CreateAsync(CreateBannerRequest createBannerRequest);
        Task UpdateAsync(string id, UpdateBannerRequest updateBannerRequest);
        Task DeleteAsync(string id);
    }
    public class BannerService : IBannerService, IScopedLifetime
    {
        private readonly ILogger<BannerService> _logger;
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;

        public BannerService(
            ILogger<BannerService> logger,
            IBannerRepository bannerRepository,
            IMapper mapper,
            IUserLoginService userLoginService)
        {
            _logger = logger;
            _bannerRepository = bannerRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
        }

        public async Task<PagingResponse<GetBannerResponse>> GetAsync(GetBannerRequest getBannerRequest)
        {
            try
            {
                var banners = await _bannerRepository.GetAsync(
                    getBannerRequest.StartDate,
                    getBannerRequest.EndDate,
                    getBannerRequest.PageIndex,
                    getBannerRequest.PageSize);

                var total = await _bannerRepository.CountAsync(getBannerRequest.StartDate, getBannerRequest.EndDate);

                var result = new PagingResponse<GetBannerResponse>
                {
                    TotalRecord = total,
                    Data = banners
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public IEnumerable<string> GetAllUrl()
        {
            try
            {
                var banners = _bannerRepository.FilterBy(x => !x.IsDeleted).ToList();

                return banners.Select(x => x.ImageUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<GetDetailBannerResponse> GetDetailAsync(string id)
        {
            try
            {
                var banner = await _bannerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (banner == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Banner)));
                }

                var bannerResult = _mapper.Map<GetDetailBannerResponse>(banner);

                return bannerResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateBannerRequest createBannerRequest)
        {
            try
            {
                var banner = _mapper.Map<Banner>(createBannerRequest);
                banner.Creator = _userLoginService.GetUserId();
                await _bannerRepository.InsertOneAsync(banner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateBannerRequest updateBannerRequest)
        {
            try
            {
                var banner = await _bannerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (banner == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Banner)));
                }

                _mapper.Map(updateBannerRequest, banner);
                banner.Modifier = _userLoginService.GetUserId();
                banner.ModifiedDate = DateTime.Now;

                await _bannerRepository.ReplaceOneAsync(banner);
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
                var banner = await _bannerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (banner == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Banner)));
                }

                banner.IsDeleted = true;
                banner.DeletedBy = _userLoginService.GetUserId();
                banner.DeletedDate = DateTime.Now;

                await _bannerRepository.ReplaceOneAsync(banner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

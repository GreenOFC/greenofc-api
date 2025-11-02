using _24hplusdotnetcore.Common;
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
    public interface ISaleChanelPosDomainService
    {
        Task AddAsync(string saleChanelId, string posId);
        Task DeleteAsync(string saleChanelId, string posId);
    }

    public class SaleChanelPosDomainService: ISaleChanelPosDomainService, IScopedLifetime
    {
        private readonly ILogger<SaleChanelPosDomainService> _logger;
        private readonly ISaleChanelRepository _saleChanelRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IPosRepository _posRepository;

        public SaleChanelPosDomainService(
            ILogger<SaleChanelPosDomainService> logger,
            ISaleChanelRepository saleChanelRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IPosRepository posRepository)
        {
            _logger = logger;
            _saleChanelRepository = saleChanelRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _posRepository = posRepository;
        }

        public async Task AddAsync(string saleChanelId, string posId)
        {
            try
            {
                var saleChanel = await _saleChanelRepository.FindByIdAsync(saleChanelId);
                if (saleChanel == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
                }

                var pos = await _posRepository.GetDetailAsync(posId);
                if (pos == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(POS)));
                }

                var posInfo = _mapper.Map<SaleChanelPosInfo>(pos);
                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                posInfo.SaleInfo = _mapper.Map<SaleInfomation>(user);
                await _saleChanelRepository.AddPosAsync(saleChanelId, posInfo);

                var saleChanelInfo = _mapper.Map<SaleChanelInfo>(saleChanel);
                await _posRepository.UpdateSaleChanelAsync(posId, saleChanelInfo, _userLoginService.GetUserId());
                await _userRepository.UpdateSaleChanelAsync(posId, saleChanelInfo, _userLoginService.GetUserId());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(string saleChanelId, string posId)
        {
            try
            {
                var saleChanel = await _saleChanelRepository.FindByIdAsync(saleChanelId);
                if (saleChanel == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
                }

                await _saleChanelRepository.DeletePosAsync(saleChanelId, posId);

                await _posRepository.UpdateSaleChanelAsync(posId, null, _userLoginService.GetUserId());
                await _userRepository.UpdateSaleChanelAsync(posId, null, _userLoginService.GetUserId());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

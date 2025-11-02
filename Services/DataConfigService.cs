using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IDataConfigService
    {
        Task<IEnumerable<GetDataConfigResponse>> GetAsync(string greenType, string type);
    }
    public class DataConfigService: IDataConfigService, IScopedLifetime
    {
        private readonly ILogger<DataConfigService> _logger;
        private readonly IMapper _mapper;
        private readonly IDataConfigRepository _dataConfigRepository;

        public DataConfigService(
            ILogger<DataConfigService>  logger, 
            IMapper mapper,
            IDataConfigRepository dataConfigRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _dataConfigRepository = dataConfigRepository;
        }

        public async Task<IEnumerable<GetDataConfigResponse>> GetAsync(string greenType, string type)
        {
            try
            {
                var dataConfigs = await _dataConfigRepository.GetAsync(greenType, type);

                var dataConfigDtos = _mapper.Map<IEnumerable<GetDataConfigResponse>>(dataConfigs);

                return await Task.FromResult(dataConfigDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

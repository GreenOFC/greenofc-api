using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Services.PtfOmnis;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class GetMasterDataJob: CronJobService
    {
        private const string ServiceName = nameof(GetMasterDataJob);
        private readonly ILogger<GetMasterDataJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public GetMasterDataJob(
            ILogger<GetMasterDataJob> logger,
            IOptions<MAFCConfig> options,
            IServiceProvider serviceProvider
            ) : base(options.Value.CronExpression)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{ServiceName} starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} {ServiceName} is working.");

            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                IMAFCBankService _mAFCBankService = scope.ServiceProvider.GetRequiredService<IMAFCBankService>();
                IMAFCSchemeService _mAFCSchemeService = scope.ServiceProvider.GetRequiredService<IMAFCSchemeService>();
                IMAFCSaleOfficeService _mAFCSaleOfficeService = scope.ServiceProvider.GetRequiredService<IMAFCSaleOfficeService>();
                IMAFCCityService _mAFCCityService = scope.ServiceProvider.GetRequiredService<IMAFCCityService>();
                IMAFCDistrictService _mAFCDistrictService = scope.ServiceProvider.GetRequiredService<IMAFCDistrictService>();
                IMAFCWardService _mAFCWardService = scope.ServiceProvider.GetRequiredService<IMAFCWardService>();
                IMCKiosService _mcKiosService = scope.ServiceProvider.GetRequiredService<IMCKiosService>();
                // ILeadEcResourceService _leadEcResourceService = scope.ServiceProvider.GetRequiredService<ILeadEcResourceService>();
                // ILeadEcProductService _leadEcProductService = scope.ServiceProvider.GetRequiredService<ILeadEcProductService>();
                IPtfOmniMasterDataService _ptfOmniMasterDataService = scope.ServiceProvider.GetRequiredService<IPtfOmniMasterDataService>();

                await _mAFCBankService.SyncAsync();
                await _mAFCSchemeService.SyncAsync();
                await _mAFCSaleOfficeService.SyncAsync();
                await _mAFCCityService.SyncAsync();
                await _mAFCDistrictService.SyncAsync();
                await _mAFCWardService.SyncAsync();

                await _mcKiosService.SyncAsync();
                // await _leadEcResourceService.SyncAsync();
                // await _leadEcProductService.SyncAsync();
                await _ptfOmniMasterDataService.SyncAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{ServiceName} stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}

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
    public class PtfOmniDataProcessingJob : CronJobService
    {
        private const string ServiceName = nameof(PtfOmniDataProcessingJob);
        private readonly ILogger<PtfOmniDataProcessingJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PtfOmniDataProcessingJob(
            ILogger<PtfOmniDataProcessingJob> logger,
            IOptions<PtfOmniConfig> options,
            IServiceProvider serviceProvider) : base(options.Value.CronExpression)
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

                IPtfOmniDataProcessingService _ptfOmniDataProcessingService = scope.ServiceProvider.GetRequiredService<IPtfOmniDataProcessingService>();
                IPtfOmniService _ptfOmniService = scope.ServiceProvider.GetRequiredService<IPtfOmniService>();

                // await _ptfOmniDataProcessingService.SyncDataAsync();
                await _ptfOmniService.UpdateCustomerStatusAsync();
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

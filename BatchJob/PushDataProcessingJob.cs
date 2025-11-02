using _24hplusdotnetcore.Services.F88;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class PushDataProcessingJob: CronJobService
    {
        private const string ServiceName = nameof(PushDataProcessingJob);
        private readonly ILogger<PushDataProcessingJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PushDataProcessingJob(
            ILogger<PushDataProcessingJob> logger,
            IOptions<CronJobConfig> options,
            IServiceProvider serviceProvider
            ) : base(options.Value.CronExpression, logger)
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
                IDataF88ProcessingService _dataF88ProcessingService = scope.ServiceProvider.GetRequiredService<IDataF88ProcessingService>();

                await _dataF88ProcessingService.SyncDataAsync();
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

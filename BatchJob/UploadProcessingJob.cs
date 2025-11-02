using _24hplusdotnetcore.Services.F88;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class UploadProcessingJob: CronJobService
    {
        private const string ServiceName = nameof(UploadProcessingJob);
        private readonly ILogger<UploadProcessingJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public UploadProcessingJob(
            ILogger<UploadProcessingJob> logger,
            IOptions<CronJobConfig> options,
            IServiceProvider serviceProvider
            ) : base(options.Value.CronUploadExpression, logger)
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
                MCService _mcService = scope.ServiceProvider.GetRequiredService<MCService>();
                
                await _mcService.PushDataToMCAsync();
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

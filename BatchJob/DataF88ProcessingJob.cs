using _24hplusdotnetcore.Services.F88;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class DataF88ProcessingJob: CronJobService
    {
        private const string ServiceName = nameof(DataF88ProcessingJob);
        private readonly ILogger<DataF88ProcessingJob> _logger;
        private readonly IDataF88ProcessingService _dataF88ProcessingService;

        public DataF88ProcessingJob(
            ILogger<DataF88ProcessingJob> logger,
            IOptions<F88Config> options,
            IDataF88ProcessingService dataF88ProcessingService): base(options.Value.CronExpression)
        {
            _logger = logger;
            _dataF88ProcessingService = dataF88ProcessingService;
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

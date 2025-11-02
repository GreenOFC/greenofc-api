using _24hplusdotnetcore.Services.OCR;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class OCRReceiveJob : CronJobService
    {
        private const string ServiceName = nameof(OCRReceiveJob);
        private readonly ILogger<OCRReceiveJob> _logger;
        private readonly IOCRService _oCRService;

        public OCRReceiveJob(
            ILogger<OCRReceiveJob> logger,
            IOptions<OCRConfig> options,
            IOCRService oCRService) : base(options.Value.CronExpression)
        {
            _logger = logger;
            _oCRService = oCRService;
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
                await _oCRService.CheckORCResultAsync();
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

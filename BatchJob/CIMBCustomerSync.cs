using _24hplusdotnetcore.Services.CIMB;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class CIMBCustomerSync : CronJobService
    {
        private const string ServiceName = nameof(CIMBCustomerSync);
        private readonly ILogger<CIMBCustomerSync> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CIMBCustomerSync(
            ILogger<CIMBCustomerSync> logger,
            IServiceProvider serviceProvider,
            IOptions<CIMBConfig> cimbConfig) : base(cimbConfig.Value.CronExpression)
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
                CIMBService _cimbService = scope.ServiceProvider.GetRequiredService<CIMBService>();
                CIMBCustomerLoanStatusService _cimbCustomerLoanStatusService = scope.ServiceProvider.GetRequiredService<CIMBCustomerLoanStatusService>();
                await _cimbService.SyncCIMBDataProcessing();
                await _cimbCustomerLoanStatusService.SyncCustomerLoanStatus();
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

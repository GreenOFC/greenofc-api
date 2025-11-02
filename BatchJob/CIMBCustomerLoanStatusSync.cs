using _24hplusdotnetcore.Services.CIMB;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class CIMBCustomerLoanStatusSync : CronJobService
    {
        private const string ServiceName = nameof(CIMBCustomerLoanStatusSync);
        private readonly ILogger<CIMBCustomerLoanStatusSync> _logger;
        private readonly CIMBConfig _cimbConfig;
        private readonly CIMBCustomerLoanStatusService _cimbCustomerLoanStatusService;

        public CIMBCustomerLoanStatusSync(
            ILogger<CIMBCustomerLoanStatusSync> logger,
            IOptions<CIMBConfig> cimbConfig,
            CIMBCustomerLoanStatusService cimbCustomerLoanStatusService) : base(cimbConfig.Value.LoanStatusCronExpression)
        {
            _logger = logger;
            _cimbConfig = cimbConfig.Value;
            _cimbCustomerLoanStatusService = cimbCustomerLoanStatusService;
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

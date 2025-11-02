using _24hplusdotnetcore.Services.CRM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class PushCustomerToCRM : HostedService
    {
        private readonly ILogger<PushCustomerToCRM> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PushCustomerToCRM(ILogger<PushCustomerToCRM> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                CRMServices _crmServices = scope.ServiceProvider.GetRequiredService<CRMServices>();
                await _crmServices.AddingDataToCRMAsync();
                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }
    }
}

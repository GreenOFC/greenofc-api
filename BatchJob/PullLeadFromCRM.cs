using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Services.CRM;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class PullLeadFromCRM : HostedService
    {
        private const string queryGreenE = "SELECT * FROM Potentials WHERE cf_1430 in ('RETURN', 'SUCCESS', 'REJECT', 'DONE') and cf_1178 LIKE ('%SHINHAN%') and leadsource = 'MobileGreenE' and cf_1206 = 0";
        // private const string queryGreenP = "SELECT * FROM Potentials WHERE cf_1430 in ('RETURN', 'SUCCESS', 'REJECT', 'DONE', 'PROCESSING') and cf_1178 LIKE ('%PTF%') and leadsource = 'PTF' and cf_1206 = 0";
        private const string queryF88 = "SELECT * FROM Potentials WHERE cf_1244 ='DSA' and leadsource = 'MobileGreenF' and cf_1206 = 0";
        private readonly IServiceProvider _serviceProvider;

        public PullLeadFromCRM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                CRMServices _cRMServices = scope.ServiceProvider.GetRequiredService<CRMServices>();
                await _cRMServices.GetGreenFromCrmAsync(queryGreenE, GreenType.GreenE);
                // await _cRMServices.GetGreenFromCrmAsync(queryGreenP, GreenType.GreenP);
                await _cRMServices.GetGreenFromCrmAsync(queryF88, GreenType.GreenF88);

                await Task.Delay(1800000, cancellationToken);
            }
        }
    }
}

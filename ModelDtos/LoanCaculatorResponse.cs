using System;

namespace _24hplusdotnetcore.ModelDtos
{
    public class LoanCaculatorResponse
    {
        public double PMT { get; set; }
        public double DTI { get; set; }
        public double EMI => Math.Round(-PMT, 0);
        public double PTI => Math.Round(-DTI * 100, 2);
    }
}

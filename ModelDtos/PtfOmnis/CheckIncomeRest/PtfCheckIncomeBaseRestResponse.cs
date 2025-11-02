using System;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeRest
{
    public class PtfCheckIncomeBaseRestResponse<T>
    {
        public bool Success { get; set; }
        public PtfCheckIncomeMetaRestResponse Meta { get; set; }
        public T Data { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

    }

    public class PtfCheckIncomeMetaRestResponse
    {

        public string RequestId { get; set; }

        public string Timestamp { get; set; }

    }
}

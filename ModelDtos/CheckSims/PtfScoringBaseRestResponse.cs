using System;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringBaseRestResponse<T>
    {
        public bool Success { get; set; }

        public PtfScoringDataRestResponse<T> Data { get; set; }
    }

    public class PtfScoringDataRestResponse<T>
    {
        public T Data { get; set; }

        public string Message { get; set; }

        public DateTime? Time { get; set; }

        public string Verdict { get; set; }
    }
}

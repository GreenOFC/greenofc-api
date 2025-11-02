using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class DataMAFCProcessingDto
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Step { get; set; }
        public string Message { get; set; }
        public IEnumerable<DataMAFCProcessingPayloadDto> Payloads { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }

    public class DataMAFCProcessingPayloadDto
    {
        public string Message { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

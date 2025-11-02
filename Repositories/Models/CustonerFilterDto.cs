using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Repositories.Models
{
    public class CustonerFilterDto
    {
        public string GreenType { get; set; }
        public IEnumerable<string> CreatorIds { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string TeamLead { get; set; }
        public string Asm { get; set; }
        public string PosManager { get; set; }
        public string Sale { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }
        public string ProductLine { get; set; }
        public string ReturnStatus { get; set; }
        public bool HasMafcId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}

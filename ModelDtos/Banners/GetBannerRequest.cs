using System;

namespace _24hplusdotnetcore.ModelDtos.Banners
{
    public class GetBannerRequest: PagingRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

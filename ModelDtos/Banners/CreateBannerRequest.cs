using System;

namespace _24hplusdotnetcore.ModelDtos.Banners
{
    public class CreateBannerRequest
    {
        public string ImageUrl { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

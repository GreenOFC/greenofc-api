using System;

namespace _24hplusdotnetcore.ModelDtos.Banners
{
    public class GetDetailBannerResponse
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

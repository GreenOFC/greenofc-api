using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.Lead
{
    public class CreateHyperLeadDto
    {
        public string conversion_id { get; set; }
        public string click_id { get; set; }
        public string conversion_sale_amount { get; set; }
        public string conversion_time { get; set; }
        public string product_url { get; set; }
        public string product_category { get; set; }
        public string offer_id { get; set; }
        public string conversion_status_code { get; set; }
        public string conversion_publisher_payout { get; set; }
        public string conversion_modified_time { get; set; }
        public string product_sku { get; set; }
        public string transaction_id { get; set; }
        public string conversion_status { get; set; }
        public string status_message { get; set; }
        public string click_time { get; set; }
        public string product_name { get; set; }
        public string product_category_id { get; set; }
        public string aff_sub1 { get; set; }
        public string aff_sub2 { get; set; }
        public string aff_sub3 { get; set; }
        public string aff_sub4 { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.F88
{
    public class CreateLeadF88Request
    {
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string LoanCategory { get; set; }
        public DataConfigDto LoanCategoryData { get; set; }
        public string IdCard { get; set; }
        public string IdCardProvince { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public DataConfigDto ProvinceData { get; set; }
        public string DateOfBirth { get; set; }
        public string Description { get; set; }
        public string SignAddress { get; set; }
        public DataConfigDto SignAddressData { get; set; }
        public string Status { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public abstract class PtfScopingBaseRequest
    {
        [Required]
        public string PhoneNumber { private get; set; }

        public string GetPhoneNumber()
        {
            var codes = new[] { "0", "+84", "84" };

            foreach (var item in codes)
            {
                if (PhoneNumber?.StartsWith(item) == true)
                {
                    return PhoneNumber.Substring(item.Length);
                }
            }
            return PhoneNumber;
        }
    }
}

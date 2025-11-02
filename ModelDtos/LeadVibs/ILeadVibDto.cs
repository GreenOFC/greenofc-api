using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadVibs
{
    public interface ILeadVibDto
    {
        [Required]
        string FullName { get; set; }
        string IdCard { get; set; }
        string Gender { get; set; }
        [Required]
        string Phone { get; set; }
        string DateOfBirth { get; set; }
        LeadVibAddressDto TemporaryAddress { get; set; }
        DataConfigDto Constitution { get; set; }
        DataConfigDto Income { get; set; }
        [Required]
        DataConfigDto Product { get; set; }
    }
    public abstract class LeadVibDto : ILeadVibDto
    {
        [Required]
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public LeadVibAddressDto TemporaryAddress { get; set; }
        public DataConfigDto Constitution { get; set; }
        public DataConfigDto Income { get; set; }
        [Required]
        public DataConfigDto Product { get; set; }
    }
}
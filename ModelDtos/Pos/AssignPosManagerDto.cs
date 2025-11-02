using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Pos
{
    public class AssignPosManagerDto
    {
        [Required]
        public string UserId { get; set; }
    }
}

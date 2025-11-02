using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.News
{
    public class UpdateNewsRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public string Title { get; set; }

        public string AvatarUrl { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public IEnumerable<string> GroupNotificationIds { get; set; }

        public IEnumerable<string> Tag { get; set; }
    }
}

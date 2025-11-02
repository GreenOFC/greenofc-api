using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace _24hplusdotnetcore.ModelDtos
{
    public class PagingRequest : BaseRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;
        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;

    }
}

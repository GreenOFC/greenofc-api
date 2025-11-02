using System.ComponentModel.DataAnnotations;
using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniUpdateDataProcessingRequest
    {
        public string Step { get; set; }
        public PtfOmniDataProcessingStatus Status { get; set; }
    }
}

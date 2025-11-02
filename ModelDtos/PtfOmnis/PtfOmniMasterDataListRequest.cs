using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniMasterDataListRequest
    {
        public PtfOmniMasterDataListRequest()
        {

        }

        public PtfOmniMasterDataListRequest(string type)
        {
            Type = type;
            GetMetaData = true;
        }

        [Required]
        public string Type { get; set; }

        public bool? GetMetaData { get; set; }
    }
}

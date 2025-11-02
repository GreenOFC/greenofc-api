using System.Dynamic;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniMasterDataResponse
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string ParentType { get; set; }

        public ExpandoObject MetaData { get; set; }

        public string ParentValue { get; set; }
    }
}

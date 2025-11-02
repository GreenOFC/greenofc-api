namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniMasterDataVersionResponse
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string ValueType { get; set; }

        public string ParentType { get; set; }

        public int? Version { get; set; }

        public long? CreateTime { get; set; }

        public long? UpdateTime { get; set; }

        public string CreateBy { get; set; }

        public string UpdateBy { get; set; }

        public bool? Private { get; set; }
    }
}

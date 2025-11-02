namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCSchemeResponse
    {
        public string SchemeId { get; set; }
        public string SchemeName { get; set; }
        public string SchemeGroup { get; set; }
        public string Product { get; set; }
        public string Maxamtfin { get; set; }
        public string Minamtfin { get; set; }
        public string Maxtenure { get; set; }
        public string Mintenure { get; set; }
        public string Type => SchemeName?.IndexOf(" AT") > -1 ? "AT" : "NON-AT";
    }
}

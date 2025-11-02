namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class MafcWorkingDto
    {

        public string Income { get; set; }
        public string Constitution { get; set; }
        public string ConstitutionId { get; set; }
        public string IncomeMethod { get; set; }
        public string IncomeMethodId { get; set; }
        public string DueDay { get; set; }
        public string Priority { get; set; }
        public string PriorityId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTaxCode { get; set; }
        public string CompanyTaxSubCode { get; set; }
        public string Position { get; set; }
        public MafcAddressDto CompanyAddress { get; set; }
        public MafcAddressDto BranchOfficeAddress { get; set; }
        public string CompanyPhone { get; set; }
    }
}

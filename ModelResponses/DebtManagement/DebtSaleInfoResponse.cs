namespace _24hplusdotnetcore.ModelResponses.DebtManagement
{
    public class DebtSaleInfoResponse
    {
        public string Id { get; set; }
        public string Code { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
        public DebtPosResponse Pos { get; set; }
        public DebtTeamLeadResponse TeamLead { get; set; }
    }
}

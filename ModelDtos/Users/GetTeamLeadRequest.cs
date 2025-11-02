namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class GetTeamLeadRequest : PagingRequest
    {
        public string PosId { get; set; }
        public string RoleName { get; set; }
    }
}

namespace _24hplusdotnetcore.ModelDtos.Pos
{
    public class GetPosDto : PagingRequest
    {
        public string RoleId { get; set; }
        public string TeamLeadUserId { get; set; }
    }
}

using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class GetUserRequest: PagingRequest
    {
        public bool? IsActive { get; set; }
        public string PosId { get; set; }
        public string SaleChanelId { get; set; }
        public string RoleId { get; set; }
        public string TeamLeadUserId { get; set; }
        public UserStatus? Status { get; set; }
    }
}

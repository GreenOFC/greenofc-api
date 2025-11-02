using _24hplusdotnetcore.ModelDtos.Pos;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class GetUserReferralResponse
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public PosInfoDto PosInfo { get; set; }
    }
}

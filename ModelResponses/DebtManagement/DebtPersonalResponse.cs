using _24hplusdotnetcore.Models.DebtManagement;

namespace _24hplusdotnetcore.ModelResponses.DebtManagement
{
    public class DebtPersonalResponse
    {
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string FirstReferee { get; set; }
        public string SecondReferee { get; set; }
    }
}

namespace _24hplusdotnetcore.ModelDtos.Notification
{
    public class GetNotificationRequest: PagingRequest
    {
        public string UserId { get; set; }
        public string GreenType { get; set; }
        public bool IsUnread { get; set; }
    }
}

using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Ticket;
using _24hplusdotnetcore.Services.FCM;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Ticket
{
    public interface ITicketNotificationService
    {
        Task PushNotificationAsync(TicketModel ticket, string currentUserId, string type);
    }
    public class TicketNotificationService : ITicketNotificationService, IScopedLifetime
    {
        private readonly ILogger<TicketNotificationService> _logger;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserServices _userServices;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPushNotiService _pushNotiService;

        public TicketNotificationService(
            ILogger<TicketNotificationService> logger,
            ITicketRepository ticketRepository,
            IUserServices userServices,
            INotificationRepository notificationRepository,
            IPushNotiService pushNotiService)
        {
            _logger = logger;
            _ticketRepository = ticketRepository;
            _userServices = userServices;
            _notificationRepository = notificationRepository;
            _pushNotiService = pushNotiService;
        }

        public async Task PushNotificationAsync(TicketModel ticket, string currentUserId, string type)
        {
            var userIds = await _userServices.GetMemberByPermission(ticket.Creator,
                    PermissionCost.AdminPermission.Admin_Ticket_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_Ticket_ViewAll,
                    PermissionCost.AsmPermission.Asm_Ticket_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_Ticket_ViewAll);

            if (!userIds.Any())
            {
                return;
            }

            var notifications = userIds
                .Select(userId => new Notification
                {
                    UserId = userId,
                    Type = type,
                    GreenType = GreenType.Ticket,
                    RecordId = ticket.Id,
                    Title = Message.NOTIFICATION_TICKET_TITLE,
                    Message = string.Format(type == NotificationType.Add ? Message.NOTIFICATION_TICKET_ADMIN_TEMPLATE : Message.NOTIFICATION_TICKET_SALES_TEMPLATE, ticket.Code)
                })
                .ToList();

            await _notificationRepository.InsertManyAsync(notifications);

            foreach (var notification in notifications)
            {
                await _pushNotiService.PushNotificationAsync(notification.UserId, notification.RecordId, notification.GreenType, notification.Message);
            }
        }
    }
}

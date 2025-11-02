using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Notification;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.FCM;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface INotificationServices
    {
        Task<PagingResponse<GetNotificationResponse>> GetAsync(GetNotificationRequest request);
        Task<long> CountAsync(GetNotificationRequest request);
        Task UpdateIsReadAsync(string id, bool isRead);
        Task ReadNewsAsync(string recordId);
        Task CreateOneAsync(Notification noti);
    }
    public class NotificationServices : INotificationServices, IScopedLifetime
    {
        private readonly ILogger<NotificationServices> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IPushNotiService _pushNotiService;
        public NotificationServices(
            ILogger<NotificationServices> logger,
            INotificationRepository notificationRepository,
            IUserLoginService userLoginService,
            IPushNotiService pushNotiService)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
            _userLoginService = userLoginService;
            _pushNotiService = pushNotiService;
        }
        public async Task<PagingResponse<GetNotificationResponse>> GetAsync(GetNotificationRequest request)
        {
            try
            {
                var listOfNews = await _notificationRepository.GetAsync(request.UserId, request.IsUnread, request.GreenType, request.PageIndex, request.PageSize);

                var total = await _notificationRepository.CountAsync(request.UserId, request.IsUnread, request.GreenType);

                var result = new PagingResponse<GetNotificationResponse>
                {
                    TotalRecord = total,
                    Data = listOfNews
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<long> CountAsync(GetNotificationRequest request)
        {
            try
            {
                return await _notificationRepository.CountAsync(request.UserId, request.IsUnread, request.GreenType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateOneAsync(Notification noti)
        {
            try
            {
                await _notificationRepository.InsertOneAsync(noti);
                await _pushNotiService.PushNotificationAsync(noti.UserId, noti.Id, noti.GreenType, noti.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public async Task UpdateIsReadAsync(string id, bool isRead)
        {
            try
            {
                var noti = await _notificationRepository.FindOneAsync(x => x.Id == id);
                if (noti == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Notification)));
                }
                noti.IsRead = isRead;
                await _notificationRepository.ReplaceOneAsync(noti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public async Task ReadNewsAsync(string recordId)
        {
            try
            {
                string userId = _userLoginService.GetUserId();
                var noti = await _notificationRepository.FindOneAsync(x => x.RecordId == recordId && x.UserId == userId);
                if (noti == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Notification)));
                }
                if (!noti.IsRead)
                {
                    noti.IsRead = true;
                    await _notificationRepository.ReplaceOneAsync(noti);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
   
    }
}

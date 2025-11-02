using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.News;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.FCM;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface INewsServices
    {
        Task<PagingResponse<GetNewsResponse>> GetAsync(GetNewsRequest getNewsRequest);
        Task<IEnumerable<GetNewsResponse>> GetTopAsync(int number);
        Task<GetDetailNewsResponse> GetDetailAsync(string id);
        Task CreateAsync(CreateNewsRequest createNewsRequest);
        Task UpdateAsync(string id, UpdateNewsRequest updateNewsRequest);
        Task DeleteAsync(string id);
    }

    public class NewsServices : INewsServices, IScopedLifetime
    {
        private readonly ILogger<NewsServices> _logger;
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPushNotiService _pushNotiService;
        private readonly IGroupNotificationUserService _groupNotificationUserService;

        public NewsServices(
            ILogger<NewsServices> logger,
            INewsRepository newsRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserLoginRepository userLoginRepository,
            INotificationRepository notificationRepository,
            IPushNotiService pushNotiService,
            IGroupNotificationUserService groupNotificationUserService
            )
        {
            _logger = logger;
            _newsRepository = newsRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userLoginRepository = userLoginRepository;
            _notificationRepository = notificationRepository;
            _pushNotiService = pushNotiService;
            _groupNotificationUserService = groupNotificationUserService;
        }

        public async Task<PagingResponse<GetNewsResponse>> GetAsync(GetNewsRequest getNewsRequest)
        {
            try
            {
                var listOfNews = await _newsRepository.GetAsync(
                    _userLoginService.GetUserId(),
                    getNewsRequest.Type,
                    getNewsRequest.TextSearch,
                    getNewsRequest.PageIndex,
                    getNewsRequest.PageSize);

                var total = await _newsRepository.CountAsync(_userLoginService.GetUserId(), getNewsRequest.Type, getNewsRequest.TextSearch);

                var result = new PagingResponse<GetNewsResponse>
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

        public async Task<IEnumerable<GetNewsResponse>> GetTopAsync(int number)
        {
            try
            {
                var listOfNews = await _newsRepository.GetAsync(_userLoginService.GetUserId(), "", "", 1, number);

                return listOfNews;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<GetDetailNewsResponse> GetDetailAsync(string id)
        {
            try
            {
                var news = await _newsRepository.GetAsync(id);
                if (news == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(News)));
                }

                return news;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateNewsRequest request)
        {
            try
            {
                var news = _mapper.Map<News>(request);
                news.Creator = _userLoginService.GetUserId();
                await _newsRepository.InsertOneAsync(news);

                var allUser = await _groupNotificationUserService.GetUserInGroup(request.GroupNotificationIds);
                var userIds = allUser.Select(x => x.UserId).Distinct();

                var listNotis = new List<Notification>();
                foreach (var userId in userIds)
                {
                    var noti = new Notification()
                    {
                        UserId = userId,
                        Type = NotificationType.Info,
                        GreenType = GreenType.News,
                        RecordId = news.Id,
                        Message = news.Title
                    };
                    listNotis.Add(noti);
                }
                await _notificationRepository.InsertManyAsync(listNotis);

                var notiticationData = new Dictionary<string, string>()
                        {
                            { "NotificationId", new Random().Next(99999999).ToString()},
                            { "NotificationType", NotificationType.Add},
                            { "GreenType", GreenType.News},
                            { "RecordId", news.Id},
                            { "TotalNotifications", "1"}
                        };

                // push notification to each group
                foreach (var item in request.GroupNotificationIds)
                {
                    await _pushNotiService.PushNotificationToGroup(item, news.Title, news.Desc, notiticationData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateNewsRequest updateNewsRequest)
        {
            try
            {
                var news = await _newsRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (news == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(News)));
                }

                _mapper.Map(updateNewsRequest, news);
                news.Modifier = _userLoginService.GetUserId();
                news.ModifiedDate = DateTime.Now;

                await _newsRepository.ReplaceOneAsync(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var news = await _newsRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (news == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(News)));
                }

                news.IsDeleted = true;
                news.DeletedBy = _userLoginService.GetUserId();
                news.DeletedDate = DateTime.Now;

                await _newsRepository.ReplaceOneAsync(news);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

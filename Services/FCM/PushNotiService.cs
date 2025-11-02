using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Message = FirebaseAdmin.Messaging.Message;

namespace _24hplusdotnetcore.Services.FCM
{
    public interface IPushNotiService
    {
        Task PushNotificationAsync(string userId, string notificationId, string greenType, string message);
        Task PushNotificationToGroup(string groupNotificationId, string title, string messageContent, Dictionary<string, string> data = null);
        void PushNotificationToRegistrationTokens(string[] registrationTokens, string title, string messageContent, Dictionary<string, string> data = null);
    }
    public class PushNotiService : IPushNotiService, IScopedLifetime
    {
        private readonly ILogger<PushNotiService> _logger;
        private readonly UserLoginServices _userLoginservices;
        private readonly IUserServices _userServices;
        private readonly INotificationRepository _notificationRepository;
        private readonly IConfiguration _config;
        private readonly IUserLoginService _userLoginService;

        private readonly IGroupNotificationService _groupNotificationService;
        private readonly IGroupNotificationUserService _groupNotificationUserService;

        public PushNotiService(
            ILogger<PushNotiService> logger,
            UserLoginServices userLoginServices,
            IUserServices userServices,
            INotificationRepository notificationRepository,
            IUserLoginService userLoginService,
            IGroupNotificationService groupNotificationService,
            IGroupNotificationUserService groupNotificationUserService,
            IConfiguration config)
        {
            _logger = logger;
            _userLoginservices = userLoginServices;
            _userServices = userServices;
            _notificationRepository = notificationRepository;
            _config = config;
            _userLoginService = userLoginService;
            _groupNotificationService = groupNotificationService;
            _groupNotificationUserService = groupNotificationUserService;
        }
        public async Task PushNotificationAsync(string userId, string notificationId, string greenType, string message)
        {
            try
            {
                var client = new RestClient("https://fcm.googleapis.com/fcm/send");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", " application/json");
                request.AddHeader("Authorization", " key=" + _config["FireBase:ServerKey"] + "");

                FireBaseNotification firebaseNoti = new FireBaseNotification();
                firebaseNoti.From = _config["FireBase:Sender"];
                firebaseNoti.CollapseKey = _config["FireBase:collapseKey"];
                firebaseNoti.Notification = new NotificationFireBase
                {
                    Body = message
                };
                firebaseNoti.RegistrationIds = _userLoginservices.GetListTokens(userId);
                firebaseNoti.Data = new Data
                {
                    NotificationId = new Random().Next(99999999),
                    NotificationType = NotificationType.Add,
                    GreenType = greenType,
                    RecordId = notificationId,
                    TotalNotifications = 1
                };

                request.AddParameter(" application/json", "" + JsonConvert.SerializeObject(firebaseNoti) + "", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }

        public async Task PushNotificationToGroup(string groupNotificationId, string title, string messageContent, Dictionary<string, string> data = null)
        {
            try
            {
                var groupDetail = await _groupNotificationService.GetById(groupNotificationId);
                if (groupDetail != null && groupDetail.Data != null)
                {
                    var userInGroup = await _groupNotificationUserService.GetUserRegistrationToken(groupNotificationId);
                    var tokens = userInGroup.Select(x => x.UserLoginLookupResult?.registration_token)
                        .Where(x => !x.IsEmpty())
                        .ToList();

                    if (tokens.Any())
                    {
                        var message = new MulticastMessage()
                        {
                            Tokens = tokens,
                            Data = data,
                            Notification = new FirebaseAdmin.Messaging.Notification
                            {
                                Body = messageContent,
                                Title = title
                            }
                        };
                        var response = FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void PushNotificationToRegistrationTokens(string[] registrationTokens, string title, string messageContent, Dictionary<string, string> data = null)
        {
            try
            {
                if (registrationTokens.Any())
                {
                    var message = new MulticastMessage()
                    {
                        Tokens = registrationTokens,
                        Data = data,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Body = messageContent,
                            Title = title
                        }
                    };
                    var response = FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).Result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

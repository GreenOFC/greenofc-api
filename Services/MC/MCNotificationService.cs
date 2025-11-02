
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace _24hplusdotnetcore.Services.MC
{
    public class MCNotificationService : IScopedLifetime
    {
        private readonly ILogger<MCNotificationService> _logger;
        private readonly IMongoCollection<MCNotificationModel> _collection;
        private readonly IMapper _mapper;
        public MCNotificationService(
            IMongoDbConnection connection,
            IMapper mapper,
            ILogger<MCNotificationService> logger)
        {
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<MCNotificationModel>(MongoCollection.MCNotification);
            _logger = logger;
            _mapper = mapper;
        }

        public MCNotificationModel CreateOne(MCNotificationDto noti)
        {
            try
            {
                MCNotificationModel mcNoti = new MCNotificationModel();
                mcNoti.MCId = noti.Id;
                mcNoti.AppId = noti.AppId;
                mcNoti.AppNumber = noti.AppNumber;
                mcNoti.CurrentStatus = noti.CurrentStatus;
                _collection.InsertOne(mcNoti);
                return mcNoti;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public PagingResponse<GetMCNotiResponse> GetListNoti(GetMCNotiRequest request)
        {
            try
            {
                var filter = GetFilter(request.TextSearch, request.FromDate, request.ToDate);
                var count = _collection.Find(filter).ToList().Count;
                var result = _collection.Find(filter)
                    .SortByDescending(c => c.CreateDate)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Limit(request.PageSize)
                    .ToList();
                return new PagingResponse<GetMCNotiResponse>
                {
                    TotalRecord = count,
                    Data = _mapper.Map<IEnumerable<GetMCNotiResponse>>(result)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        private FilterDefinition<MCNotificationModel> GetFilter(string textSearch, string fromDate, string toDate)
        {
            var filter = Builders<MCNotificationModel>.Filter.Empty;
            string[] format = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
            DateTime _datefrom = DateTime.Now.AddDays(-30);
            DateTime _dateto = DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime.TryParseExact(fromDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _datefrom);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime.TryParseExact(toDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _dateto);
            }

            filter &= Builders<MCNotificationModel>.Filter.Gte(c => c.CreateDate, _datefrom) & Builders<MCNotificationModel>.Filter.Lte(c => c.CreateDate, _dateto);
            if (!string.IsNullOrEmpty(textSearch))
            {
                filter &= Builders<MCNotificationModel>.Filter.Regex(c => c.AppNumber, ".*" + textSearch + ".*");
            }
            return filter;
        }
    }
}

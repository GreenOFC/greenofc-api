using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public class ChecklistService : IScopedLifetime
    {
        private readonly ILogger<ChecklistService> _logger;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<ChecklistModel> _collection;

        public ChecklistService(
            IMongoDbConnection connection,
            ILogger<ChecklistService> logger,
            IMapper mapper,
            DataCRMProcessingServices dataCRMProcessingServices)
        {
            _logger = logger;
            _mapper = mapper;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<ChecklistModel>(MongoCollection.Checklist);
        }

        public bool CreateOne(ChecklistModel model)
        {
            try
            {
                _collection.InsertOne(model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        public ChecklistModel GetCheckListByCategoryId(string categoryId)
        {
            try
            {
                ChecklistModel checklist = _collection.Find(x => x.CategoryId == categoryId).FirstOrDefault();
                return checklist;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

    }
}

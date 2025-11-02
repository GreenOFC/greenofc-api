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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public class MAFCStatusService : IScopedLifetime
    {
        private readonly ILogger<MAFCStatusService> _logger;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<MAFCStatusModel> _collection;

        public MAFCStatusService(
            IMongoDbConnection connection,
            ILogger<MAFCStatusService> logger,
            IMapper mapper,
            DataCRMProcessingServices dataCRMProcessingServices)
        {
            _logger = logger;
            _mapper = mapper;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<MAFCStatusModel>(MongoCollection.MAFCStatus);
        }

        public bool CreateOne(MAFCStatusModel model)
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

        public List<MAFCStatusModel> GetListByQuery(string textSearch, string fromDate, string toDate, int? pagenumber, int? pagesize, ref int totalPage, ref int totalrecord)
        {
            var result = new List<MAFCStatusModel>();
            try
            {
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

                int _pagesize = !pagesize.HasValue ? Common.Config.PageSize : (int)pagesize;
                var filterList = Builders<MAFCStatusModel>.Filter.Gte(c => c.CreatedTime, _datefrom) & Builders<MAFCStatusModel>.Filter.Lte(c => c.CreatedTime, _dateto);

                if (!string.IsNullOrEmpty(textSearch))
                {
                    var filterSearch = Builders<MAFCStatusModel>.Filter.Or(
                        Builders<MAFCStatusModel>.Filter.Regex(c => c.Client_name, ".*" + textSearch.ToUpper() + ".*"),
                        Builders<MAFCStatusModel>.Filter.Regex(c => c.Id_f1, ".*" + textSearch + ".*"));
                    filterList = filterList & filterSearch;
                }

                var lstCount = _collection.Find(filterList).SortBy(c => c.CreatedTime).ToList().Count;
                result = _collection.Find(filterList).SortByDescending(c => c.CreatedTime)
               .Skip((pagenumber != null && pagenumber > 0) ? ((pagenumber - 1) * _pagesize) : 0).Limit(_pagesize).ToList();

                totalrecord = lstCount;
                if (lstCount == 0)
                {
                    totalPage = 0;
                }
                else
                {
                    if (lstCount <= _pagesize)
                    {
                        totalPage = 1;
                    }
                    else
                    {
                        totalPage = lstCount / _pagesize + ((lstCount % _pagesize) > 0 ? 1 : 0);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return result;
            }
        }

    }
}

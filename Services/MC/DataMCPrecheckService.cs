
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public class DataMCPrecheckService : IScopedLifetime
    {
        private readonly ILogger<DataMCPrecheckService> _logger;
        private readonly IMongoCollection<DataMCPrecheckModel> _collection;
        public DataMCPrecheckService(IMongoDbConnection connection,
            ILogger<DataMCPrecheckService> logger)
        {
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<DataMCPrecheckModel>(MongoCollection.DataMCPrecheck);
            _logger = logger;
        }

        public void CreateOne(DataMCPrecheckModel model)
        {
            try
            {
                _collection.InsertOne(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public async Task UpdatePayloadAsync(string id, PayloadModel payload)
        {
            try
            {
                var current = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (current != null)
                {
                    var temp = new List<PayloadModel>();
                    if (current.Payloads != null)
                    {
                        temp = current.Payloads.ToList();
                        temp.Add(payload);
                    }
                    else
                    {
                        temp.Add(payload);
                    }
                    current.Payloads = temp;
                    await _collection.ReplaceOneAsync(x => x.Id == current.Id, current);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public async Task UpdateSubmitedDateAsync(string customerId)
        {
            try
            {
                var current = await _collection.Find(x => x.CustomerId == customerId).FirstOrDefaultAsync();
                if (current != null)
                {
                    current.SubmitedDate = DateTime.Now;
                    await _collection.ReplaceOneAsync(x => x.Id == current.Id, current);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<DataMCPrecheckModel> GetByCustomerIdAsync(string customerId, string idCard)
        {
            try
            {
                return await _collection.Find(x => x.CustomerId == customerId && x.IdCard == idCard).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }


        public List<DataMCPrecheckModel> GetListByQuery(string textSearch, string fromDate, string toDate, int? pagenumber, int? pagesize, ref int totalPage, ref int totalrecord)
        {
            var result = new List<DataMCPrecheckModel>();
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
                var filterList = Builders<DataMCPrecheckModel>.Filter.Gte(c => c.CreateDate, _datefrom) & Builders<DataMCPrecheckModel>.Filter.Lte(c => c.CreateDate, _dateto);

                if (!string.IsNullOrEmpty(textSearch))
                {
                    var filterSearch = Builders<DataMCPrecheckModel>.Filter.Or(
                        Builders<DataMCPrecheckModel>.Filter.Regex(c => c.CustomerName, ".*" + textSearch.ToUpper() + ".*"),
                        Builders<DataMCPrecheckModel>.Filter.Regex(c => c.IdCard, ".*" + textSearch + ".*"));
                    filterList = filterList & filterSearch;
                }

                var lstCount = _collection.Find(filterList).SortBy(c => c.CreateDate).ToList().Count;
                result = _collection.Find(filterList).SortByDescending(c => c.CreateDate)
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

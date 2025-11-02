using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IPosRepository
    {
        Task<POS> GetDetailAsync(string id);
        Task<POS> Create(POS pos);
        Task Update(string posId, UpdateDefinition<POS> posUpdate);
        Task<POS> Delete(string id);

        Task<IEnumerable<POS>> GetListAsync(int pageIndex, int pageSize, string textSearch, IEnumerable<string> creators = null, bool? hasSaleChanelInfo = null);
        IEnumerable<POS> GetAll();
        Task<long> CountAsync(string textSearch, IEnumerable<string> creators = null, bool? hasSaleChanelInfo = null);
        Task UpdateSaleChanelAsync(string id, SaleChanelInfo saleChanelInfo, string modifier);
        Task UpdateListSaleChanelAsync(IEnumerable<string> id, SaleChanelInfo saleChanelInfo, string modifier);
        Task<IEnumerable<POS>> GetByIdAsync(IEnumerable<string> ids);
    }

    public class PosRepository : IPosRepository, IScopedLifetime
    {

        private readonly ILogger<PosRepository> _logger;
        private readonly IMongoRepository<POS> _posRepository;

        public PosRepository(ILogger<PosRepository> logger, IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _posRepository = posRepository;
        }

        public async Task<POS> Create(POS pos)
        {
            try
            {
                await _posRepository.InsertOneAsync(pos);
                return pos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<POS> Delete(string id)
        {
            try
            {
                var posDetail = await _posRepository.FindByIdAsync(id);
                await _posRepository.DeleteByIdAsync(posDetail.Id);
                return posDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<POS> GetDetailAsync(string id)
        {
            try
            {
                var posDetail = await _posRepository.FindByIdAsync(id);
                return posDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<long> CountAsync(string textSearch, IEnumerable<string> creators = null, bool? hasSaleChanelInfo = null)
        {
            try
            {
                var filter = GetFilter(creators, textSearch, hasSaleChanelInfo);

                var totalPos = await _posRepository.GetCollection()
                    .Aggregate()
                    .Match(filter)
                    .ToListAsync();

                return totalPos.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<POS>> GetListAsync(int pageIndex, int pageSize, string textSearch, IEnumerable<string> creators = null, bool? hasSaleChanelInfo = null)
        {
            try
            {
                var filter = GetFilter(creators, textSearch, hasSaleChanelInfo);

                var posList = await _posRepository.GetCollection()
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.CreatedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .ToListAsync();

                return posList;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<POS> GetAll()
        {
            try
            {

                return _posRepository.FilterBy(x => x.IsDeleted == false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Update(string posId, UpdateDefinition<POS> posUpdate)
        {
            try
            {
                await _posRepository.UpdateOneAsync(x => x.Id == posId, posUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateSaleChanelAsync(string id, SaleChanelInfo saleChanelInfo, string modifier)
        {
            var update = Builders<POS>.Update
                        .Set(x => x.SaleChanelInfo, saleChanelInfo)
                        .Set(x => x.ModifiedDate, DateTime.Now)
                        .Set(x => x.Modifier, modifier);

            await _posRepository.UpdateOneAsync(x => x.Id == id, update);
        }
        public async Task UpdateListSaleChanelAsync(IEnumerable<string> ids, SaleChanelInfo saleChanelInfo, string modifier)
        {
            try
            {
                var update = Builders<POS>.Update
                            .Set(x => x.SaleChanelInfo, saleChanelInfo)
                            .Set(x => x.ModifiedDate, DateTime.Now)
                            .Set(x => x.Modifier, modifier);

                await _posRepository.UpdateManyAsync(x => ids.Contains(x.Id), update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<POS>> GetByIdAsync(IEnumerable<string> ids)
        {
            return await _posRepository.FilterByAsync(x => x.IsDeleted == false && ids.Contains(x.Id));
        }

        private FilterDefinition<POS> GetFilter(IEnumerable<string> creators, string textSearch, bool? hasSaleChanelInfo = null)
        {
            var filter = Builders<POS>.Filter.Ne(x => x.IsDeleted, true);

            if (!string.IsNullOrEmpty(textSearch))
            {
                filter &= Builders<POS>.Filter.Regex(x => x.Name, new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i"));
            }

            if (creators?.Any() == true)
            {
                filter &= Builders<POS>.Filter.In(x => x.Creator, creators);
            }

            if (hasSaleChanelInfo == true)
            {
                filter &= Builders<POS>.Filter.Ne(x => x.SaleChanelInfo.Id, null);
            }

            if (hasSaleChanelInfo == false)
            {
                filter &= Builders<POS>.Filter.Eq(x => x.SaleChanelInfo.Id, null);
            }

            return filter;
        }
    }
}

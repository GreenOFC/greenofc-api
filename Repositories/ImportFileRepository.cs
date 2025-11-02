using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
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
    public interface IImportFileRepository
    {
        Task<ImportFile> Create(ImportFile request);
        Task<IEnumerable<ImportFile>> GetList(ImportFilePagingRequest request, IEnumerable<string> creatorIds);
        Task<long> Count(ImportFilePagingRequest request, IEnumerable<string> creatorIds);
    }

    public class ImportFileRepository : IImportFileRepository, IScopedLifetime
    {
        private readonly ILogger<ImportFileRepository> _logger;
        private readonly IMongoRepository<ImportFile> _importFileRepository;

        public ImportFileRepository(ILogger<ImportFileRepository> logger, IMongoRepository<ImportFile> importFileRepository)
        {
            _logger = logger;
            _importFileRepository = importFileRepository;
        }

        public async Task<ImportFile> Create(ImportFile request)
        {
            try
            {
                await _importFileRepository.InsertOneAsync(request);
                return request;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private FilterDefinition<ImportFile> GetFilter(ImportFilePagingRequest request, IEnumerable<string> creatorIds)
        {
            var filter = Builders<ImportFile>.Filter.Empty;

            filter &= Builders<ImportFile>.Filter.Eq(x => x.ImportType, request.Type);

            if (!request.TextSearch.IsEmpty())
            {
                var regex = new BsonRegularExpression($"/{request.TextSearch}/i");

                filter &= Builders<ImportFile>.Filter.Regex(x => x.FileName, regex);
            }

            if (creatorIds.Any())
            {
                filter &= Builders<ImportFile>.Filter.In(x => x.Creator, creatorIds);
            }

            if (!request.Sale.IsEmpty())
            {
                var regex = new BsonRegularExpression($"/{request.Sale}/i");

                filter &= Builders<ImportFile>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                        Builders<ImportFile>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }

            filter &= Builders<ImportFile>.Filter.Gte(x => x.CreatedDate, request.GetFromDate());
            filter &= Builders<ImportFile>.Filter.Lte(x => x.CreatedDate, request.GetToDate());

            return filter;
        }

        public async Task<IEnumerable<ImportFile>> GetList(ImportFilePagingRequest request, IEnumerable<string> creatorIds)
        {
            try
            {
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var filter = GetFilter(request, creatorIds);

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "ImportType", 1 },
                        { "FileName", 1 },
                        { "Extensions", 1 },
                        { "FileSize", 1 },
                        { "TotalRecords", 1 },
                        { "IsSuccess", 1 },
                        { "SaleInfomation", 1 },
                        { "CreatedDate", 1 },
                        { "Modifier", 1 },
                        { "ModifiedDate", 1 }

                    };

                IEnumerable<ImportFile> result = await _importFileRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(filter)
                                                                            .Project(mapping)
                                                                            .As<ImportFile>()
                                                                            .SortByDescending(x => x.CreatedDate)
                                                                            .Skip((request.PageIndex - 1) * request.PageSize)
                                                                            .Limit(request.PageSize)
                                                                            .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
        public async Task<long> Count(ImportFilePagingRequest request, IEnumerable<string> creatorIds)
        {
            try
            {
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var filter = GetFilter(request, creatorIds);

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "ImportType", 1 },
                        { "FileName", 1 },
                        { "Extensions", 1 },
                        { "FileSize", 1 },
                        { "TotalRecords", 1 },
                        { "IsSuccess", 1 },
                        { "SaleInfomation", 1 },
                        { "CreatedDate", 1 },
                        { "Modifier", 1 },
                        { "ModifiedDate", 1 }
                    };

                IEnumerable<ImportFile> result = await _importFileRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(filter)
                                                                            .Project(mapping)
                                                                            .As<ImportFile>()
                                                                            .ToListAsync();

                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
    }
}

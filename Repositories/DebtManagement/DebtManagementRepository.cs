using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.DebtManagement;
using _24hplusdotnetcore.Models.DebtManagement;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.DebtManagement
{

    public interface IDebtManagementRepository
    {
        Task<DebtManagementModel> GetDetailAsync(string id);
        Task<DebtManagementModel> Create(DebtManagementModel request);
        Task UpdateAsync(string id, DebtManagementModel request);
        Task Delete(string id);
        Task<IEnumerable<DebtManagementModel>> GetList(GetDebtDto request, IEnumerable<string> userIds, string type);
        Task<long> Count(GetDebtDto request, IEnumerable<string> userIds, string type);
        Task<List<DebtManagementModel>> Import(List<DebtManagementModel> models);
        Task<List<DebtManagementModel>> ImportOverDueDate(List<DebtManagementModel> models);
        Task<IEnumerable<DebtManagementModel>> GetExportData(GetDebtDto request, IEnumerable<string> userIds, string type);
    }

    public class DebtManagementRepository : IDebtManagementRepository, IScopedLifetime
    {
        private readonly ILogger<DebtManagementRepository> _logger;
        private readonly IMongoRepository<DebtManagementModel> _debtManageRepository;
        private readonly ICounterRepository _counterRepository;

        public DebtManagementRepository(ILogger<DebtManagementRepository> logger, IMongoRepository<DebtManagementModel> debtManageRepository, ICounterRepository counterRepository)
        {
            _logger = logger;
            _debtManageRepository = debtManageRepository;
            _counterRepository = counterRepository;
        }

        public async Task<DebtManagementModel> Create(DebtManagementModel request)
        {
            try
            {
                await _debtManageRepository.InsertOneAsync(request);
                return request;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                if (!id.IsEmpty())
                {
                    UpdateOptions updateOptions = new UpdateOptions
                    {
                        IsUpsert = false
                    };

                    var update = Builders<DebtManagementModel>.Update
                        .Set(x => x.IsDeleted, true)
                        .Set(x => x.DeletedDate, DateTime.Now);

                    await _debtManageRepository.UpdateOneAsync(x => x.Id == id, update, updateOptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<DebtManagementModel> GetDetailAsync(string id)
        {
            try
            {
                return await _debtManageRepository.FindOneAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private FilterDefinition<DebtManagementModel> GetFilter(GetDebtDto request, IEnumerable<string> userIds, string type)
        {
            var filter = Builders<DebtManagementModel>.Filter.Eq(x => x.Type, type);

            if (!request.TextSearch.IsEmpty())
            {
                var regex = new BsonRegularExpression($"/{request.TextSearch}/i");

                filter &= Builders<DebtManagementModel>.Filter.Regex(x => x.Personal.Name, regex) |
                          Builders<DebtManagementModel>.Filter.Regex(x => x.Personal.IdCard, regex) |
                          Builders<DebtManagementModel>.Filter.Regex(x => x.Personal.Phone, regex);
            }

            if (userIds?.Any() == true)
            {
                filter &= Builders<DebtManagementModel>.Filter.In(x => x.SaleInfo.Id, userIds);
            }

            if (!request.ContractCode.IsEmpty())
            {
                filter &= Builders<DebtManagementModel>.Filter.Eq(x => x.ContractCode, request.ContractCode);
            }
            if (!request.Sale.IsEmpty())
            {
                var regex = new BsonRegularExpression($"/{request.Sale}/i");

                filter &= Builders<DebtManagementModel>.Filter.Regex(x => x.SaleInfo.FullName, regex) |
                        Builders<DebtManagementModel>.Filter.Regex(x => x.SaleInfo.Code, regex);
            }

            if (!request.PosManager.IsEmpty())
            {
                var regex = new BsonRegularExpression($"/{request.PosManager}/i");
                filter &= Builders<DebtManagementModel>.Filter.Regex(x => x.SaleInfo.Pos.Name, regex);
            }

            if (!request.TeamLead.IsEmpty())
            {
                var regex = new BsonRegularExpression($"/{request.TeamLead}/i");
                filter &= Builders<DebtManagementModel>.Filter.Regex(x => x.SaleInfo.TeamLead.FullName, regex) |
                    Builders<DebtManagementModel>.Filter.Regex(x => x.SaleInfo.TeamLead.UserName, regex);
            }

            filter &= Builders<DebtManagementModel>.Filter.Ne(x => x.Loan.Amount, "0");

            filter &= Builders<DebtManagementModel>.Filter.Gte(x => x.ModifiedDate, request.GetFromDate());
            filter &= Builders<DebtManagementModel>.Filter.Lte(x => x.ModifiedDate, request.GetToDate());

            return filter;
        }


        public async Task<IEnumerable<DebtManagementModel>> GetList(GetDebtDto request, IEnumerable<string> userIds, string type)
        {
            try
            {
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var filter = GetFilter(request, userIds, type);

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "ContractCode", 1 },
                        { "GreenType", 1 },
                        { "Personal", 1 },
                        { "Loan", 1 },
                        { "SaleInfo", 1 },
                        { "CreatedDate", 1 },
                        { "Modifier", 1 },
                        { "ModifiedDate", 1 },
                        { "ActualUpdatedDate", 1 },
                        { "ModifierInfo", 1 },
                        { "NumberOverDueDate", 1 },
                        { "OverDueDate", 1 },
                        { "Type", 1 },
                        { "RowNumber", 1 }
                    };

                IEnumerable<DebtManagementModel> result = await _debtManageRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(filter)
                                                                            .Project(mapping)
                                                                            .As<DebtManagementModel>()
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
        public async Task<long> Count(GetDebtDto request, IEnumerable<string> userIds, string type)
        {
            try
            {
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var filter = GetFilter(request, userIds, type);

                var mapping = new BsonDocument()
                    {
                        { "_id", 1 },
                        { "ContractCode", 1 },
                        { "GreenType", 1 },
                        { "Personal", 1 },
                        { "Loan", 1 },
                        { "SaleInfo", 1 },
                        { "CreatedDate", 1 },
                        { "Modifier", 1 },
                        { "ModifiedDate", 1 },
                        { "ModifierInfo", 1 },
                        { "NumberOverDueDate", 1 },
                        { "OverDueDate", 1 },
                        { "Type", 1 },
                        { "RowNumber", 1 }
                    };

                IEnumerable<DebtManagementModel> result = await _debtManageRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(filter)
                                                                            .Project(mapping)
                                                                            .As<DebtManagementModel>()
                                                                            .ToListAsync();

                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task UpdateAsync(string id, DebtManagementModel request)
        {
            try
            {
                request.ModifiedDate = DateTime.Now;
                await _debtManageRepository.ReplaceOneAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<DebtManagementModel>> Import(List<DebtManagementModel> models)
        {
            try
            {
                UpdateOptions updateOptions = new UpdateOptions
                {
                    IsUpsert = true
                };

                var updateTasks = new List<Task>();

                foreach (var debt in models)
                {
                    var update = Builders<DebtManagementModel>.Update
                                .Set(x => x.ContractCode, debt.ContractCode)
                                .Set(x => x.GreenType, debt.GreenType)
                                .Set(x => x.Personal, debt.Personal)
                                .Set(x => x.Type, debt.Type)
                                .Set(x => x.SaleInfo, debt.SaleInfo)
                                .Set(x => x.Loan, debt.Loan)
                                .Set(x => x.Modifier, debt.Modifier)
                                .Set(x => x.ModifiedDate, debt.ModifiedDate)
                                .Set(x => x.ModifierInfo, debt.ModifierInfo)
                                .Set(x => x.ActualUpdatedDate, debt.ActualUpdatedDate)
                                .Set(x => x.RowNumber, debt.RowNumber);

                    var updateTask = _debtManageRepository
                        .UpdateOneAsync(x => x.ContractCode == debt.ContractCode
                        && x.Type == DebtManagementImportType.COMMING.ToString(), update, updateOptions);
                    updateTasks.Add(updateTask);
                }

                await Task.WhenAll(updateTasks);

                var contractCodes = models.Select(x => x.ContractCode);
                var response = (await _debtManageRepository.FilterByAsync(x => contractCodes.Contains(x.ContractCode))).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<DebtManagementModel>> ImportOverDueDate(List<DebtManagementModel> models)
        {
            try
            {
                UpdateOptions updateOptions = new UpdateOptions
                {
                    IsUpsert = true
                };

                var updateTasks = new List<Task>();

                foreach (var debt in models)
                {
                    var update = Builders<DebtManagementModel>.Update
                                .Set(x => x.ContractCode, debt.ContractCode)
                                .Set(x => x.GreenType, debt.GreenType)
                                .Set(x => x.Personal, debt.Personal)
                                .Set(x => x.Type, debt.Type)
                                .Set(x => x.SaleInfo, debt.SaleInfo)
                                .Set(x => x.Loan, debt.Loan)
                                .Set(x => x.Modifier, debt.Modifier)
                                .Set(x => x.ModifiedDate, debt.ModifiedDate)
                                .Set(x => x.ModifierInfo, debt.ModifierInfo)
                                .Set(x => x.ActualUpdatedDate, debt.ActualUpdatedDate)
                                .Set(x => x.OverDueDate, debt.OverDueDate)
                                .Set(x => x.NumberOverDueDate, debt.NumberOverDueDate)
                                .Set(x => x.RowNumber, debt.RowNumber);

                    var updateTask = _debtManageRepository
                        .UpdateOneAsync(x => x.ContractCode == debt.ContractCode
                        && debt.Type == DebtManagementImportType.OVERDUE.ToString(), update, updateOptions);

                    updateTasks.Add(updateTask);
                }

                await Task.WhenAll(updateTasks);

                var contractCodes = models.Select(x => x.ContractCode);
                var response = (await _debtManageRepository.FilterByAsync(x => contractCodes.Contains(x.ContractCode))).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<DebtManagementModel>> GetExportData(GetDebtDto request, IEnumerable<string> userIds, string type)
        {
            try
            {
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var filter = GetFilter(request, userIds, type);



                IEnumerable<DebtManagementModel> result = await _debtManageRepository
                                                                            .GetCollection()
                                                                            .Aggregate()
                                                                            .Match(filter)
                                                                            .As<DebtManagementModel>()
                                                                            .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

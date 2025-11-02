using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.BatchJob
{
    public class DataSeedingJob : IHostedService
    {
        private readonly ILogger<DataSeedingJob> _logger;
        private readonly IEnumerable<(string, IReadOnlyList<DataConfig>)> _dataConfigs;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public DataSeedingJob(
            ILogger<DataSeedingJob> logger,
            IMapper mapper,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            _dataConfigs = new List<(string, IReadOnlyList<DataConfig>)>
            {
                ( DataConfigType.F88Province, LeadF88SeedData.Provinces ),
                ( DataConfigType.F88SignAddress, LeadF88SeedData.SignAddress ),
                ( DataConfigType.F88Category, LeadF88SeedData.Categories ),
                ( DataConfigType.MAFCCategory, LeadMafcSeedData.Categories ),
                ( DataConfigType.LeadVibIncome, LeadVibSeedData.Incomes ),
                ( DataConfigType.LeadVibIncomeStream, LeadVibSeedData.IncomeStreams ),
                ( DataConfigType.News, NewsDataSeed.News ),
                ( DataConfigType.NewsTag, NewsDataSeed.NewsTags ),
                ( DataConfigType.LeadCimbTerm, LeadCimbSeedData.Terms ),
                ( DataConfigType.LeadCimbEmploymentStatus, LeadCimbSeedData.EmploymentStatus ),
                ( DataConfigType.LeadCimbMaritalStatus, LeadCimbSeedData.MaritalStatus ),
                ( DataConfigType.LeadCimbEducation, LeadCimbSeedData.Educations ),
                ( DataConfigType.LeadCimbReferenceContactType, LeadCimbSeedData.ReferenceContactTypes ),
                ( DataConfigType.LeadCimbCustomerType, LeadCimbSeedData.CustomerTypes ),
                ( DataConfigType.LeadCimbLoanPurpose, LeadCimbSeedData.LoanPurposes ),
                ( DataConfigType.LeadEcEmployeeType, LeadEcSeedData.EmployeeTypes ),
                ( DataConfigType.LeadEcDisbursementMethod, LeadEcSeedData.DisbursementMethods ),
                ( DataConfigType.LeadEcJobType, LeadEcSeedData.JobTypes ),
                ( DataConfigType.LeadEcRelationship, LeadEcSeedData.Relationships ),
                ( DataConfigType.LeadEcPurpose, LeadEcSeedData.Purposes ),
                ( DataConfigType.LeadEcMaritalStatus, LeadEcSeedData.MaritalStatus ),
                ( DataConfigType.LeadMcCicType, LeadMcSeedData.CicTypes ),
                ( DataConfigType.LeadMcOccupations, LeadMcSeedData.Occupations ),
                ( DataConfigType.LeadMcIncome, LeadMcSeedData.Incomes ),
                ( DataConfigType.LeadMcTerm, LeadMcSeedData.Terms ),
                ( DataConfigType.LeadMcIdCardProvince, LeadMcSeedData.IdCardProvinces ),
                ( DataConfigType.CustomerTitle, LeadMafcSeedData.CustomerTitles ),
                ( DataConfigType.MaritalStatus, LeadMafcSeedData.MaritalStatus ),
                ( DataConfigType.EducationLevel, LeadMafcSeedData.EducationLevels ),
                ( DataConfigType.PropertyStatus, LeadMafcSeedData.PropertyStatus ),
                ( DataConfigType.Relationship, LeadMafcSeedData.Relationships ),
                ( DataConfigType.Occupations, LeadMafcSeedData.Occupations ),
                ( DataConfigType.IncomeMethod, LeadMafcSeedData.IncomeMethods ),
                ( DataConfigType.LoanPurpose, LeadMafcSeedData.LoanPurposes ),
                ( DataConfigType.LoanPaymentDate, LeadMafcSeedData.LoanPaymentDates ),
                ( DataConfigType.Term, LeadMafcSeedData.Terms ),
                ( DataConfigType.WorkingPriority, LeadMafcSeedData.WorkingPriorities ),
                ( DataConfigType.Constitution, LeadMafcSeedData.Constitutions ),
                ( DataConfigType.WokingStatus, LeadMafcSeedData.WokingStatus ),
                ( DataConfigType.SecretWithType, LeadMafcSeedData.SecretWithTypes ),
                ( DataConfigType.IdCardProvince, LeadMafcSeedData.IdCardProvinces ),
                ( DataConfigType.MafcCustomerType, LeadMafcSeedData.CustomerType ),
                ( DataConfigType.Relationship, LeadMcSeedData.Relationships ),
                ( DataConfigType.LeadShinhanTerm, LeadShinhanSeedData.Terms ),
                ( DataConfigType.LeadShinhanSignAddress, LeadShinhanSeedData.LeadShinhanSignAddresses ),
                ( DataConfigType.LeadShinhanOccupation, LeadShinhanSeedData.Occupation ),
                ( DataConfigType.LeadShinhanIncomeMethod, LeadShinhanSeedData.IncomeMethod ),

                ( DataConfigType.Gender, LeadPtfSeedData.Genders ),
                ( DataConfigType.MaritalStatus, LeadPtfSeedData.MaritalStatus ),
                ( DataConfigType.DependentType, LeadPtfSeedData.DependentTypes ),
                ( DataConfigType.EducationLevel, LeadPtfSeedData.EducationLevels ),
                ( DataConfigType.Position, LeadPtfSeedData.Positions ),
                ( DataConfigType.JobType, LeadPtfSeedData.JobTypes ),
                ( DataConfigType.SocialAccount, LeadPtfSeedData.SocialAccounts ),
                ( DataConfigType.Relationship, LeadPtfSeedData.Relationships ),
                ( DataConfigType.LoanPurpose, LeadPtfSeedData.LoanPurposes ),
                ( DataConfigType.Term, LeadPtfSeedData.Terms ),
                ( DataConfigType.LoanService, LeadPtfSeedData.LoanServices ),
                ( DataConfigType.DisbursementMethod, LeadPtfSeedData.DisbursementMethods ),
                ( DataConfigType.PartnerName, LeadPtfSeedData.PartnerNames ),

            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            // ILeadEcResourceService _leadEcResourceService = scope.ServiceProvider.GetRequiredService<ILeadEcResourceService>();

            foreach (var item in _dataConfigs)
            {
                await InsertSeedDataAsync(item.Item1, item.Item2);
            }
            await PermissionAsync();
            await LeadCimbLoanInfomationAsync();
            await SeedGroupNotification();
            // await _leadEcResourceService.SyncAsync();
            await LeadPtfCategoryAsync();
            await LeadPtfChecklistAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task InsertSeedDataAsync(string dataConfigType, IReadOnlyList<DataConfig> dataConfigs)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IMongoRepository<DataConfig> _dataConfigRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<DataConfig>>();

            var entities = _dataConfigRepository.FilterBy(x => x.Type == dataConfigType);
            var entitiesToInsert = dataConfigs.Where(x => !entities.Any(y => x.Type == y.Type && x.GreenType == y.GreenType && x.Key == y.Key)).ToList();

            var entitiesToUpdate = entities
               .Where(x => dataConfigs.Any(y => x.Type == y.Type && x.GreenType == y.GreenType && x.Key == y.Key && x.Value != y.Value))
               .Select(x =>
               {
                   var dataConfig = dataConfigs.First(y => x.Type == y.Type && x.GreenType == y.GreenType && x.Key == y.Key);
                   _mapper.Map(dataConfig, x);
                   x.ModifiedDate = DateTime.Now;
                   return x;
               })
               .ToList();

            if (entitiesToInsert.Any())
            {
                await _dataConfigRepository.InsertManyAsync(entitiesToInsert);
            }
            if (entitiesToUpdate.Any())
            {
                await _dataConfigRepository.UpdateManyAsync(entitiesToUpdate);
            }
        }

        private async Task PermissionAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IMongoRepository<Permission> _permissionRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<Permission>>();

            var permissions = _permissionRepository.FilterBy(x => true);
            var permissionToInsert = PermissionSeedData.Entities.Where(x => !permissions.Any(y => y.Value == x.Value)).ToList();
            var permissionToUpdate = permissions
               .Where(x => PermissionSeedData.Entities.Any(y => y.Value == x.Value))
               .Select(x =>
               {
                   var permission = PermissionSeedData.Entities.First(y => y.Value == x.Value);
                   _mapper.Map(permission, x);
                   x.ModifiedDate = DateTime.Now;
                   return x;
               })
               .ToList();

            if (permissionToInsert.Any())
            {
                await _permissionRepository.InsertManyAsync(permissionToInsert);
            }
            if (permissionToUpdate.Any())
            {
                await _permissionRepository.UpdateManyAsync(permissionToUpdate);
            }
        }

        private async Task LeadCimbLoanInfomationAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IMongoRepository<LeadCimbLoanInfomation> _leadCimbLoanInfomationRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LeadCimbLoanInfomation>>();

            var entities = _leadCimbLoanInfomationRepository.FilterBy(x => true);
            var entityToInsert = LeadCimbSeedData.LoanInfomations.Where(x => !entities.Any(y => y.PackageSize == x.PackageSize)).ToList();
            if (entityToInsert.Any())
            {
                await _leadCimbLoanInfomationRepository.InsertManyAsync(entityToInsert);
            }
        }

        public async Task SeedGroupNotification()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            IGroupNotificationRepository groupNotificationRepository = scope.ServiceProvider.GetRequiredService<IGroupNotificationRepository>();

            var entities = await groupNotificationRepository.GetAllGroupNotification();

            var entityToInsert = GroupNotificationSeedData.GroupNotifications.Where(x => !entities.Any(y => y.GroupCode.Equals(x.GroupCode, StringComparison.InvariantCultureIgnoreCase))).ToList();

            if (entityToInsert.Any())
            {
                await groupNotificationRepository.CreateManyAsync(entityToInsert);
            }

        }

        public async Task LeadPtfCategoryAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IMongoRepository<LeadPtfCategoryGroup> _leadPtfCategoryGroupRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LeadPtfCategoryGroup>>();
            
            var entities = _leadPtfCategoryGroupRepository.FilterBy(x => true);
            var entityToInsert = LeadPtfSeedData.CategoryGroups.Where(x => !entities.Any(y => y.ProductLine == x.ProductLine)).ToList();
            var entitiesToUpdate = entities
               .Where(x => LeadPtfSeedData.CategoryGroups.Any(y => y.ProductLine == x.ProductLine))
               .Select(x =>
               {
                   var dataConfig = LeadPtfSeedData.CategoryGroups.First(y => y.ProductLine == x.ProductLine);
                   _mapper.Map(dataConfig, x);
                   x.ModifiedDate = DateTime.Now;
                   return x;
               })
               .ToList();

            if (entityToInsert.Any())
            {
                await _leadPtfCategoryGroupRepository.InsertManyAsync(entityToInsert);
            }
            if (entitiesToUpdate.Any())
            {
                await _leadPtfCategoryGroupRepository.UpdateManyAsync(entitiesToUpdate);
            }
        }

        public async Task LeadPtfChecklistAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IMongoRepository<ChecklistModel> _checkListRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<ChecklistModel>>();
            
            var entities = _checkListRepository.FilterBy(x => true);
            var entityToInsert = LeadPtfSeedData.Documents.Where(x => 
                !entities.Any(y => y.GreenType == x.GreenType && y.ProductLine == x.ProductLine)).ToList();
            var entitiesToUpdate = entities
               .Where(x => LeadPtfSeedData.Documents.Any(y => y.GreenType == x.GreenType && y.ProductLine == x.ProductLine))
               .Select(x =>
               {
                   var dataConfig = LeadPtfSeedData.Documents.First(y => y.GreenType == x.GreenType && y.ProductLine == x.ProductLine);
                   _mapper.Map(dataConfig, x);
                   return x;
               })
               .ToList();

            if (entityToInsert.Any())
            {
                await _checkListRepository.InsertManyAsync(entityToInsert);
            }
            if (entitiesToUpdate.Any())
            {
                await _checkListRepository.UpdateManyAsync(entitiesToUpdate);
            }
        }
    }
}

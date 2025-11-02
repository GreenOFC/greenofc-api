using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CIMB;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.MC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public class CustomerServices : IScopedLifetime
    {
        private readonly ILogger<CustomerServices> _logger;
        private readonly IMongoCollection<Customer> _customer;
        private readonly NotificationServices _notificationServices;
        private readonly UserServices _userServices;
        private readonly CRM.DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly MC.DataMCProcessingServices _dataMCProcessingServices;
        private readonly MAFC.DataMAFCProcessingServices _dataMAFCProcessingService;
        private readonly MCCheckCICService _mcCheckCICService;
        private readonly IUserLoginService _userLoginService;

        private readonly IMongoRepository<Customer> _genericCollection;
        private readonly IMapper _mapper;

        public CustomerServices(IMongoDbConnection connection,
            ILogger<CustomerServices> logger,
            NotificationServices notificationServices,
            UserServices userServices,
            CRM.DataCRMProcessingServices dataCRMProcessingServices,
            MC.DataMCProcessingServices dataMCProcessingServices,
            MAFC.DataMAFCProcessingServices dataMAFCProcessingService,
            MCCheckCICService mcCheckCICService,
            IUserLoginService userLoginService,
            IMongoRepository<Customer> genericCollection,
            IMapper mapper)
        {
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _customer = database.GetCollection<Customer>(MongoCollection.CustomerCollection);
            _logger = logger;
            _notificationServices = notificationServices;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _dataMCProcessingServices = dataMCProcessingServices;
            _dataMAFCProcessingService = dataMAFCProcessingService;
            _mcCheckCICService = mcCheckCICService;
            _userServices = userServices;
            _userLoginService = userLoginService;
            _genericCollection = genericCollection;
            _mapper = mapper;

        }
        public List<Customer> GetList(string greenType, string status)
        {
            var userId = _userLoginService.GetUserId();
            var lstCustomer = new List<Customer>();

            try
            {
                var filter = Builders<Customer>.Filter.Ne(u => u.IsDeleted, true);

                filter &= Builders<Customer>.Filter.Eq(c => c.Creator, userId);
                if (!string.IsNullOrEmpty(greenType))
                {
                    filter &= Builders<Customer>.Filter.Eq(c => c.GreenType, greenType);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    filter &= Builders<Customer>.Filter.Eq(c => c.Status, status);
                }
                lstCustomer = _customer.Find(filter).SortByDescending(c => c.ModifiedDate).ToList();
                return lstCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return lstCustomer;
        }

        public List<Customer> GetList(User user, string greenType, string productLine, string status, string textSearch, string fromDate, string toDate, int? pagenumber, 
            int? pagesize, ref long totalPage, ref long totalrecord, string teamLead, string posManager, string sale)
        {
            var lstCustomer = new List<Customer>();
            var listMemberNames = new List<string>();
            bool hasMember = false;
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
            var listMemmbers = _userServices.GetTeamMember(user.Id);
            if (listMemmbers != null)
            {
                foreach (var member in listMemmbers)
                {
                    listMemberNames.Add(member.UserName);
                }
                hasMember = true;
            }
            try
            {
                int _pagesize = !pagesize.HasValue ? Common.Config.PageSize : (int)pagesize;
                var filter = Builders<Customer>.Filter.Ne(u => u.IsDeleted, true); ;
                var filterMember = Builders<Customer>.Filter.Ne(u => u.IsDeleted, true); ;

                var filterCreateDate = Builders<Customer>.Filter.Gte(c => c.ModifiedDate, _datefrom) & Builders<Customer>.Filter.Lte(c => c.ModifiedDate, _dateto);
                filter = filter & filterCreateDate;
                if (hasMember)
                {
                    filterMember &= filterCreateDate & Builders<Customer>.Filter.In(c => c.UserName, listMemberNames);
                }

                filter &= Builders<Customer>.Filter.Eq(c => c.UserName, user.UserName);
                if (!string.IsNullOrEmpty(greenType))
                {
                    filter &= Builders<Customer>.Filter.Eq(c => c.GreenType, greenType);
                    if (hasMember)
                    {
                        filterMember &= Builders<Customer>.Filter.Eq(c => c.GreenType, greenType);
                    }
                }
                if (!string.IsNullOrEmpty(productLine))
                {
                    filter &= Builders<Customer>.Filter.Eq(c => c.ProductLine, productLine);
                    if (hasMember)
                    {
                        filterMember &= Builders<Customer>.Filter.Eq(c => c.ProductLine, productLine);
                    }
                }
                if (!string.IsNullOrEmpty(textSearch))
                {
                    filter &= Builders<Customer>.Filter.Or(
                        Builders<Customer>.Filter.Regex(c => c.Personal.Name, ".*" + textSearch + ".*"),
                        Builders<Customer>.Filter.Regex(c => c.Personal.IdCard, ".*" + textSearch + ".*"),
                        Builders<Customer>.Filter.Regex(c => c.ContractCode, ".*" + textSearch + ".*")
                    );
                    if (hasMember)
                    {
                        filterMember &= Builders<Customer>.Filter.Or(
                            Builders<Customer>.Filter.Regex(c => c.Personal.Name, ".*" + textSearch + ".*"),
                            Builders<Customer>.Filter.Regex(c => c.Personal.IdCard, ".*" + textSearch + ".*"),
                            Builders<Customer>.Filter.Regex(c => c.ContractCode, ".*" + textSearch + ".*")
                        );
                    }
                }
                if (!string.IsNullOrEmpty(status))
                {
                    filter &= Builders<Customer>.Filter.Eq(c => c.Status, status);

                    if (status.ToUpper() != CustomerStatus.DRAFT && hasMember)
                    {
                        filterMember &= Builders<Customer>.Filter.Eq(c => c.Status, status);
                    }
                }
                else
                {
                    if (hasMember)
                    {
                        filterMember &= Builders<Customer>.Filter.Ne(c => c.Status, CustomerStatus.DRAFT);
                    }
                }
                if (!string.IsNullOrEmpty(sale))
                {
                    var regex = new BsonRegularExpression($"/{sale.ConvertSpecialCharacters()}/i");
                    filter &= Builders<Customer>.Filter.Regex(x => x.SaleInfo.Name, regex) |
                        Builders<Customer>.Filter.Regex(x => x.SaleInfo.Code, regex);
                }
                if (!string.IsNullOrEmpty(teamLead))
                {
                    var regex = new BsonRegularExpression($"/{teamLead.ConvertSpecialCharacters()}/i");
                    filter &= Builders<Customer>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                        Builders<Customer>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
                }
                if (!string.IsNullOrEmpty(posManager))
                {
                    var regex = new BsonRegularExpression($"/{posManager.ConvertSpecialCharacters()}/i");
                    filter &= Builders<Customer>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                        Builders<Customer>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
                }
                filter = Builders<Customer>.Filter.Or(filter, filterMember);
                var lstCount = _customer.Find(filter).CountDocuments();
                lstCustomer = _customer.Find(filter).SortByDescending(c => c.ModifiedDate)
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

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return lstCustomer;
        }
        public List<Customer> GetAllList(string greenType, string productLine, string status, string textSearch, string fromDate, string toDate, 
            int? pagenumber, int? pagesize, ref long totalPage, ref long totalrecord, string teamLead, string posManager, string sale)
        {
            var lstCustomer = new List<Customer>();
            DateTime _datefrom = DateTime.Now.AddDays(-7);
            DateTime _dateto = DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime.TryParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _datefrom);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime.TryParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dateto);
            }
            try
            {
                int _pagesize = !pagesize.HasValue ? Common.Config.PageSize : (int)pagesize;

                var filterCreateDate = Builders<Customer>.Filter.Gte(c => c.ModifiedDate, _datefrom) & Builders<Customer>.Filter.Lte(c => c.ModifiedDate, _dateto);
                filterCreateDate &= Builders<Customer>.Filter.Ne(u => u.IsDeleted, true);
                if (!string.IsNullOrEmpty(greenType))
                {
                    var filterGreenType = Builders<Customer>.Filter.Eq(c => c.GreenType, greenType);
                    filterCreateDate = filterCreateDate & filterGreenType;
                }
                if (!string.IsNullOrEmpty(productLine))
                {
                    filterCreateDate &= Builders<Customer>.Filter.Eq(c => c.ProductLine, productLine);
                }
                if (!string.IsNullOrEmpty(textSearch))
                {
                    var filterCustomerName = Builders<Customer>.Filter.Or(
                        Builders<Customer>.Filter.Regex(c => c.Personal.Name, ".*" + textSearch + ".*"),
                        Builders<Customer>.Filter.Regex(c => c.Personal.IdCard, ".*" + textSearch + ".*"),
                        Builders<Customer>.Filter.Regex(c => c.ContractCode, ".*" + textSearch + ".*")
                    );
                    filterCreateDate = filterCreateDate & filterCustomerName;
                }
                if (!string.IsNullOrEmpty(status))
                {
                    var filterStatus = Builders<Customer>.Filter.Regex(c => c.Status, "/^" + status + "$/i");
                    filterCreateDate = filterCreateDate & filterStatus;
                }
                if (!string.IsNullOrEmpty(sale))
                {
                    var regex = new BsonRegularExpression($"/{sale.ConvertSpecialCharacters()}/i");
                    filterCreateDate &= Builders<Customer>.Filter.Regex(x => x.SaleInfo.Name, regex) |
                        Builders<Customer>.Filter.Regex(x => x.SaleInfo.Code, regex);
                }
                if (!string.IsNullOrEmpty(teamLead))
                {
                    var regex = new BsonRegularExpression($"/{teamLead.ConvertSpecialCharacters()}/i");
                    filterCreateDate &= Builders<Customer>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                        Builders<Customer>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
                }
                if (!string.IsNullOrEmpty(posManager))
                {
                    var regex = new BsonRegularExpression($"/{posManager.ConvertSpecialCharacters()}/i");
                    filterCreateDate &= Builders<Customer>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                        Builders<Customer>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
                }
                var lstCount = _customer.Find(filterCreateDate).CountDocuments();
                lstCustomer = _customer.Find(filterCreateDate).SortByDescending(c => c.ModifiedDate)
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

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return lstCustomer;
        }

        public Customer GetCustomer(string Id)
        {
            try
            {
                return _customer.Find(c => !c.IsDeleted && c.Id == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public List<Customer> GetCustomerByUserName(string UserName, int? pagenumber)
        {
            var lstCustomer = new List<Customer>();
            try
            {
                lstCustomer = _customer.Find(c => !c.IsDeleted && c.UserName == UserName).SortByDescending(c => c.ModifiedDate).Skip((pagenumber != null && pagenumber > 0) ? ((pagenumber - 1) * Common.Config.PageSize) : 0).Limit(Common.Config.PageSize).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return lstCustomer;
        }

        public StatusCount GetStatusCount(User user, string greenType, string productLine)
        {
            var statusCount = new StatusCount();
            try
            {
                var listMemberNames = new List<string>();
                DateTime _datefrom = DateTime.Now.AddDays(-30);
                DateTime _dateto = DateTime.Now.AddDays(1);
                var filter = Builders<Customer>.Filter.And(
                    Builders<Customer>.Filter.Ne(c => c.IsDeleted, true),
                    Builders<Customer>.Filter.Or(
                        Builders<Customer>.Filter.Eq(c => c.UserName, user.UserName),
                        Builders<Customer>.Filter.Eq(c => c.Creator, user.Id)
                    ),
                    Builders<Customer>.Filter.Eq(c => c.GreenType, greenType),
                    Builders<Customer>.Filter.Gte(c => c.ModifiedDate, _datefrom),
                    Builders<Customer>.Filter.Lte(c => c.ModifiedDate, _dateto)
                );
                if (!string.IsNullOrEmpty(productLine))
                {
                    filter &= Builders<Customer>.Filter.Eq(c => c.ProductLine, productLine);
                }
                var lstCustomer = _customer.Find(filter).ToList();

                var listMemmbers = _userServices.GetTeamMember(user.Id);
                if (listMemmbers != null)
                {
                    foreach (var member in listMemmbers)
                    {
                        listMemberNames.Add(member.UserName);
                    }
                }
                if (lstCustomer != null && lstCustomer.Count > 0)
                {
                    var statusdraft = lstCustomer.Where(l => string.Equals(l.Status, CustomerStatus.DRAFT, StringComparison.CurrentCultureIgnoreCase)).ToList().Count;

                    var statusreturn = lstCustomer.Where(l => string.Equals(l.Status, CustomerStatus.RETURN, StringComparison.CurrentCultureIgnoreCase)).ToList().Count;

                    var statussubmit = lstCustomer.Where(l => string.Equals(l.Status, CustomerStatus.SUBMIT, StringComparison.CurrentCultureIgnoreCase)).ToList().Count;

                    var statusreject = lstCustomer.Where(l => string.Equals(l.Status, CustomerStatus.REJECT, StringComparison.CurrentCultureIgnoreCase)).ToList().Count;

                    var statusapprove = lstCustomer.Where(l => string.Equals(l.Status, CustomerStatus.APPROVE, StringComparison.CurrentCultureIgnoreCase)).ToList().Count;

                    statusCount.Draft = statusdraft;
                    statusCount.Return = statusreturn + statusreject;
                    statusCount.Submit = statussubmit;
                    statusCount.Approve = statusapprove;
                    statusCount.All = lstCustomer.Count;
                }

                var listReview = 0;
                if (listMemberNames != null && listMemmbers.Count() > 0)
                {
                    var filterMember = Builders<Customer>.Filter.And(
                        Builders<Customer>.Filter.Ne(c => c.IsDeleted, true),
                        Builders<Customer>.Filter.In(c => c.UserName, listMemberNames),
                        Builders<Customer>.Filter.Eq(c => c.Status, CustomerStatus.REVIEW),
                        Builders<Customer>.Filter.Gte(c => c.ModifiedDate, _datefrom),
                        Builders<Customer>.Filter.Lte(c => c.ModifiedDate, _dateto)
                    );
                    listReview = _customer.Find(filterMember).ToList().Count;
                }
                statusCount.All += listReview;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return statusCount;
        }
        internal long[] CustomerPagesize(List<Customer> lstCustomer)
        {
            long customersize = lstCustomer.Count;
            if (customersize <= Common.Config.PageSize)
            {
                return new long[]{
                    customersize,
                    1
                };
            }
            long totalpage = (customersize % Common.Config.PageSize) > 0 ? (customersize / Common.Config.PageSize + 1) : (customersize / Common.Config.PageSize + 1);
            return new long[]{
                customersize,
                totalpage
            };
        }

        public async Task<Customer> CheckExistedCustomerAsync(string idCard, string greenType)
        {
            try
            {
                DateTime datefrom = DateTime.Now.AddDays(-15);
                var filter = Builders<Customer>.Filter.And(
                    Builders<Customer>.Filter.Gte(c => c.ModifiedDate, datefrom),
                    Builders<Customer>.Filter.Eq(c => c.GreenType, greenType),
                    Builders<Customer>.Filter.Eq(c => c.Personal.IdCard, idCard),
                    Builders<Customer>.Filter.Ne(c => c.IsDeleted, true),
                    Builders<Customer>.Filter.Nin(c => c.Status, new string[] { CustomerStatus.CANCEL, CustomerStatus.DRAFT, CustomerStatus.REJECT, CustomerStatus.SUCCESS })
                );
                return await _customer.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public IEnumerable<Customer> GetListSubmitedCustomerByIdCard(string IdCard)
        {
            try
            {
                return _customer.Find(c => !c.IsDeleted && c.Personal.IdCard == IdCard && c.Status == CustomerStatus.SUBMIT).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<Customer> GetByIdCards(IEnumerable<string> idCards)
        {
            try
            {
                return _customer.Find(c => !c.IsDeleted && idCards.Contains(c.Personal.IdCard)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<Customer> GetByIds(IEnumerable<string> ids)
        {
            try
            {
                return _customer.Find(c => !c.IsDeleted && ids.Contains(c.Id)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public Customer GetByMCId(int mcId)
        {
            try
            {
                return _customer.Find(c => !c.IsDeleted && c.MCId == mcId).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public Customer GetByMAFCId(string mafcId)
        {
            try
            {
                int id = 0;
                Int32.TryParse(mafcId, out id);
                return _customer.Find(c => !c.IsDeleted && c.MAFCId == id).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerCheckListRequestModel> GetCustomerCheckListAsync(string id)
        {
            try
            {
                var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
                filter &= Builders<Customer>.Filter.Ne(c => c.IsDeleted, true);
                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
                var projectMapping = new BsonDocument()
                {
                   {"_id", 0 },
                   {"MobileSchemaProductCode", "$Loan.ProductObj.ProductCodeMC" },
                   {"MobileTemResidence", new BsonDocument("$toInt", "$IsTheSameResidentAddress") },
                   {"LoanAmountAfterInsurrance", "$Loan.Amount" },
                   {"ShopCode", "$Loan.SignAddress" },
                   {"CustomerName", "$Personal.Name" },
                   {"CitizenId", "$Personal.IdCard" },
                   {"LoanTenor", "$Loan.Term" },
                   {"HasInsurance", "$Loan.BuyInsurance" },
                   {"CompanyTaxNumber", "$Working.TaxId" },
                };
                BsonDocument document = await _customer
                    .Aggregate()
                    .Match(filter)
                    .Lookup("Product", "Loan.ProductId", "ProductId", "Loan.ProductObj")
                    .Unwind("Loan.ProductObj", unwindOption)
                    .Project(projectMapping)
                    .FirstOrDefaultAsync();

                var result = BsonSerializer.Deserialize<CustomerCheckListRequestModel>(document);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public LoanCaculatorResponse CalculateLoan(LoanCaculatorRequest request, double rate)
        {
            try
            {
                double pmt = Financial.Pmt(rate / 100, request.Nper, request.Pv);
                double dti = pmt / request.Income;

                LoanCaculatorResponse response = new LoanCaculatorResponse
                {
                    PMT = pmt,
                    DTI = dti
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

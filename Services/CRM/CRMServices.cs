using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.CRM
{
    public class CRMServices : IScopedLifetime
    {
        private readonly ILogger<CRMServices> _logger;
        private readonly CustomerServices _customerServices;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly CustomerQueryService _customerQueryService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly DataProcessingService _dataProcessingService;
        private readonly ConfigServices _configServices;
        private readonly IMapper _mapper;
        private readonly LeadCrmService _leadCrmService;
        private readonly ILeadSourceRepository _leadSourceRepository;
        private readonly ICustomerRepository _customerRepository;

        public CRMServices(ILogger<CRMServices> logger,
            CustomerServices customerServices,
            CustomerDomainServices customerDomainServices,
            CustomerQueryService customerQueryService,
            DataCRMProcessingServices dataCRMProcessingServices,
            DataProcessingService dataProcessingService,
            ConfigServices configServices,
            IMapper mapper,
            LeadCrmService leadCrmService,
            ICustomerRepository customerRepository,
            ILeadSourceRepository leadSourceRepository)
        {
            _logger = logger;
            _customerServices = customerServices;
            _customerDomainServices = customerDomainServices;
            _customerQueryService = customerQueryService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _dataProcessingService = dataProcessingService;
            _mapper = mapper;
            _leadCrmService = leadCrmService;
            _configServices = configServices;
            _customerRepository = customerRepository;
            _leadSourceRepository = leadSourceRepository;
        }

        private string CRMLogin()
        {
            string session = string.Empty;
            try
            {
                var client = new RestClient(Common.Constants.Url.CRMLogin);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("_operation", "login");
                request.AddParameter("username", "" + Common.Constants.ConfigRequest.CRM_UserName + "");
                request.AddParameter("password", "" + Common.Constants.ConfigRequest.CRM_Password + "");
                IRestResponse response = client.Execute(request);
                dynamic context = JsonConvert.DeserializeObject<dynamic>(response.Content);
                session = context.result.login.session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return session;
        }

        private CrmCustomerData QueryLead(string session, string queryString)
        {
            try
            {
                var client = new RestClient(Common.Constants.Url.CRMQueryLead);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("_operation", "query");
                request.AddParameter("_session", "" + session + "");
                request.AddParameter("query", "" + queryString + "");
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<CrmCustomerData>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task GetGreenFromCrmAsync(string query, string greenType)
        {
            try
            {
                string session = CRMLogin();
                if (string.IsNullOrEmpty(session))
                {
                    throw new Exception("Token is null or empty");
                }
                CrmCustomerData dataFromCRM = QueryLead(session, query);
                await UpdateCustomerGreenAsync(dataFromCRM, greenType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task UpdateCustomerGreenAsync(CrmCustomerData crmCustomer, string greenType)
        {
            try
            {
                if (crmCustomer?.Result?.Records?.Any() != true)
                {
                    return;
                }

                IEnumerable<string> crmIds = crmCustomer.Result.Records.Select(x => x.Id);
                IEnumerable<Customer> customers = await _customerQueryService.GetByCrmIdsAsync(crmIds);

                var dataCRMProcessingCreations = new List<DataCRMProcessing>();

                foreach (Record record in crmCustomer.Result.Records)
                {
                    Customer customer = customers.FirstOrDefault(x => x.CRMId == record.Id);
                    if (customer != null)
                    {
                        var success = greenType switch
                        {
                            GreenType.GreenE => await updateReturnGECustomer(record, customer),
                            GreenType.GreenF88 => await UpdateReturnGreenFCustomer(record, customer),
                            _ => throw new NotImplementedException(),
                        };
                        if (success)
                        {
                            dataCRMProcessingCreations.Add(new DataCRMProcessing
                            {
                                CustomerId = customer.Id,
                                LeadSource = LeadSourceType.MC
                            });
                        }
                    }
                }

                _dataCRMProcessingServices.InsertMany(dataCRMProcessingCreations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AddingDataToCRMAsync()
        {
            try
            {
                string session = CRMLogin();
                if (!string.IsNullOrEmpty(session))
                {
                    var dataCRMProcessings = _dataCRMProcessingServices.GetDataCRMProcessings(Common.DataCRMProcessingStatus.InProgress);
                    if (!dataCRMProcessings.Any())
                    {
                        return;
                    }

                    IEnumerable<string> customerIds = dataCRMProcessings
                        .Where(x => !string.IsNullOrEmpty(x.CustomerId))
                        .Select(x => x.CustomerId);
                    IEnumerable<Customer> customers = _customerServices.GetByIds(customerIds);

                    IEnumerable<string> leadCrmIds = dataCRMProcessings
                        .Where(x => !string.IsNullOrEmpty(x.LeadCrmId))
                        .Select(x => x.LeadCrmId);
                    IEnumerable<LeadCrm> leadCrms = _leadCrmService.GetByIds(leadCrmIds);

                    IEnumerable<string> leadSourceIds = dataCRMProcessings
                        .Where(x => !string.IsNullOrEmpty(x.LeadSourceId))
                        .Select(x => x.LeadSourceId);
                    IEnumerable<LeadSource> leadSources = await _leadSourceRepository.GetByIds(leadSourceIds);

                    foreach (var dataCRMProcessing in dataCRMProcessings)
                    {
                        await processDataAsync(dataCRMProcessing, leadCrms, customers, leadSources, session);
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task processDataAsync(DataCRMProcessing dataCRMProcessing, IEnumerable<LeadCrm> leadCrms, IEnumerable<Customer> customers,
            IEnumerable<LeadSource> leadSources, string session)
        {
            try
            {

                UpSertCrmResponse crmCustomerResponse = null;

                switch (dataCRMProcessing.LeadSource)
                {
                    case LeadSourceType.MA:
                        {
                            LeadCrm leadCrm = leadCrms.FirstOrDefault(x => x.Id == dataCRMProcessing.LeadCrmId);
                            if (leadCrm == null)
                            {
                                break;
                            }
                            var dataCRM = _mapper.Map<CRMRequestDto>(leadCrm);
                            dataCRM.Cf1178 = "MIRAE ASSET";
                            dataCRM.Cf1206 = "1";
                            if (string.IsNullOrEmpty(dataCRM.Leadsource))
                            {
                                dataCRM.Leadsource = "Telesales 24hPlus -2020";
                            }
                            crmCustomerResponse = PushDataToCRM(dataCRM, session, dataCRMProcessing);
                        }
                        break;
                    case LeadSourceType.FIBO:
                        {
                            LeadCrm leadCrm = leadCrms.FirstOrDefault(x => x.Id == dataCRMProcessing.LeadCrmId);
                            if (leadCrm == null)
                            {
                                break;
                            }
                            var dataCRM = _mapper.Map<CRMRequestDto>(leadCrm);
                            dataCRM.AssignedUserId = "19x3017";
                            crmCustomerResponse = PushDataToCRM(dataCRM, session, dataCRMProcessing);
                            if (crmCustomerResponse?.Result?.Record == null)
                            {
                                break;
                            }
                            leadCrm.LeadCrmId = crmCustomerResponse.Result.Record.Id;
                            leadCrm.PotentialNo = crmCustomerResponse.Result.Record.PotentialNo;
                            _leadCrmService.ReplaceOne(leadCrm);
                        }
                        break;
                    case LeadSourceType.LeadSource:
                        {
                            LeadSource leadSource = leadSources.FirstOrDefault(x => x.Id == dataCRMProcessing.LeadSourceId);
                            if (leadSource == null)
                            {
                                break;
                            }
                            var dataCRM = _mapper.Map<CRMRequestDto>(leadSource);
                            crmCustomerResponse = PushDataToCRM(dataCRM, session, dataCRMProcessing);
                            if (crmCustomerResponse?.Result?.Record == null)
                            {
                                break;
                            }
                            leadSource.CRMId = crmCustomerResponse.Result.Record.Id;
                            await _leadSourceRepository.ReplaceOneAsync(leadSource);
                        }
                        break;
                    default:
                        {
                            Customer customer = customers.FirstOrDefault(x => x.Id == dataCRMProcessing.CustomerId);
                            if (customer == null)
                            {
                                break;
                            }
                            CRMRequestDto dataCRM = _mapper.Map<CRMRequestDto>(customer);
                            crmCustomerResponse = PushDataToCRM(dataCRM, session, dataCRMProcessing);
                            if (crmCustomerResponse?.Result?.Record == null || !string.IsNullOrEmpty(customer.CRMId))
                            {
                                break;
                            }
                            var update = Builders<Customer>.Update
                                .Set(x => x.ModifiedDate, DateTime.Now)
                                .Set(x => x.CRMId, crmCustomerResponse.Result.Record.Id);
                            await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);
                        }
                        break;
                }

                // Update result to data crm processing
                if (crmCustomerResponse?.Success == true)
                {
                    dataCRMProcessing.FinishDate = DateTime.Now;
                    _dataCRMProcessingServices.UpdateByCustomerId(dataCRMProcessing, DataCRMProcessingStatus.Done);
                }
                else
                {
                    dataCRMProcessing.FinishDate = DateTime.Now;
                    dataCRMProcessing.Message = crmCustomerResponse?.Error?.Message ?? "";
                    _dataCRMProcessingServices.UpdateByCustomerId(dataCRMProcessing, DataCRMProcessingStatus.Error);
                }
            }
            catch (Exception ex)
            {
                dataCRMProcessing.Message = ex.ToString();
                _dataCRMProcessingServices.UpdateByCustomerId(dataCRMProcessing, DataCRMProcessingStatus.Error);
            }
        }

        private UpSertCrmResponse PushDataToCRM(CRMBaseModel dataCRM, string session, DataCRMProcessing dataCRMProcessing)
        {
            try
            {
                var client = new RestClient(Common.Constants.Url.CRMURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("_operation", "saveRecord");
                request.AddParameter("values", "" + JsonConvert.SerializeObject(dataCRM) + "");
                request.AddParameter("_session", "" + session + "");
                request.AddParameter("module", "Potentials");
                //request.AddParameter("record", "13x55730");
                IRestResponse response = client.Execute(request);

                return JsonConvert.DeserializeObject<UpSertCrmResponse>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        private async Task<bool> updateReturnGECustomer(Record record, Customer customer)
        {
            try
            {
                string[] listReturnDoc = record.Cf1442.Split(" |##| ");
                List<GroupDocument> returnDocuments = new List<GroupDocument>();
                if (listReturnDoc.Count() > 0)
                {
                    foreach (var item in listReturnDoc)
                    {
                        DocumentMapping.GREEN_E_RETURN_DOCUMENT_MAPPING.TryGetValue(item, out string docString);
                        if (docString != null)
                        {
                            var docObj = JsonConvert.DeserializeObject<GroupDocument>(docString);
                            returnDocuments.Add(docObj);
                        }
                    }
                }
                var customerResult = new Models.Result();
                if (customer.Result != null)
                {
                    customerResult = customer.Result;
                }
                customerResult.Reason = record.Cf1420;

                var update = Builders<Customer>.Update
                    .Set(x => x.ModifiedDate, DateTime.Now)
                    .Set(x => x.Status, record.Cf1430 == "DONE" ? "SUCCESS" : record.Cf1430)
                    .Set(x => x.ReturnDocuments, returnDocuments)
                    .Set(x => x.Result, customerResult);
                await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);
                return true;
            }
            catch (System.Exception ex)
            {
                // @todo
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private async Task<bool> UpdateReturnGreenPCustomer(Record record, Customer customer)
        {
            try
            {
                string[] listReturnDoc = record.Cf1442.Split(" |##| ");
                List<GroupDocument> returnDocuments = new List<GroupDocument>();
                foreach (var item in listReturnDoc)
                {
                    LeadPtfSeedData.CrmReturmDocumentMapping.TryGetValue(item, out string documentName);
                    var documents = LeadPtfSeedData.Documents.FirstOrDefault(x => x.GreenType == customer.GreenType)?.Checklist;
                    var document = documents?.FirstOrDefault(x => string.Equals(x.GroupName, documentName, StringComparison.OrdinalIgnoreCase));
                    if (document != null)
                    {
                        returnDocuments.Add(document);
                    }
                }
                customer.Result ??= new Models.Result();
                customer.Result.Reason = record.Cf1420;
                customer.Result.ReturnStatus = record.Cf1184;
                customer.Status = record.Cf1430 switch
                {
                    "SUCCESS" => CustomerStatus.SUCCESS,
                    "DONE" => CustomerStatus.SUCCESS,
                    "REJECT" => CustomerStatus.REJECT,
                    "CANCEL" => CustomerStatus.CANCEL,
                    "RETURN" => CustomerStatus.RETURN,
                    "PROCESSING" => CustomerStatus.PROCESSING,
                    _ => throw new NotImplementedException(),
                };
                customer.ReturnDocuments = returnDocuments;
                await _customerDomainServices.ReplaceOneAsync(customer, nameof(UpdateReturnGreenPCustomer));
                return true;
            }
            catch (System.Exception ex)
            {
                // @todo
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private async Task<bool> UpdateReturnGreenFCustomer(Record record, Customer customer)
        {
            try
            {
                customer.ContractId = record.Cf1504;
                customer.ContractCode = record.Cf1208;
                customer.Loan ??= new Models.Loan();
                customer.Loan.Amount = record.Cf968;


                customer.Result ??= new Models.Result();
                customer.Result.Reason = record.Cf1420;
                customer.Result.ReturnStatus = record.Cf1184;
                customer.Result.ContractNumber = record.Cf1504;
                if(decimal.TryParse(record.Cf1502, out decimal totalLoanAmt))
                {
                    customer.Result.ApprovedAmount = totalLoanAmt;
                }

                customer.Status = record.Cf1430 switch
                {
                    "SUCCESS" => CustomerStatus.SUCCESS,
                    "DONE" => CustomerStatus.SUCCESS,
                    "REJECT" => CustomerStatus.REJECT,
                    "CANCEL" => CustomerStatus.CANCEL,
                    "RETURN" => CustomerStatus.RETURN,
                    "PROCESSING" => CustomerStatus.PROCESSING,
                    _ => throw new NotImplementedException(),
                };
                await _customerDomainServices.ReplaceOneAsync(customer, nameof(UpdateReturnGreenFCustomer));
                return true;
            }
            catch (System.Exception ex)
            {
                // @todo
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}

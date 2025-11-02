
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckInitContractModels;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.ModelResponses.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.MC;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Services.Storage;
using _24hplusdotnetcore.Settings;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public class MCService : IScopedLifetime
    {
        private readonly ILogger<MCService> _logger;
        private readonly FileUploadServices _fileUploadServices;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly DataMCProcessingServices _dataMCProcessingServices;
        private readonly DataMCPrecheckService _dataMCPrecheckService;
        private readonly CustomerServices _customerServices;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly ProductServices _productServices;
        private readonly IUserRepository _userRepository;
        private readonly IRestMCService _restMCService;
        private readonly IMapper _mapper;
        private readonly IRestLoginService _restLoginService;
        private readonly MCConfig _mCConfig;
        private readonly ConfigServices _configServices;
        private readonly IUserLoginService _userLoginService;
        private readonly ICustomerRepository _customerRepository;
        private readonly CRM.DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITrustingSocialRepository _trustingSocialRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly IMcDomainService _mcDomainService;
        private readonly IStorageService _storageService;

        private readonly IMCYearVerifyHistoryRepository _mcYearVerifyHistoryRepository;


        public MCService(
            ILogger<MCService> logger,
            FileUploadServices fileUploadServices,
            IWebHostEnvironment webHostEnvironment,
            DataMCProcessingServices dataMCProcessingServices,
            CustomerServices customerServices,
            CustomerDomainServices customerDomainServices,
            ProductServices productServices,
            IRestMCService restMCService,
            IMapper mapper,
            IRestLoginService restLoginService,
            DataMCPrecheckService dataMCPrecheckService,
            IOptions<MCConfig> mCConfigOptions,
            ConfigServices configServices,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            ICustomerRepository customerRepository,
            CRM.DataCRMProcessingServices dataCRMProcessingServices,
            INotificationRepository notificationRepository,
            ITrustingSocialRepository trustingSocialRepository,
            IHistoryDomainService historyDomainService,
            IMcDomainService mcDomainService,
            IStorageService storageService,
            IMCYearVerifyHistoryRepository mcYearVerifyHistoryRepository
            )
        {
            _logger = logger;
            _fileUploadServices = fileUploadServices;
            _hostingEnvironment = webHostEnvironment;
            _dataMCProcessingServices = dataMCProcessingServices;
            _restMCService = restMCService;
            _customerServices = customerServices;
            _customerDomainServices = customerDomainServices;
            _productServices = productServices;
            _mapper = mapper;
            _restLoginService = restLoginService;
            _dataMCPrecheckService = dataMCPrecheckService;
            _mCConfig = mCConfigOptions.Value;
            _configServices = configServices;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _notificationRepository = notificationRepository;
            _trustingSocialRepository = trustingSocialRepository;
            _historyDomainService = historyDomainService;
            _mcDomainService = mcDomainService;
            _storageService = storageService;
            _mcYearVerifyHistoryRepository = mcYearVerifyHistoryRepository;
        }

        public async Task<MCCheckCatResponseDto> CheckCatAsync(string companyTaxNumber)
        {
            try
            {
                MCCheckCatResponseDto result = await _restMCService.CheckCatAsync(companyTaxNumber);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<string> GetMCTokenAsync()
        {
            var config = _configServices.FindOneByKey(ConfigKey.MC_AUTHORIZATION);
            return await Task.FromResult(config?.Value);
        }

        public async Task<int> PushDataToMCAsync()
        {
            try
            {
                var lstMCProcessing = _dataMCProcessingServices.GetDataMCProcessings(Common.DataCRMProcessingStatus.InProgress);
                if (lstMCProcessing?.Any() != true)
                {
                    return 0;
                }

                int uploadCount = 0;
                foreach (var item in lstMCProcessing)
                {
                    try
                    {
                        await this.processDataAsync(item);

                        uploadCount++;
                    }
                    catch (System.Exception ex)
                    {
                        item.Message = ex.ToString();
                        item.Status = Common.DataCRMProcessingStatus.Error;
                        _dataMCProcessingServices.UpdateById(item.Id, item);
                    }
                }

                return uploadCount;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public async Task processDataAsync(DataMCProcessing item)
        {
            try
            {
                var objCustomer = _customerServices.GetCustomer(item.CustomerId);
                if (objCustomer.Status == CustomerStatus.CANCEL)
                {
                    item.Status = DataCRMProcessingStatus.Done;
                    item.Message = "Case đã hủy";
                    _dataMCProcessingServices.UpdateById(item.Id, item);
                    return;
                }
                var product = _productServices.GetProductByProductId(objCustomer.Loan.ProductId);
                var listInfo = new List<Info>();
                var files = new List<(string, string)>();
                var documents = objCustomer.MCId != 0 ? objCustomer.ReturnDocuments : objCustomer.Documents;

                foreach (var group in documents)
                {
                    var groupId = group.GroupId;
                    foreach (var doc in group.Documents)
                    {
                        if (doc.UploadedMedias != null)
                        {
                            foreach (var media in doc.UploadedMedias)
                            {
                                string mimeType = "";
                                if (media.Type != null && media.Type.IndexOf("/") > -1)
                                {
                                    mimeType = media.Type.Split("/")[1];
                                }
                                else
                                {
                                    mimeType = media.Type;
                                }
                                var dataMCFileInfo = new Info();
                                dataMCFileInfo.GroupId = group.GroupId.ToString();
                                dataMCFileInfo.DocumentCode = doc.DocumentCode;
                                dataMCFileInfo.FileName = media.Name;
                                dataMCFileInfo.MimeType = mimeType;
                                listInfo.Add(dataMCFileInfo);
                                files.Add((media.Uri, media.Name));
                            }
                        }
                    }
                }

                var (filePath, hash) = await ZipFileAsync(objCustomer.Id, files);
                //var (bytes, hash) = await ZipFileAsync(files);
                if (filePath == null)
                {
                    item.Status = DataCRMProcessingStatus.Error;
                    item.Message = "Không nén được file";
                    _dataMCProcessingServices.UpdateById(item.Id, item);
                    return;
                }

                var loanAmount = objCustomer.Loan.Amount.Replace(",", string.Empty);
                var income = objCustomer.Working.Income.Replace(",", string.Empty);
                var term = objCustomer.Loan.Term.Split(' ');
                var dataMC = new DataMC();
                dataMC.Request = new Models.MC.Request();
                dataMC.Request.Id = objCustomer.MCId != 0 ? objCustomer.MCId : 0;
                dataMC.Request.CitizenId = objCustomer.Personal.IdCard;
                dataMC.Request.CustomerName = objCustomer.Personal.Name;
                dataMC.Request.ProductId = product.ProductIdMC;
                dataMC.Request.TempResidence = objCustomer.IsTheSameResidentAddress == true ? 1 : 0;
                dataMC.Request.SaleCode = objCustomer.ProductLine == ProductLineEnum.DSA ? _mCConfig.SaleCodeDSA : _mCConfig.SaleCode;
                dataMC.Request.CompanyTaxNumber = objCustomer.Working.CompanyTaxCode;
                dataMC.Request.ShopCode = objCustomer.Loan.SignAddress.Split(" - ")[0].Trim();
                dataMC.Request.IssuePlace = objCustomer.Loan.SignAddress.Split(" - ")[1].Trim();
                dataMC.Request.LoanAmount = Int32.Parse(loanAmount);
                dataMC.Request.LoanTenor = Int16.Parse(term[0]);
                dataMC.Request.HasInsurance = objCustomer.Loan.BuyInsurance ? 1 : 0;
                dataMC.Request.HasCourier = objCustomer.ProductLine == ProductLineEnum.DSA ? 1 : 0;
                dataMC.Request.Gender = objCustomer.Personal.Gender == "Nam" ? "M" : "F";
                dataMC.Request.CustomerIncome = Int32.Parse(income);
                dataMC.Request.DateOfBirth = objCustomer.Personal.DateOfBirth;

                dataMC.AppStatus = objCustomer.MCId != 0 ? 2 : 1;
                dataMC.MobileIssueDateCitizen = objCustomer.Personal.IdCardDate;
                dataMC.MobileProductType = "CashLoan";
                dataMC.Md5 = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                dataMC.Info = listInfo;

                string token = await GetMCTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    return;
                }

                var client = new RestClient(_mCConfig.Host + "/mcMobileService/service/v1.0/mobile-4sales/upload-document");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Bearer " + token + "");
                request.AddHeader("x-security", _mCConfig.SecurityKey);
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddFile("file", "" + filePath + "");
                //request.AddFile("file", bytes, $"{DateTime.Now:yyyyMMddHHmmssffff}.zip");
                request.AddParameter("object", JsonConvert.SerializeObject(dataMC));
                IRestResponse response = client.Execute(request);
                dynamic content = JsonConvert.DeserializeObject<dynamic>(response.Content);
                if (content != null && content.id != null)
                {
                    int mcId = Int32.Parse(content.id.Value.ToString());
                    objCustomer.MCId = mcId;
                    objCustomer.Status = CustomerStatus.PROCESSING;
                    objCustomer.Result.Reason = "";
                    objCustomer.Result.FinishedRound1 = true;
                    await _customerDomainServices.ReplaceOneAsync(objCustomer, nameof(processDataAsync));
                    await _dataMCPrecheckService.UpdateSubmitedDateAsync(objCustomer.Id);
                }
                else
                {
                    CustomerUpdateStatusDto error = new CustomerUpdateStatusDto();
                    error.CustomerId = objCustomer.Id;
                    error.LeadSource = LeadSourceType.MC.ToString();
                    error.Reason = JsonConvert.SerializeObject(content?.returnMes);
                    error.Status = CustomerStatus.RETURN;
                    await _customerDomainServices.UpdateStatusAsync(error);
                }
                item.Status = Common.DataCRMProcessingStatus.Done;
                item.PayLoad = JsonConvert.SerializeObject(dataMC);
                item.Response = response.Content;
                _dataMCProcessingServices.UpdateById(item.Id, item);

                File.Delete(filePath);
            }
            catch (System.Exception ex)
            {
                item.Message = ex.ToString();
                item.Status = Common.DataCRMProcessingStatus.Error;
                _dataMCProcessingServices.UpdateById(item.Id, item);
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<(string, byte[])> ZipFileAsync(string customerId, IEnumerable<(string, string)> listFile)
        {
            try
            {
                string serverPath = Path.Combine(_hostingEnvironment.ContentRootPath, "FileUpload/" + customerId);
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                Directory.CreateDirectory(Path.Combine(serverPath, timeStamp));
                string d = Path.Combine(serverPath, timeStamp);
                foreach (var (uri, names) in listFile)
                {
                    var obj = await _storageService.GetObjectAsync(uri);
                    using var fs = new FileStream(Path.Combine(d, names), FileMode.Create, FileAccess.Write);
                    fs.Write(obj.Bytes, 0, obj.Bytes.Length);
                }

                ZipFile.CreateFromDirectory(d, Path.Combine(serverPath, timeStamp + ".zip"), CompressionLevel.Optimal, false);
                string fileZip = Path.Combine(serverPath, timeStamp + ".zip");
                Directory.Delete(Path.Combine(serverPath, timeStamp), true);
                var md5 = MD5.Create();
                using var stream = File.OpenRead(fileZip);
                var hash = md5.ComputeHash(stream);

                return (fileZip, hash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (null, null);
            }
        }

        public async Task<(byte[], byte[])> ZipFileAsync(IEnumerable<(string uri, string names)> files)
        {
            try
            {
                var fileToZips = new List<(byte[], string)>();
                foreach (var (uri, names) in files)
                {
                    var obj = await _storageService.GetObjectAsync(uri);
                    fileToZips.Add((obj.Bytes, names));
                }
                var bytes = ZipEntensions.CompressDirectory(fileToZips);

                using MD5 md5 = MD5.Create();
                var hash = md5.ComputeHash(bytes);

                return (bytes, hash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (null, null);
            }
        }

        public async Task<IEnumerable<MCProduct>> GetProductAsync()
        {
            try
            {
                IEnumerable<MCProduct> mCProducts = await _restMCService.GetProductAsync();
                return mCProducts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<CustomerCheckListResponseModel> CheckListAsync(string customerId)
        {
            try
            {
                CustomerCheckListResponseModel result = await _mcDomainService.CheckListAsync(customerId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MCSuccessResponseDto> CancelCaseAsync(CancelCaseRequestDto cancelCaseRequestDto)
        {
            try
            {
                Customer customer = _customerServices.GetCustomer(cancelCaseRequestDto.CustomerId);
                if (customer == null)
                {
                    throw new ArgumentException(Message.CUSTOMER_NOT_FOUND);
                }

                MCCancelCaseRequestDto mCCancelCaseRequestDto = new MCCancelCaseRequestDto
                {
                    Id = customer.MCId,
                    Comment = cancelCaseRequestDto.Comment,
                    Reason = cancelCaseRequestDto.Reason
                };

                MCSuccessResponseDto mCSuccessResponseDto = await _restMCService.CancelCaseAsync(mCCancelCaseRequestDto);
                customer.Status = CustomerStatus.CANCEL;
                await _customerDomainServices.ReplaceOneAsync(customer, nameof(CancelCaseAsync));
                return mCSuccessResponseDto;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<MCResponseDto>();
                throw new ArgumentException(error.ReturnMes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MCCaseNoteListDto> GetCaseNoteAsync(string customerId)
        {
            try
            {
                Customer customer = _customerServices.GetCustomer(customerId);
                if (customer == null)
                {
                    throw new ArgumentException(Message.CUSTOMER_NOT_FOUND);
                }

                MCCaseNoteListDto mCCaseNoteListDto = await _restMCService.GetCaseNoteAsync(customer.MCAppnumber);
                return mCCaseNoteListDto;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<MCResponseDto>();
                throw new ArgumentException(error.ReturnMes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MCSuccessResponseDto> SendCaseNoteAsync(SendCaseNoteRequestDto sendCaseNoteRequestDto)
        {
            try
            {
                Customer customer = _customerServices.GetCustomer(sendCaseNoteRequestDto.CustomerId);
                if (customer == null)
                {
                    throw new ArgumentException(Message.CUSTOMER_NOT_FOUND);
                }

                var mCSendCaseNoteRequestDto = new MCSendCaseNoteRequestDto
                {
                    AppNumber = customer.MCAppnumber,
                    NoteContent = sendCaseNoteRequestDto.NoteContent
                };

                MCSuccessResponseDto mCSuccessResponseDto = await _restMCService.SendCaseNoteAsync(mCSendCaseNoteRequestDto);
                return mCSuccessResponseDto;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<MCResponseDto>();
                throw new ArgumentException(error.ReturnMes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerCheckListResponseModel> GetReturnCheckListAsync(string customerId)
        {
            try
            {
                Customer customer = _customerServices.GetCustomer(customerId);
                if (customer == null)
                {
                    throw new ArgumentException(Message.CUSTOMER_NOT_FOUND);
                }

                CustomerCheckListResponseModel customerCheckListResponseModel = await _restMCService.GetReturnCheckListAsync(customer.MCAppId);
                return customerCheckListResponseModel;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<MCResponseDto>();
                throw new ArgumentException(error.ReturnMes);
            }
        }
        public async Task<IEnumerable<GetCaseMCResponseDto>> GetCasesAsync(GetCaseRequestDto getCaseRequestDto)
        {
            try
            {
                GetCaseMCRequestDto request = _mapper.Map<GetCaseMCRequestDto>(getCaseRequestDto);

                IEnumerable<GetCaseMCResponseDto> cases = await _restMCService.GetCasesAsync(request);

                return cases;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CheckInitContractResponse> CheckInitContractAsync(CheckInitContractRequest checkInitContractRequest)
        {
            Customer customer = _customerServices.GetCustomer(checkInitContractRequest.CustomerId);
            // todo
            if (customer.GreenType != GreenType.GreenC)
            {
                return null;
            }

            Product product = _productServices.GetProductByProductId(customer?.Loan?.ProductId);
            DataMCPrecheckModel history = await _dataMCPrecheckService.GetByCustomerIdAsync(checkInitContractRequest.CustomerId, customer.Personal.IdCard);

            var checkInitContractRestRequest = _mapper.Map<CheckInitContractRestRequest>(customer);
            _mapper.Map(product, checkInitContractRestRequest);
            try
            {
                var result = await _restMCService.CheckInitContractAsync(checkInitContractRestRequest);
                var response = MappingResponseInitContractModel(result);
                PayloadModel payload = new PayloadModel()
                {
                    Payload = JsonConvert.SerializeObject(checkInitContractRestRequest),
                    Response = JsonConvert.SerializeObject(response)
                };
                if (history == null)
                {
                    var currentUser = await _userRepository.FindOneAsync(x => x.UserName == customer.UserName);
                    history = new DataMCPrecheckModel();
                    history.CustomerId = customer.Id;
                    history.CustomerName = customer.Personal.Name;
                    history.IdCard = customer.Personal.IdCard;
                    history.UserName = customer.UserName;
                    history.SaleName = currentUser?.FullName;
                    history.TeamLeadInfo = new TeamLead()
                    {
                        UserName = currentUser?.TeamLeadInfo?.UserName,
                        FullName = currentUser?.TeamLeadInfo?.FullName,
                    };
                    history.Payloads = new List<PayloadModel>() {
                        payload
                    };
                    _dataMCPrecheckService.CreateOne(history);
                }
                else
                {
                    await _dataMCPrecheckService.UpdatePayloadAsync(history.Id, payload);
                }

                return response;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<MCResponseDto>();

                PayloadModel payload = new PayloadModel()
                {
                    Payload = JsonConvert.SerializeObject(checkInitContractRestRequest),
                    Response = JsonConvert.SerializeObject(error)
                };
                if (history == null)
                {
                    var currentUser = await _userRepository.FindOneAsync(x => x.UserName == customer.UserName);
                    history = new DataMCPrecheckModel();
                    history.CustomerId = customer.Id;
                    history.CustomerName = customer.Personal.Name;
                    history.IdCard = customer.Personal.IdCard;
                    history.UserName = customer.UserName;
                    history.SaleName = currentUser?.FullName;
                    history.TeamLeadInfo = new TeamLead()
                    {
                        UserName = currentUser?.TeamLeadInfo?.UserName,
                        FullName = currentUser?.TeamLeadInfo?.FullName,
                    };
                    history.Payloads = new List<PayloadModel>() {
                        payload
                    };
                    _dataMCPrecheckService.CreateOne(history);
                }
                else
                {
                    await _dataMCPrecheckService.UpdatePayloadAsync(history.Id, payload);
                }
                throw new ArgumentException(error.ReturnMes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        private CheckInitContractResponse MappingResponseInitContractModel(CheckInitContractRestResponse checkInitContractRestResponse)
        {
            if (checkInitContractRestResponse.ReturnCode != StatusCodes.Status200OK.ToString())
            {
                throw new Exception("Check init contract failed");
            }
            var results = JsonConvert.DeserializeObject<IEnumerable<CheckInitContractResultResponse>>(checkInitContractRestResponse.ReturnMes);
            return new CheckInitContractResponse { Results = results };
        }

        public async Task RefreshTokenAsync()
        {
            try
            {
                var loginRequest = new LoginRequestModel
                {
                    Username = _mCConfig.Username,
                    Password = _mCConfig.Password,
                    NotificationId = _mCConfig.NotificationId,
                    Imei = _mCConfig.Imei,
                    OsType = _mCConfig.OsType
                };
                LoginResponseModel result = await _restLoginService.GetTokenAsync(loginRequest);

                await _configServices.UpsertAsync(ConfigKey.MC_AUTHORIZATION, result.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<SendOtpResponse>> SendOtp(SendOtpRequest request)
        {
            var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

            try
            {
                var sendResult = new BaseResponse<SendOtpResponse>();

                SendOtpRequestValidation validator = new SendOtpRequestValidation();
                ValidationResult result = validator.Validate(request);

                sendResult.MappingFluentValidation(result);

                if (sendResult.ValidationErrors.Any())
                {
                    return sendResult; ;
                }

                string token = await GetMCTokenAsync();

                var sentRequest = new SendOtpRestRequest
                {
                    RequestedMsisdn = request.RequestedMsisdn,
                    TypeScore = request.TypeScore,
                };
                var response = await _restMCService.SendOtp(sentRequest, string.Format("{0} {1}", "Bearer", token));

                sendResult.Data = JsonConvert.DeserializeObject<SendOtpResponse>(response.ToString());


                var sendOtp = new MCTrustingSocial
                {
                    PayLoad = JsonConvert.SerializeObject(request),
                    Response = response.ToString(),
                    CreatedDate = DateTime.Now,
                    Creator = currentUser?.Id
                };

                await _trustingSocialRepository.Create(sendOtp);

                return sendResult;
            }
            catch (ApiException ex)
            {

                var sendOtp = new MCTrustingSocial
                {
                    PayLoad = JsonConvert.SerializeObject(request),
                    Response = ex.Content,
                    Creator = currentUser?.Id
                };

                await _trustingSocialRepository.Create(sendOtp);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<Scoring3PResponse>> SendScoring3P(Scoring3PRequest request)
        {

            var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

            try
            {
                var sendResult = new BaseResponse<Scoring3PResponse>();

                Scoring3PRequestValidation validator = new Scoring3PRequestValidation();
                ValidationResult result = validator.Validate(request);

                sendResult.MappingFluentValidation(result);

                if (sendResult.ValidationErrors.Any())
                {
                    return sendResult; ;
                }

                string token = await GetMCTokenAsync();

                var sentRequest = new Scoring3PRestRequest
                {
                    NationalId = request.NationalId,
                    PrimaryPhone = request.PrimaryPhone,
                    TypeScore = request.TypeScore,
                    UserName = _mCConfig.Username,
                    VerificationCode = request.VerificationCode
                };

                var response = await _restMCService.SendScoring3P(sentRequest, string.Format("{0} {1}", "Bearer", token));

                var responseObj = JsonConvert.DeserializeObject<Scoring3PResponse>(response.ToString());
                responseObj.ReturnMes = MCTrustingSocialMapping.MESSAGE.TryGetValue(responseObj.ReturnMes, out string message) ? message : responseObj.ReturnMes;
                sendResult.Data = responseObj;

                var scoring = new MCTrustingSocial
                {
                    PayLoad = JsonConvert.SerializeObject(request),
                    PayLoadObj = request,
                    Response = response.ToString(),
                    ResponseObj = responseObj,
                    Creator = currentUser?.Id
                };

                await _trustingSocialRepository.Create(scoring);

                return sendResult;
            }
            catch (ApiException ex)
            {
                var scoring = new MCTrustingSocial
                {
                    PayLoad = JsonConvert.SerializeObject(request),
                    Response = ex.Content,
                    Creator = currentUser?.Id
                };

                await _trustingSocialRepository.Create(scoring);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<PagingResponse<TrustingSocialResponse>> GetTTrustingSocialList(PagingRequest pagingRequest)
        {
            try
            {
                var filterByCreatorIds = new List<string>();

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllTrustingSocial))
                {
                    filterByCreatorIds.Add(_userLoginService.GetUserId());
                    var teamMembers = _userRepository.FilterBy(x => x.TeamLeadInfo.Id == _userLoginService.GetUserId());
                    filterByCreatorIds.AddRange(teamMembers.Select(x => x.Id));
                }

                var result = await _trustingSocialRepository.GetList(pagingRequest, filterByCreatorIds);
                var rowCount = await _trustingSocialRepository.Count(pagingRequest, filterByCreatorIds);

                return new PagingResponse<TrustingSocialResponse>
                {
                    TotalRecord = rowCount,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerBirthYearVerifyResponse> VerifyCustomerBirthYear(string customerId)
        {

            var result = new CustomerBirthYearVerifyResponse()
            {
                CustomerId = customerId,
            };

            try
            {
                var mcYearVerifyHistory = new MCYearVerifyHistory
                {
                    CreatedDate = DateTime.Now,
                    Creator = _userLoginService.GetUserId(),
                    CustomerId = customerId,
                    Action = HistoryActionType.Verify,
                    Type = "YearVerification"
                };

                var customerDetail = await Task.Run(() => _customerServices.GetCustomer(customerId));

                if (customerDetail == null)
                {
                    result.ReturnMes = $"Customer {customerId} is not found.";

                    mcYearVerifyHistory.Response = JsonConvert.SerializeObject(result);
                    await _mcYearVerifyHistoryRepository.Create(mcYearVerifyHistory);

                    return result;
                }

                string formatedDate = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";

                var ruleCheckRequest = new MCAgeCheckingDto
                {
                    BirthDate = customerDetail?.Personal?.DateOfBirth,
                    CitizenID = customerDetail?.Personal?.IdCard,
                    LoanTenor = customerDetail?.Loan?.TermId,
                    ProductCode = customerDetail?.Loan?.ProductId,
                    SalaryPaymentType = customerDetail?.Working?.IncomeMethodId,
                    RuleCode = "AGE_VALIDATION",
                    ProductGroup = "CashLoan",
                    CreatedDate = formatedDate,
                    CheckAgeDate = formatedDate,
                    CustCompanyCat = string.Empty
                };


                var mcToken = await GetMCTokenAsync();
                var xSecurity = $"{_mCConfig.XSecurityAndroid},{_mCConfig.XSecurityIOS}";

                var checkAgeRestResult = await _restMCService.CheckRule(ruleCheckRequest, mcToken, _mCConfig.SecurityKey, _mCConfig.XVersion);

                var returnResult = checkAgeRestResult.ToObject<CustomerBirthYearVerifyResponse>();

                result.IsValid = true;
                result.ReturnMes = $"Khách hàng đủ tuổi vay";
                result.ReturnCode = (int)HttpStatusCode.OK;

                mcYearVerifyHistory.Response = JsonConvert.SerializeObject(result);
                await _mcYearVerifyHistoryRepository.Create(mcYearVerifyHistory);

                return result;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);

                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.IsValid = true;
                    result.ReturnCode = (int)HttpStatusCode.OK;
                    result.ReturnMes = $"Khách hàng đủ tuổi vay";

                    return result;
                }

                if (ex.StatusCode == HttpStatusCode.OK)
                {
                    result.IsValid = false;
                    result.ReturnCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnMes = $"KH không đủ/quá tuổi vay";

                    return result;
                }

                if (ex.StatusCode == HttpStatusCode.InternalServerError)
                {
                    result.IsValid = false;
                    result.ReturnCode = (int)HttpStatusCode.InternalServerError;
                    result.ReturnMes = $"Sai input";

                    return result;
                }

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                result.ReturnCode = (int)HttpStatusCode.InternalServerError;
                result.ReturnMes = "Internal Server Error";
                result.IsValid = false;

                return result;
            }
        }

        public async Task<MCCustomerPaymentVerifyResponse> CheckCustomerPaymentVerification(MCCustomerPaymentVerifyDto request)
        {
            try
            {
                var result = new MCCustomerPaymentVerifyResponse()
                {
                    IsValid = true,
                    CustomerId = request.CustomerId,
                    BirthYearVerifiCationResponse = new CustomerBirthYearVerifyResponse(),
                    ContractVerificationResponse = new CheckInitContractResponse(),
                    IDVerificationResponse = new MCResponseDto()
                };

                var checkInitContractRequest = new CheckInitContractRequest
                {
                    CustomerId = request.CustomerId
                };

                if (request.GreenType.ToUpper() != "C")
                {
                    result.IsValid = false;
                    result.Message = "Invalid green type";

                    return result;
                }

                result.BirthYearVerifiCationResponse = await VerifyCustomerBirthYear(request.CustomerId);

                if (!result.BirthYearVerifiCationResponse.IsValid)
                {
                    result.IsValid = false;
                    return result;
                }

                try
                {
                    result.IDVerificationResponse = await _restMCService.CheckCitizendAsync(request.CitizenId);
                }
                catch (ApiException ex)
                {
                    _logger.LogError(ex, ex.Content);

                    result.IsValid = false;
                    result.IDVerificationResponse.ReturnCode = (int)HttpStatusCode.BadRequest;
                    result.IDVerificationResponse = await ex.GetContentAsAsync<MCResponseDto>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    result.IsValid = false;
                    result.IDVerificationResponse.ReturnCode = (int)HttpStatusCode.InternalServerError;
                    result.IDVerificationResponse.ReturnMes = "Internal Server Error";
                }

                if (!result.IsValid)
                {
                    return result;
                }

                try
                {
                    result.ContractVerificationResponse = await CheckInitContractAsync(checkInitContractRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    result.IsValid = false;
                    result.ContractVerificationResponse.ReturnCode = (int)HttpStatusCode.InternalServerError;
                    result.ContractVerificationResponse.ReturnMes = "Internal Server Error";
                }

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

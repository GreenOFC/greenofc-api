using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.CIMB;
using _24hplusdotnetcore.ModelResponses.CIMB;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;

namespace _24hplusdotnetcore.Services.CIMB
{
    public class CIMBDataProcessingService : IScopedLifetime
    {
        private readonly ILogger<CIMBDataProcessingService> _logger;
        private readonly IMongoRepository<DataCimbProcessing> _dataCimbProcessingCollection;
        private readonly IMongoRepository<Customer> _customerCollection;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICIMBRestService _cimbRestService;
        private readonly IMapper _mapper;

        public CIMBDataProcessingService(
            ILogger<CIMBDataProcessingService> logger,
            IMongoRepository<DataCimbProcessing> dataCimbProcessingCollection,
            IMongoRepository<Customer> customerCollection,
            IWebHostEnvironment hostingEnvironment,
            ICIMBRestService cimbRestService,
            IMapper mapper
            )
        {
            _logger = logger;
            _dataCimbProcessingCollection = dataCimbProcessingCollection;
            _customerCollection = customerCollection;
            _hostingEnvironment = hostingEnvironment;
            _cimbRestService = cimbRestService;
            _mapper = mapper;
        }

        public async Task<bool> UpdateStatus(string id, string status, string message = null, string payload = null, string response = null)
        {
            try
            {
                var update = Builders<DataCimbProcessing>.Update
                    .Set(x => x.Status, status)
                    .Set(x => x.Message, message)
                    .Set(x => x.PayLoad, payload)
                    .Set(x => x.Response, response);

                await _dataCimbProcessingCollection.UpdateOneAsync(x => x.Id == id, update);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<DataCimbProcessing> GetCIMBDataProcessing(string status)
        {
            try
            {
                var cimbDataProcessing = _dataCimbProcessingCollection.FilterBy(d => d.Status == status && !string.IsNullOrEmpty(d.CustomerId));
                return cimbDataProcessing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CIMBCustomerUploadInformation> GetCusomterUploadProfile(string customerId)
        {
            try
            {
                var customerDetail = await _customerCollection.FindOneAsync(x => x.Id == customerId);

                List<string> docCodes = new List<string>()
                {
                   CIMBUploadDocumentType.CERT_FRONT_PIC,
                   CIMBUploadDocumentType.CERT_BACK_PIC,
                   CIMBUploadDocumentType.SELFIE
                };

                var customerUploadInformation = new CIMBCustomerUploadInformation();
                List<DocumentUpload> uploadedDocuments = new List<DocumentUpload>();
                List<UploadedMedia> uploadedMedias = new List<UploadedMedia>();

                if (customerDetail != null && customerDetail.Documents != null)
                {
                    foreach (var group in customerDetail.Documents)
                    {
                        var documents = group.Documents.Where(x => docCodes.Contains(x.DocumentCode));

                        if (documents != null && documents.Any())
                        {
                            uploadedDocuments.AddRange(documents);
                        }
                    }

                    var certFrontPic = uploadedDocuments.FirstOrDefault(x => x.DocumentCode == CIMBUploadDocumentType.CERT_FRONT_PIC);
                    var certBackPic = uploadedDocuments.FirstOrDefault(x => x.DocumentCode == CIMBUploadDocumentType.CERT_BACK_PIC);
                    var certSelfiePic = uploadedDocuments.FirstOrDefault(x => x.DocumentCode == CIMBUploadDocumentType.SELFIE);

                    if (certFrontPic != null)
                    {
                        var uploadMedia = certFrontPic.UploadedMedias.FirstOrDefault();
                        customerUploadInformation.CertFrontPicUri = uploadMedia.Uri;
                        customerUploadInformation.CertFrontPicName = uploadMedia.Name;
                        customerUploadInformation.CertFrontPicBytes = await GetImageBytes(uploadMedia.Uri);
                    }

                    if (certBackPic != null)
                    {
                        var certBackPicMedia = certBackPic.UploadedMedias.FirstOrDefault();
                        customerUploadInformation.CertBackPicUri = certBackPicMedia.Uri;
                        customerUploadInformation.CertBackPicName = certBackPicMedia.Name;
                        customerUploadInformation.CertBackPicBytes = await GetImageBytes(certBackPicMedia.Uri);
                    }

                    if (certSelfiePic != null)
                    {
                        var certSelfie = certSelfiePic.UploadedMedias.FirstOrDefault();
                        customerUploadInformation.SelfieUri = certSelfie.Uri;
                        customerUploadInformation.SelfieName = certSelfie.Name;
                        customerUploadInformation.SelfieBytes = await GetImageBytes(certSelfie.Uri);
                    }
                }

                return customerUploadInformation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<byte[]> GetImageBytes(string uri)
        {
            try
            {
                byte[] bytes = null;

                if (!uri.IsEmpty())
                {
                    using (HttpClient c = new HttpClient())
                    {
                        using (Stream stream = await c.GetStreamAsync(uri))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                bytes = ms.ToArray();
                            }
                        }
                    }
                }

                return bytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CIMBOnBoardingChecResponse> CanOnBoarding(CIMBOnBoardingCheckDto onBoardingCheckDto)
        {
            try
            {
                var filter = Builders<Customer>.Filter.Eq(c => c.Personal.Email, onBoardingCheckDto.Email);
                filter &= Builders<Customer>.Filter.Ne(c => c.IsDeleted, true);

                var customerDetail = await _customerCollection.FindOneAsync(x => x.Personal.Email == onBoardingCheckDto.Email && !x.IsDeleted);

                var response = new CIMBOnBoardingChecResponse();

                if (onBoardingCheckDto == null)
                {
                    response.RejectReason = "Invalid customer information";
                    return response;
                }

                if (!onBoardingCheckDto.IsEmailVerified.Value)
                {
                    response.RejectReason = "Email is not verified";
                    return response;
                }

                if (!onBoardingCheckDto.IsPhoneVerified.Value)
                {
                    response.RejectReason = "Phone number is not verified";
                    return response;
                }

                if (onBoardingCheckDto.OnboardProductType != OnBoardProductType.LOAN.ToString())
                {
                    response.RejectReason = "OnBoard product type must be loan";
                    return response;
                }

                if (!onBoardingCheckDto.PartnerAccountId.IsEmpty())
                {
                    response.RejectReason = "Partner account is empty";
                    return response;
                }

                response.CanOnboard = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CIMBSubmitInformation> GetCustomerSubmit(string customerId)
        {
            var customerDetail = await _customerCollection.FindOneAsync(x => !x.IsDeleted && x.Id == customerId);

            var submitInfo = new CIMBSubmitInformation
            {
                ReferenceLoanId = Guid.NewGuid().ToString(),
                PartnerAccountId = customerDetail?.Personal?.IdCard,
                Customer = new CIMBCustomerDto
                {
                    Name = customerDetail?.Personal.Name,
                    NationalId = customerDetail?.Personal?.IdCard,
                    PreviousIdNumber = customerDetail?.Personal?.OldIdCard,
                    DateOfBirth = customerDetail?.Personal?.DateOfBirth.FormatDate("yyyy-MM-dd"),
                    Gender = customerDetail?.Personal?.Gender == "Nam" ? "MALE" : "FEMALE",
                    MobileNumber = customerDetail?.Personal?.Phone,
                    Email = customerDetail?.Personal?.Email,
                    MaritalStatus = customerDetail?.Personal?.MaritalStatusId,
                    Education = customerDetail?.Personal?.EducationLevelId,

                    Contacts = customerDetail?.Referees.Select(x => new CIMBCustomerContactDto
                    {
                        Name = x.Name,
                        PhoneNumber = x.Phone,
                        Type = x.RelationshipId
                    }),

                    CurrentAddress = new CIMBCustomerAddressDto
                    {
                        City = customerDetail?.ResidentAddress?.ProvinceId,
                        District = customerDetail?.ResidentAddress?.DistrictId,
                        Street = customerDetail?.ResidentAddress?.Street,
                        Ward = customerDetail?.ResidentAddress?.WardId
                    },
                },

                Income = new CIMBIncomeDto
                {
                    DeclaredMonthlyAmount = customerDetail?.Working?.Income.ToNumber()
                },

                Loan = new CIMBLoanDto
                {
                    // @todo
                    LoanPurpose = "3008",
                    RequestAmount = customerDetail?.Loan.Amount.ToNumber(),
                    RequestTenor = customerDetail?.Loan.TermId.ToNumber(),
                },

                Employment = new CIMBEmployment
                {
                    CompanyName = customerDetail?.Working?.CompanyName,
                    EmploymentStatus = customerDetail?.Working?.EmploymentStatusId
                }
            };

            return submitInfo;
        }

        public async Task<IEnumerable<DataCimbProcessing>> GetPayloadAsync(string customerId)
        {
            try
            {
                
                var data = _dataCimbProcessingCollection.FilterBy(x => x.CustomerId == customerId);
                var result = _mapper.Map<IEnumerable<DataCimbProcessing>>(data);
                if (result.Count() == 0)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "hồ sơ!"));
                }
                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    
        public async Task CreateAsync(string id)
        {
            try
            {
                var customer = await _customerCollection.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }
                await _dataCimbProcessingCollection.InsertOneAsync(new DataCimbProcessing(customer.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

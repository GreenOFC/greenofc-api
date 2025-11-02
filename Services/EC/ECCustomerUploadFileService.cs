using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Settings;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public class ECCustomerUploadFileService : IScopedLifetime
    {
        private readonly IMongoRepository<Customer> _customerCollection;
        private readonly ILogger<ECCustomerUploadFileService> _logger;
        private readonly ECUploadFileService _ecUploadFileService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ECConfig _ecConfig;
        private readonly ECDataProcessingService _ecDataProcessingService;

        private readonly IECRestService _eCRestService;
        private readonly ECAuthorizationService _eCAuthorizationService;

        private readonly ECOfferService _ecOfferService;

        public ECCustomerUploadFileService(
            ILogger<ECCustomerUploadFileService> logger,
            IMongoRepository<Customer> customerCollection,
            ECUploadFileService ecUploadFileService,
            IWebHostEnvironment hostingEnvironment,
            IOptions<ECConfig> ecConfig,
            ECDataProcessingService ecDataProcessingService,
            IECRestService eCRestService,
            ECAuthorizationService eCAuthorizationService,
            ECOfferService ecOfferService
            )
        {
            _logger = logger;
            _customerCollection = customerCollection;
            _ecUploadFileService = ecUploadFileService;
            _hostingEnvironment = hostingEnvironment;
            _ecConfig = ecConfig.Value;
            _ecDataProcessingService = ecDataProcessingService;
            _eCRestService = eCRestService;
            _eCAuthorizationService = eCAuthorizationService;
            _ecOfferService = ecOfferService;
        }

        public async Task UploadCusomterUploadProfile(string customerId)
        {
            try
            {
                var customerDetail = await _customerCollection.FindOneAsync(x => x.Id == customerId);

                if (null != customerDetail)
                {
                    List<DocumentUpload> uploadedDocuments = new List<DocumentUpload>();
                    List<UploadedMedia> uploadedMedias = new List<UploadedMedia>();

                    if (customerDetail != null && customerDetail.Documents != null)
                    {
                        foreach (var group in customerDetail.Documents)
                        {
                            foreach (var doc in group.Documents)
                            {
                                if (doc.UploadedMedias != null && doc.UploadedMedias.Count() > 0)
                                {
                                    uploadedDocuments.Add(doc);
                                }
                            }
                        }

                        var listDocs = new List<ECDocCollectingListDto>();
                        string imgIdCard = string.Empty;
                        string imgSelfie = string.Empty;

                        List<ECUploadFileDto> uploadFiles = new List<ECUploadFileDto>();

                        var timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                        string requestId = _ecConfig.PartnerCode + timeStamp;

                        var fullLoanRequest = await _ecDataProcessingService.GetFullLoanInfo(customerId);

                        foreach (var uploadedDocument in uploadedDocuments)
                        {
                            var uploadMedias = uploadedDocument.UploadedMedias;

                            string pdfFileName = $"{{0}}_{customerDetail?.Personal?.IdCard}_{customerDetail?.Personal?.Phone}_{requestId}.pdf";

                            if (uploadedDocument.DocumentCode == "SPID")
                            {
                                var bytes = ConverToPdf(uploadMedias);
                                var fileName = string.Format(pdfFileName, "PID");
                                imgIdCard = fileName;
                                var ecuploadFile = new ECUploadFileDto()
                                {
                                    RemoteFolder = "PID",
                                    Bytes = bytes,
                                    FileName = fileName
                                };
                                uploadFiles.Add(ecuploadFile);
                            }
                            else if (uploadedDocument.DocumentCode == "SPIC")
                            {
                                var bytes = ConverToPdf(uploadMedias);
                                var fileName = string.Format(pdfFileName, "PIC");
                                imgSelfie = fileName;

                                var ecuploadFile = new ECUploadFileDto()
                                {
                                    RemoteFolder = "PIC",
                                    Bytes = bytes,
                                    FileName = fileName
                                };
                                uploadFiles.Add(ecuploadFile);
                            }

                            if (uploadedDocument.MapBpmVar != "TEMP")
                            {
                                var fileName = string.Format(pdfFileName, uploadedDocument.DocumentCode);

                                listDocs.Add(new ECDocCollectingListDto()
                                {
                                    FileName = fileName,
                                    FileType = uploadedDocument.DocumentCode
                                });

                                var bytes = ConverToPdf(uploadMedias);
                                var ecuploadFile = new ECUploadFileDto()
                                {
                                    RemoteFolder = string.Empty,
                                    Bytes = bytes,
                                    FileName = fileName
                                };
                                uploadFiles.Add(ecuploadFile);
                            }
                        }

                        // upload file using api
                        var pidDetail = uploadFiles.FirstOrDefault(x => x.RemoteFolder == "PID");
                        var picDetail = uploadFiles.FirstOrDefault(x => x.RemoteFolder == "PIC");

                        // if (pidDetail != null && picDetail != null)
                        // {
                            // var pidBytes = new ByteArrayPart(pidDetail.Bytes, pidDetail.FileName);
                            // var picBytes = new ByteArrayPart(picDetail.Bytes, picDetail.FileName);

                            // var selectOfferDetail = await _ecOfferService.GetAsync(customerDetail.ECRequest);
                            // var proposalId = selectOfferDetail?.ProposalId;


                            // var token = await _eCAuthorizationService.GetToken();

                            // var pidParams = new ECGetPresignUrl
                            // {
                            //     DocType = "PID",
                            //     FileName = pidDetail.FileName,
                            //     ProposalId = proposalId,
                            //     RequestId = requestId,
                            //     PartnerCode = _ecConfig.PartnerCode
                            // };

                            // get pid presign url

                            // _logger.LogInformation($"start getting pid presigned url: fileName: {pidDetail.FileName} requestId: {requestId} partnerCode: {_ecConfig.PartnerCode} proposalId: {proposalId}");
                            // var getPidResult = await _eCRestService.GetPresignUrl(_ecConfig.PartnerCode, pidParams, token);
                            // var pidUploadResponse = getPidResult.ToObject<ECPresignUrlResponse>();
                            // _logger.LogInformation("get pid resigned-url successfully");

                            // // push pid to s3
                            // _logger.LogInformation($"start uploading pic file to s3: fileName: {picDetail.FileName} url: {pidUploadResponse?.Data?.Url}");
                            // var s3client = RestService.For<IECS3RestService>(pidUploadResponse.Data.Url);
                            // await s3client.PushPIDFile(pidBytes);
                            // _logger.LogInformation("push pid to s3 successfully");

                            // var picParams = new ECGetPresignUrl
                            // {
                            //     DocType = "PIC",
                            //     FileName = picDetail.FileName,
                            //     ProposalId = proposalId,
                            //     RequestId = requestId,
                            //     PartnerCode = _ecConfig.PartnerCode
                            // };

                            // // get pic presign url
                            // _logger.LogInformation($"start getting pid presigned url: fileName: {picDetail.FileName} requestId: {requestId} partnerCode: {_ecConfig.PartnerCode} proposalId: {proposalId}");
                            // var getPicResult = await _eCRestService.GetPresignUrl(_ecConfig.PartnerCode, picParams, token);
                            // var picUploadResponse = getPicResult.ToObject<ECPresignUrlResponse>();
                            // _logger.LogInformation("get pic resigned-url successfully");

                            // // push pic to s3

                            // _logger.LogInformation($"start uploading pic file to s3: fileName: {picDetail.FileName} url: {picUploadResponse?.Data?.Url}");
                            // s3client = RestService.For<IECS3RestService>(picUploadResponse?.Data?.Url);
                            // await s3client.PushPICFile(picBytes);
                            // _logger.LogInformation("push pic to s3 successfully");
                        // }

                        // upload file using sftp
                        _ecUploadFileService.UploadFIle(uploadFiles);
                        // [todo] delete file after uploading
                        fullLoanRequest.DocCollectingList = listDocs;
                        fullLoanRequest.ImgIdCard = imgIdCard;
                        fullLoanRequest.ImgSelfie = imgSelfie;

                        await _ecDataProcessingService.CreateFullLoanPayload(customerId, fullLoanRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private byte[] ConverToPdf(IEnumerable<UploadedMedia> medias)
        {
            Document document = new Document(PageSize.Letter, 10f, 10f, 10f, 0f);
            using var memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            writer.CloseStream = false;
            document.Open();
            try
            {
                //Resize image depend upon your need
                float pageWidth = document.PageSize.Width - (10 + 10);
                float pageHeight = document.PageSize.Height - (10 + 10);

                foreach (var media in medias)
                {
                    Image img = Image.GetInstance(media.Uri);

                    // img.SetAbsolutePosition(10, 10);
                    img.ScaleToFit(pageWidth, pageHeight);

                    //Give space before image
                    img.SpacingBefore = 10f;

                    //Give some space after the image
                    // jpg.SpacingAfter = 10f;
                    img.Alignment = Element.ALIGN_LEFT;
                    document.Add(img);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                document.Close();
            }

            return memoryStream.ToArray();
        }
    }
}
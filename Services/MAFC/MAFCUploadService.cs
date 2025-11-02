using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Services.Files;
using _24hplusdotnetcore.Services.Storage;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMAFCUploadService
    {
        Task<bool> PushUnderSystemAsync(Customer customer, string processId);
        Task<bool> processDeferDataAsync(DataMAFCProcessingModel process, Customer customer);
    }

    public class MAFCUploadService : IMAFCUploadService, IScopedLifetime
    {
        private readonly ILogger<MAFCUploadService> _logger;
        private readonly IRestMAFCUploadService _restMAFCUploadService;
        private readonly IRestMAFCDataEntryService _restMAFCDataEntryService;
        private readonly DataMAFCProcessingServices _dataMAFCProcessingServices;
        private readonly CustomerServices _customerServices;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly MAFCConfig _mafcConfig;
        private readonly IStorageService _storageService;

        public MAFCUploadService(
            ILogger<MAFCUploadService> logger,
            IRestMAFCUploadService restMAFCUploadService,
            IRestMAFCDataEntryService restMAFCDataEntryService,
            DataMAFCProcessingServices dataMAFCProcessingServices,
            CustomerServices customerServices,
            CustomerDomainServices customerDomainServices,
            IFileService fileService,
            IWebHostEnvironment hostingEnvironment,
            IOptions<MAFCConfig> mafcConfig,
            IMapper mapper,
            IStorageService storageService)
        {
            _logger = logger;
            _restMAFCUploadService = restMAFCUploadService;
            _restMAFCDataEntryService = restMAFCDataEntryService;
            _dataMAFCProcessingServices = dataMAFCProcessingServices;
            _customerServices = customerServices;
            _customerDomainServices = customerDomainServices;
            _fileService = fileService;
            _hostingEnvironment = hostingEnvironment;
            _mafcConfig = mafcConfig.Value;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<bool> processDeferDataAsync(DataMAFCProcessingModel process, Customer customer)
        {
            bool valid = true;
            string warning = "Y";
            string comment = "";

            List<string> createdFiles = new List<string>();
            List<DocumentUpload> listDocs = new List<DocumentUpload>();
            if (customer.ReturnDocuments.Any())
            {

                // generate pdf if it doesn't have
                var checklist = customer.ReturnDocuments.ToList();
                var dnGroup = checklist.Find(x => x.GroupId == 1);
                var dnDoc = dnGroup.Documents.FirstOrDefault();
                if (customer.Result.GeneratePdf && dnDoc.DocumentCode == "DN" && dnDoc.UploadedMedias?.Any() != true)
                {
                    var file = await _fileService.GenarateDNFile(customer.Id);
                    List<UploadedMedia> dn = new List<UploadedMedia>()
                    {
                        new UploadedMedia () {
                            Name = file.FileName,
                            Type = "pdf",
                            Uri = file.AbsolutePath
                        }
                    };
                    dnDoc.UploadedMedias = dn;

                    customer.ReturnDocuments = checklist;
                    await _customerDomainServices.ReplaceOneAsync(customer, nameof(processDeferDataAsync));
                }

                for (int i = 0; i < customer.ReturnDocuments.Count(); i++)
                {
                    var group = customer.ReturnDocuments.ToList()[i];
                    foreach (var doc in group.Documents)
                    {
                        if (doc.UploadedMedias != null && doc.UploadedMedias.Count() > 0)
                        {
                            listDocs.Add(doc);
                        }
                    }
                }

                if (listDocs.Any())
                {
                    for (int i = 0; i < listDocs.Count; i++)
                    {
                        var doc = listDocs[i];
                        if (i == listDocs.Count - 1)
                        {
                            warning = "N";
                            comment = !string.IsNullOrEmpty(customer.CaseNote) ? customer.CaseNote : "Đã bổ sung";
                        }

                        UploadedMedia media = doc.UploadedMedias?.FirstOrDefault();
                        byte[] bytes = new byte[0];
                        string fileName = media.Name;

                        if (IsConvertUploadMediaToPdf(doc.DocumentCode) && media?.Type?.ToLower().IndexOf("pdf") == -1)
                        {
                            bytes = ConverToPdf(doc.UploadedMedias);
                            fileName = $"{doc.DocumentCode}-{DateTime.Now:yyyyMMddHHmmssffff}.pdf";
                        }

                        if (!IsConvertUploadMediaToPdf(doc.DocumentCode) && media?.Type?.ToLower().IndexOf("jpg") == -1)
                        {
                            fileName = $"{doc.DocumentCode}-{DateTime.Now:yyyyMMddHHmmssffff}.jpg";
                        }

                        if (!bytes.Any() && !string.IsNullOrEmpty(media?.Uri))
                        {
                            var response = await _storageService.GetObjectAsync(media.Uri);
                            bytes = response.Bytes;
                        }

                        if(bytes.Any())
                        {
                            valid = PushPendingUnderSystem(customer.MAFCId, process.Id, doc.DocumentCode, fileName, bytes, warning, comment);
                        }
                    }
                }
                else
                {
                    warning = "N";
                    comment = !string.IsNullOrEmpty(customer.CaseNote) ? customer.CaseNote : "Đã bổ sung";
                    valid = PushPendingUnderSystem(customer.MAFCId, process.Id, "", "", null, warning, comment);
                }
            }
            return valid;
        }

        public async Task<bool> PushUnderSystemAsync(Customer customer, string processId)
        {
            try
            {
                var requestDto = new MAFCUploadRequestDto();
                List<MAFCFileUploadRequestDto> files = new List<MAFCFileUploadRequestDto>();
                var token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_mafcConfig.DataUpload.Username + ":" + _mafcConfig.DataUpload.Password));
                var client = new RestClient(_mafcConfig.Host + "/thirdparty/pushundersystem");
                var request = new RestRequest(Method.POST);

                client.Timeout = -1;
                requestDto.Appid = customer.MAFCId;
                request.AddHeader("Authorization", "Basic " + token);
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddParameter("warning", "N");
                request.AddParameter("warning_msg", "");
                request.AddParameter("appid", customer.MAFCId);
                request.AddParameter("salecode", "EXT_24H");
                request.AddParameter("usersname", "EXT_24H");
                request.AddParameter("password", "");

                foreach (var group in customer.Documents)
                {
                    foreach (var doc in group.Documents?.Where(x => x.UploadedMedias?.Any() == true))
                    {
                        UploadedMedia media = doc.UploadedMedias.FirstOrDefault();
                        byte[] bytes = new byte[0];
                        string fileName = media.Name;
                        
                        if (IsConvertUploadMediaToPdf(doc.DocumentCode) && media.Type?.ToLower().IndexOf("pdf") == -1)
                        {
                            bytes = ConverToPdf(doc.UploadedMedias);
                            fileName = $"{doc.DocumentCode}-{DateTime.Now:yyyyMMddHHmmssffff}.pdf";
                        }

                        if (!IsConvertUploadMediaToPdf(doc.DocumentCode) && media.Type?.ToLower().IndexOf("jpg") == -1)
                        {
                            fileName = $"{doc.DocumentCode}-{DateTime.Now:yyyyMMddHHmmssffff}.jpg";
                        }

                        if (!bytes.Any() && !string.IsNullOrEmpty(media?.Uri))
                        {
                            var obj = await _storageService.GetObjectAsync(media.Uri);
                            bytes = obj.Bytes;
                        }

                        if(bytes.Any())
                        {
                            request.AddFile(doc.DocumentCode, bytes, fileName);

                            var file = new MAFCFileUploadRequestDto
                            {
                                DocumentCode = doc.DocumentCode,
                                FileName = fileName
                            };
                            files.Add(file);
                        }
                    }
                }

                IRestResponse response = client.Execute(request);
                requestDto.Files = files;
                PayloadModel payload = new PayloadModel();
                payload.Message = "PushUnderSystem";
                payload.Payload = JsonConvert.SerializeObject(requestDto);
                payload.Response = JsonConvert.SerializeObject(response.Content);
                _dataMAFCProcessingServices.AddPayload(processId, payload);

                dynamic content = JsonConvert.DeserializeObject<dynamic>(response.Content);

                return content != null && content.success != null ? content.success : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                PayloadModel payload = new PayloadModel();
                payload.Message = "PushUnderSystem";
                payload.Response = JsonConvert.SerializeObject(ex.Message);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                _dataMAFCProcessingServices.UpdateStatus(processId, DataProcessingStatus.ERROR, "");
                return false;
            }
        }

        private bool PushPendingUnderSystem(int mafcId, string processId, string documentCode, string fileName, byte[] bytes, string warning, string comment)
        {
            try
            {
                var requestDto = new MAFCUploadRequestDto();
                MAFCFileUploadRequestDto files = new MAFCFileUploadRequestDto()
                {
                    DocumentCode = documentCode,
                    FileName = fileName,
                };
                var token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_mafcConfig.DataUploadDefer.Username + ":" + _mafcConfig.DataUploadDefer.Password));
                var client = new RestClient(_mafcConfig.Host + "/thirdparty/pushpenhistory");
                var request = new RestRequest(Method.POST);
                // request.AddHeader("Authorization", "Bearer " + token + "");

                client.Timeout = -1;
                requestDto.Appid = mafcId;
                requestDto.Warning = warning;
                requestDto.Warning_msg = comment;
                request.AddHeader("Authorization", "Basic " + token);
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddParameter("deferstatus", warning);
                request.AddParameter("defercode", "S1");
                request.AddParameter("appid", mafcId);
                request.AddParameter("userid", "EXT_24H");
                request.AddParameter("comment", comment);
                request.AddParameter("usersname", "EXT_24H");
                request.AddParameter("password", "");
                request.AlwaysMultipartFormData = true;
                if (!string.IsNullOrEmpty(documentCode))
                {
                    request.AddFile(documentCode, bytes, fileName);
                }

                IRestResponse response = client.Execute(request);
                requestDto.Files = new List<MAFCFileUploadRequestDto>() { files };
                PayloadModel payload = new PayloadModel();
                payload.Message = "PushPenUnderSystem";
                payload.Payload = JsonConvert.SerializeObject(requestDto);
                payload.Response = JsonConvert.SerializeObject(response.Content);
                _dataMAFCProcessingServices.AddPayload(processId, payload);

                dynamic content = JsonConvert.DeserializeObject<dynamic>(response.Content);
                if (content != null)
                {
                    if (content.success == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                PayloadModel payload = new PayloadModel();
                payload.Message = "PushPenUnderSystem";
                payload.Response = JsonConvert.SerializeObject(ex.Message);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                _dataMAFCProcessingServices.UpdateStatus(processId, DataProcessingStatus.ERROR, "");
                return false;
            }
        }

        private byte[] ConverToPdf(IEnumerable<UploadedMedia> medias)
        {
            Document document = new Document(PageSize.A1, 10f, 10f, 10f, 0f);
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
                    
                    if (img.Width > pageWidth) {
                        img.ScaleToFit(pageWidth, pageHeight);
                    }

                    // img.SetAbsolutePosition(10, 10);
                    // img.ScaleToFit(pageWidth, pageHeight);

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

        private bool IsConvertUploadMediaToPdf(string documentCode)
        {
            var documentCodes = new string[] { "IDFRONT", "IDBACK", "CMC", "DDKD", "HINH" };
            return !documentCodes.Contains(documentCode);
        }
    }
}

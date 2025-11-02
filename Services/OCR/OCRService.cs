using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.StorageModels;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.Storage;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OCREntity = _24hplusdotnetcore.Models.OCR;

namespace _24hplusdotnetcore.Services.OCR
{
    public interface IOCRService
    {
        Task<OCRTranferResponse> TransferAsync(OCRTranferRequest oCRTranferRequest, string userId);
        Task<OCREntity> InsertOneAsync(OCREntity ocr);
        Task<PagingResponse<OCRInfoResponse>> GetListAsync(string userId, PagingRequest pagingRequest);
        Task CheckORCResultAsync();
        Task<OCRInfoResponse> GetAsync(string id);
    }

    public class OCRService : IOCRService, IScopedLifetime
    {
        private readonly ILogger<OCRService> _logger;
        private readonly IRestOCRService _restOCRService;
        private readonly IStorageService _storageService;
        private readonly IMongoCollection<OCREntity> _ocrEntity;
        private readonly IMapper _mapper;

        public OCRService(
            ILogger<OCRService> logger,
            IRestOCRService restOCRService,
            IStorageService storageService,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            _restOCRService = restOCRService;
            _storageService = storageService;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _ocrEntity = database.GetCollection<OCREntity>(MongoCollection.OCR);
            _mapper = mapper;
        }

        public async Task<OCRTranferResponse> TransferAsync(OCRTranferRequest oCRTranferRequest, string userId)
        {
            try
            {
                var streams = oCRTranferRequest.Files
                .Where(file => file.Length > 0)
                .Select(file =>
                {
                    var stream = file.OpenReadStream();
                    var streamPart = new StreamPart(stream, file.FileName, file.ContentType);
                    return streamPart;
                });

                var result = await _restOCRService.TransferAsync(oCRTranferRequest.Type, streams);

                var files = await UploadFileAsync(oCRTranferRequest.Files);
                var ocr = new OCREntity
                {
                    UserId = userId,
                    PayLoad = new OCRPayLoad
                    {
                        Type = oCRTranferRequest.Type.ToString(),
                        Files = _mapper.Map<IEnumerable<OCRFile>>(files)
                    },
                    Response = _mapper.Map<OCRResponse>(result)
                };
                await InsertOneAsync(ocr);

                return result;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<OCRReceiveResponse> ReceiveAsync(string keyImages)
        {
            try
            {
                var result = await _restOCRService.ReceiveAsync(keyImages);

                return result;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<OCREntity> InsertOneAsync(OCREntity ocr)
        {
            await _ocrEntity.InsertOneAsync(ocr);
            return ocr;
        }

        private async Task<IEnumerable<StorageFileResponse>> UploadFileAsync(IEnumerable<IFormFile> files)
        {
            var response = new List<StorageFileResponse>();
            foreach (var file in files)
            {
                byte[] fileBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                string filename = file.FileName;
                var lastDotPosition = filename.LastIndexOf(".");
                if (lastDotPosition != -1)
                {
                    filename.Insert(lastDotPosition, $"-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}");
                }
                var result = await _storageService.UploadFileAsync("OCR", file.FileName, fileBytes);
                response.Add(result);
            }
            return response;
        }

        public async Task<PagingResponse<OCRInfoResponse>> GetListAsync(string userId, PagingRequest pagingRequest)
        {
            try
            {
                Expression<Func<OCREntity, bool>> filter = x => x.UserId == userId;

                var totalRecord = await _ocrEntity.Find(filter).CountDocumentsAsync();
                var ocrs = await _ocrEntity
                    .Find(filter)
                    .SortByDescending(x => x.Createdtime)
                    .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                    .Limit(pagingRequest.PageSize)
                    .ToListAsync();

                var result = new PagingResponse<OCRInfoResponse>
                {
                    TotalRecord = totalRecord,
                    Data = _mapper.Map<IEnumerable<OCRInfoResponse>>(ocrs)
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CheckORCResultAsync()
        {
            try
            {
                var ocrs = await _ocrEntity.Find(x => string.Equals(x.Status, OCRStatus.INPROGRESS)).ToListAsync();

                if (ocrs?.Any() != true)
                {
                    return;
                }

                foreach (var ocr in ocrs)
                {
                    await CheckORCResultAsync(ocr);
                }

                await ReplaceManyAsync(ocrs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task CheckORCResultAsync(OCREntity ocr)
        {
            ocr.Modifiedtime = DateTime.Now;

            if (ocr.RetryCount > 2)
            {
                ocr.Status = OCRStatus.ERROR;
                return;
            }

            OCRReceiveResponse receiveResponse = await ReceiveAsync(ocr.Response?.KeyImages);
            ocr.Result = _mapper.Map<OCRResult>(receiveResponse);
            ocr.RetryCount++;

            if(string.IsNullOrEmpty(receiveResponse.Message))
            {
                ocr.Status = OCRStatus.DONE;
                return;
            }

            if(!string.Equals(receiveResponse.Message, OCRStatus.ISBEINGPRECESSED))
            {
                ocr.Status = OCRStatus.ERROR;
            }
        }

        private async Task ReplaceManyAsync(IEnumerable<OCREntity> ocrs)
        {
            var updates = ocrs.Select(ocr => new ReplaceOneModel<OCREntity>(
                    Builders<OCREntity>.Filter.Where(y => y.Id == ocr.Id),
                    ocr));
            await _ocrEntity.BulkWriteAsync(updates);
        }

        public async Task<OCRInfoResponse> GetAsync(string id)
        {
            try
            {
                var ocr = await _ocrEntity.Find(x => x.Id == id).FirstOrDefaultAsync();
                var response = _mapper.Map<OCRInfoResponse>(ocr);
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

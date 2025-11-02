using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IImportFileService
    {
        Task<BaseResponse<ImportFileResponse>> Create(ImportFileDto model);
        Task<BaseResponse<PagingResponse<ImportFileResponse>>> GetList(ImportFilePagingRequest request);
    }

    public class ImportFileService : IImportFileService, IScopedLifetime
    {
        private readonly ILogger<ImportFileService> _logger;
        private readonly IMapper _mapper;
        private readonly IImportFileRepository _importFileRepository;
        private readonly IUserServices _userServices;

        public ImportFileService(ILogger<ImportFileService> logger, 
            IMapper mapper, 
            IImportFileRepository importFileRepository,
            IUserServices userServices)
        {
            _logger = logger;
            _mapper = mapper;
            _importFileRepository = importFileRepository;
            _userServices = userServices;
        }

        public async Task<BaseResponse<ImportFileResponse>> Create(ImportFileDto model)
        {
            try
            {
                var response = new BaseResponse<ImportFileResponse>();
                var validator = new CreateImportFileValidation();
                ValidationResult result = validator.Validate(model);

                response.MappingFluentValidation(result);

                if (!response.IsSuccess)
                {
                    return response;
                }

                var importFileModel = _mapper.Map<ImportFile>(model);
                var data = await _importFileRepository.Create(importFileModel);
                response.Data = _mapper.Map<ImportFileResponse>(data);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<PagingResponse<ImportFileResponse>>> GetList(ImportFilePagingRequest request)
        {
            try
            {
                var filterCreators = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_ImportFile_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_ImportFile_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_ImportFile_ViewAll,
                    PermissionCost.AsmPermission.Asm_ImportFile_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_ImportFile_ViewAll);

                var response = new BaseResponse<PagingResponse<ImportFileResponse>>();
                var result = await _importFileRepository.GetList(request, filterCreators);
                var count = await _importFileRepository.Count(request, filterCreators);

                response.Data = new PagingResponse<ImportFileResponse>
                {
                    Data = _mapper.Map<ImportFileResponse[]>(result),
                    TotalRecord = count
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

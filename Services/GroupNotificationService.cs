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
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IGroupNotificationService
    {
        Task<BaseResponse<GroupNotificationDetailResponse>> GetById(string id);
        Task<BaseResponse<GroupNotificationDetailResponse>> Create(CreateGroupNotificationDto request);
        Task<BaseResponse<GroupNotificationDetailResponse>> Update(string id, UpdateGroupNotificationDto request);
        Task<BaseResponse<GroupNotificationDetailResponse>> GetDetailByCode(string groupCode);
        Task<BaseResponse<PagingResponse<GroupNotificationDetailResponse>>> GetList(string textSearch = null, int pageIndex = 1, int pageSize = 10);
        Task<BaseResponse<bool>> Delete(string id);
    }

    public class GroupNotificationService : IGroupNotificationService, IScopedLifetime
    {
        private readonly ILogger<GroupNotificationService> _logger;
        private readonly IMapper _mapper;

        private readonly IGroupNotificationRepository _groupNotificationRepository;

        public GroupNotificationService(ILogger<GroupNotificationService> logger, IMapper mapper, IGroupNotificationRepository groupNotificationRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _groupNotificationRepository = groupNotificationRepository;
        }

        public async Task<BaseResponse<GroupNotificationDetailResponse>> GetDetailByCode(string groupCode)
        {
            try
            {
                var result = new BaseResponse<GroupNotificationDetailResponse>();
                var groupDetail = await _groupNotificationRepository.GetDetailByCode(groupCode);

                result.Data = _mapper.Map<GroupNotificationDetailResponse>(groupDetail);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<GroupNotificationDetailResponse>> Create(CreateGroupNotificationDto request)
        {
            try
            {
                var response = new BaseResponse<GroupNotificationDetailResponse>();

                var validator = new CreateGroupNotificationValidation();

                ValidationResult result = validator.Validate(request);

                response.MappingFluentValidation(result);

                if (!response.IsSuccess)
                {
                    return response;
                }

                var isExist = await _groupNotificationRepository.GetDetailByName(request.GroupName);

                if (isExist != null)
                {
                    return response.ReturnWithMessage("GroupName has been already existed.");
                }

                var data = await _groupNotificationRepository.Create(request);

                response.Data = _mapper.Map<GroupNotificationDetailResponse>(data);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<BaseResponse<GroupNotificationDetailResponse>> GetById(string id)
        {
            try
            {
                var result = new BaseResponse<GroupNotificationDetailResponse>();

                var groupNotificationDetail = await _groupNotificationRepository.Get(id);

                result.Data = _mapper.Map<GroupNotificationDetailResponse>(groupNotificationDetail);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<GroupNotificationDetailResponse>> Update(string id, UpdateGroupNotificationDto request)
        {
            try
            {
                var response = new BaseResponse<GroupNotificationDetailResponse>();

                var validator = new UpdateGroupNotificationValidation();
                ValidationResult result = validator.Validate(request);

                response.MappingFluentValidation(result);

                if (!response.IsSuccess)
                {
                    return response;
                }

                var detail = await _groupNotificationRepository.Get(id);

                if (detail == null)
                {
                    return response.ReturnWithMessage($"Group {id} does not exist.");
                }

                var updatedEntity = _mapper.Map<GroupNotification>(request);

                updatedEntity.Id = id;
                updatedEntity.GroupCode = detail.GroupCode;

                var updateResult = await _groupNotificationRepository.Update(updatedEntity);

                response.Data = _mapper.Map<GroupNotificationDetailResponse>(updateResult);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<PagingResponse<GroupNotificationDetailResponse>>> GetList(string textSearch = null, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var response = new BaseResponse<PagingResponse<GroupNotificationDetailResponse>>();
                var result = await _groupNotificationRepository.GetList(textSearch, pageIndex, pageSize);
                var count = await _groupNotificationRepository.CountGroupNotification(textSearch);
                response.Data = new PagingResponse<GroupNotificationDetailResponse>
                {
                    Data = _mapper.Map<GroupNotificationDetailResponse[]>(result),
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

        public async Task<BaseResponse<bool>> Delete(string id)
        {
            try
            {
                var result = new BaseResponse<bool>();

                await _groupNotificationRepository.Delete(id);

                result.Data = true;

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

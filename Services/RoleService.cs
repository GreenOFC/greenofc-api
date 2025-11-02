using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Roles;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IRoleService
    {
        IEnumerable<Role> GetList(IEnumerable<string> ids);
        Task<PagingResponse<GetRoleResponse>> GetAsync(GetRoleRequest getRoleRequest);
        Task CreateAsync(CreateRoleRequest createRoleRequest);
        Task UpdateAsync(string id, UpdateRoleRequest updateRoleRequest);
        Task<GetRoleDetailResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
    }

    public class RoleService : IRoleService, IScopedLifetime
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;

        public RoleService(
            ILogger<RoleService> logger,
            IRoleRepository roleRepository,
            IMapper mapper,
            IUserLoginService userLoginService)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
        }
        
        public IEnumerable<Role> GetList(IEnumerable<string> ids)
        {
            return _roleRepository.FilterBy(x => x.IsDeleted != true && ids.Contains(x.Id));
        }

        public async Task<PagingResponse<GetRoleResponse>> GetAsync(GetRoleRequest getRoleRequest)
        {
            try
            {
                var roles = await _roleRepository.GetAsync(
                   getRoleRequest.TextSearch,
                   getRoleRequest.PageIndex,
                   getRoleRequest.PageSize);

                var total = await _roleRepository.CountAsync(getRoleRequest.TextSearch);

                var result = new PagingResponse<GetRoleResponse>
                {
                    TotalRecord = total,
                    Data = _mapper.Map<IEnumerable<GetRoleResponse>>(roles)
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateRoleRequest createRoleRequest)
        {
            try
            {
                var role = _mapper.Map<Role>(createRoleRequest);
                role.Creator = _userLoginService.GetUserId();
                await _roleRepository.InsertOneAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateRoleRequest updateRoleRequest)
        {
            try
            {
                var role = await _roleRepository.FindOneAsync(x => x.Id == id && x.IsDeleted != true);
                if (role == null)
                {
                    throw new ArgumentException(Message.ROLE_NOT_FOUND);
                }

                _mapper.Map(updateRoleRequest, role);
                role.Modifier = _userLoginService.GetUserId();
                role.ModifiedDate = DateTime.Now;

                await _roleRepository.ReplaceOneAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetRoleDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var role = await _roleRepository.GetDetailAsync(id);
                if (role == null)
                {
                    throw new ArgumentException(Message.ROLE_NOT_FOUND);
                }

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var role = await _roleRepository.FindOneAsync(x => x.Id == id && x.IsDeleted != true);
                if (role == null)
                {
                    throw new ArgumentException(Message.ROLE_NOT_FOUND);
                }

                role.IsDeleted = true;
                role.DeletedDate = DateTime.Now;
                role.DeletedBy = _userLoginService.GetUserId();

                await _roleRepository.ReplaceOneAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

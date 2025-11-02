using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos.Permissions;
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
    public interface IPermissionService
    {
        IEnumerable<Permission> GetList(IEnumerable<string> ids);
        Task<IEnumerable<PermissionDto>> GetAsync();
        Task<bool> IsPermissionAsync(string userId, string permission);
    }
    public class PermissionService : IPermissionService, IScopedLifetime
    {
        private readonly ILogger<PermissionService> _logger;
        private readonly IMongoRepository<Permission> _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PermissionService(
            ILogger<PermissionService> logger,
            IMongoRepository<Permission> permissionRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _logger = logger;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<PermissionDto>> GetAsync()
        {
            try
            {
                var permissions = _permissionRepository.FilterBy(x => true);
                var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);
                return Task.FromResult(permissionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<Permission> GetList(IEnumerable<string> ids)
        {
            return _permissionRepository.FilterBy(x => ids.Contains(x.Id));
        }
        public async Task<bool> IsPermissionAsync(string userId, string permission)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(User)));
                }
                if (user.RoleIds == null || user.RoleIds.Count() == 0)
                {
                    return false;
                }
                var roles = await _roleRepository.FilterByAsync(x => user.RoleIds.Contains(x.Id));

                var permissionIds = roles.SelectMany(x => x.PermissionIds ?? new List<string>());
                if (permissionIds.Count() == 0)
                {
                    return false;
                }
                
                var permissions = _permissionRepository.FilterBy(x => permissionIds.Contains(x.Id));
                var listPermissions = permissions.Select(x => x.Value);
                if (listPermissions.Contains(permission))
                {
                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}

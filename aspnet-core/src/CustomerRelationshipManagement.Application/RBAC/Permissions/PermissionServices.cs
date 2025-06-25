using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.RBAC.Permissions
{
    public class PermissionServices : ApplicationService, IPermissionServices
    {
        private readonly IRepository<PermissionInfo, Guid> permissionInfoRepo;

        public PermissionServices(IRepository<PermissionInfo,Guid> permissionInfoRepo)
        {
            this.permissionInfoRepo = permissionInfoRepo;
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="createPermissionDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<PermissionInfo>> AddPermission(CreatePermissionDto createPermissionDto)
        {
            try
            {
                  var permissionInfoDto = ObjectMapper.Map<CreatePermissionDto, PermissionInfo>(createPermissionDto);
                  var permissionInfo = await permissionInfoRepo.InsertAsync(permissionInfoDto);
                  return ApiResult<PermissionInfo>.Success(ResultCode.Success, permissionInfo);
            }
            catch (Exception ex)
            {
                return ApiResult<PermissionInfo>.Fail(ex.Message, ResultCode.Fail);
            }
        }
    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.Permissions
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public interface IPermissionServices: IApplicationService
    {
        /// <summary>
        /// `创建权限`
        /// </summary>
        /// <param name="createPermissionDto"></param>
        /// <returns></returns>
        Task<ApiResult<PermissionDto>> AddPermission(CreatePermissionDto createPermissionDto);

        /// <summary>
        /// 获取系统中所有权限（含分组字段），用于前端构建权限树
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionDto>> GetAllPermissionsAsync();
    }
}

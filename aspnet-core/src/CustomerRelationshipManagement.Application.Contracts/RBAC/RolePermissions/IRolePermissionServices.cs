using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.RolePermissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.RolePermissions
{
    public interface IRolePermissionServices: IApplicationService
    {
        /// <summary>
        ///  分配权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResult> AssignPermissionsAsync(AssignPermissionsDto input);
        /// <summary>
        /// 获取已分配权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<ApiResult<List<Guid>>> GetAssignedPermissionIds(Guid roleId);
    }
}

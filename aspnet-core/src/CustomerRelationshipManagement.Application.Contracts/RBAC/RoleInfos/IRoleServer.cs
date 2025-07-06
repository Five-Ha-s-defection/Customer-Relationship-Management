using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBACDtos.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.RoleInfos
{
    public interface IRoleServer: IApplicationService
    {
        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="createOrUpdateRoleInfoDto"></param>
        /// <returns></returns>
        Task<ApiResult<RoleDto>> AddRoleInfo(CreateOrUpdateRoleDto createOrUpdateRoleInfoDto);
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="createOrUpdateRoleInfoDto"></param>
        /// <returns></returns>
        Task<ApiResult<RoleDto>> UpdateRoleInfo(Guid Id,CreateOrUpdateRoleDto createOrUpdateRoleInfoDto);

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult> DeleteRoleInfo(Guid id);
        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<RoleDto>>> GetRoleInfoList();
    }
}

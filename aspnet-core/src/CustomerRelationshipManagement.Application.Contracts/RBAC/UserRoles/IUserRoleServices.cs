using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.UserRoles
{
    public interface IUserRoleServices: IApplicationService
    {
        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="createUserRoleDto"></param>
        /// <returns></returns>
        Task<ApiResult> AddUserRole(CreateUserRoleDto createUserRoleDto);
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<List<Guid>>> GetCreateUserRoleAsync(Guid userId);
    }
}

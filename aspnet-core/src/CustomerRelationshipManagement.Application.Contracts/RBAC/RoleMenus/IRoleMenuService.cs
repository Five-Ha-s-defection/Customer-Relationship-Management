using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Menus;
using CustomerRelationshipManagement.RBACDtos.RoleMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.RoleMenus
{
    /// <summary>
    /// 角色菜单服务接口
    /// 提供角色与菜单关联的管理功能，包括分配菜单给角色和查询角色的菜单权限
    /// </summary>
    public interface IRoleMenuService : IApplicationService
    {
        /// <summary>
        /// 为角色分配菜单权限
        /// 先删除该角色的所有现有菜单权限，然后重新分配新的菜单权限
        /// </summary>
        /// <param name="input">角色菜单创建数据传输对象，包含角色ID和要分配的菜单ID列表</param>
        /// <returns>操作结果</returns>
        Task<ApiResult> AddRoleMenusAsync(CreateRoleMenuDto input);

        /// <summary>
        /// 根据角色ID获取该角色拥有的所有菜单
        /// 用于前端显示角色的菜单权限或进行权限验证
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>该角色拥有的菜单列表</returns>
        Task<ApiResult<List<MenuDto>>> GetMenusByRoleIdAsync(Guid roleId);
    }
}

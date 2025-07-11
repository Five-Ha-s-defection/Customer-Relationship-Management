using System;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Menus;
using CustomerRelationshipManagement.RBACDtos.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.Menus
{
    /// <summary>
    /// 菜单管理应用服务接口
    /// 提供菜单的增删改查功能
    /// </summary>
    public interface IMenuAppService : IApplicationService
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="input">菜单创建数据传输对象</param>
        /// <returns>操作结果</returns>
        Task<ApiResult> CreateMenuAsync(CreateOrUpdateMenuDto input);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="input">菜单更新数据传输对象</param>
        /// <returns>操作结果</returns>
        Task<ApiResult> UpdateMenuAsync(Guid id,CreateOrUpdateMenuDto input);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>操作结果</returns>
        Task<ApiResult> DeleteMenuAsync(Guid id);

        /// <summary>
        /// 获取菜单树形结构
        /// 用于前端构建导航菜单，支持多级菜单嵌套
        /// </summary>
        /// <returns>包含树形结构的菜单列表，每个菜单项包含子菜单Children属性</returns>
        Task<ApiResult<List<MenuDto>>> GetMenuTreeAsync();
    }
} 
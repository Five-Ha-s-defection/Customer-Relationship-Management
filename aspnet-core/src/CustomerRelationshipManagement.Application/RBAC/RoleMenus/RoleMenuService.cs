using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBAC.Menus;
using CustomerRelationshipManagement.RBACDtos.Menus;
using CustomerRelationshipManagement.RBACDtos.RoleMenus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.RBAC.RoleMenus
{
    /// <summary>
    /// 角色菜单服务实现类
    /// 实现角色与菜单关联的管理功能，包括分配菜单给角色和查询角色的菜单权限
    /// </summary>
    [ApiExplorerSettings(GroupName ="v1")]
    [AllowAnonymous]
    public class RoleMenuService : ApplicationService, IRoleMenuService
    {
        // 角色菜单关联仓储，用于数据库操作
        private readonly IRepository<RoleMenuInfo, Guid> _roleMenuRepo;
        // 菜单信息仓储，用于获取菜单详细信息
        private readonly IRepository<MenuInfo, Guid> _menuRepo;

        /// <summary>
        /// 构造函数，注入所需的仓储
        /// </summary>
        /// <param name="roleMenuRepo">角色菜单关联仓储</param>
        /// <param name="menuRepo">菜单信息仓储</param>
        public RoleMenuService(
            IRepository<RoleMenuInfo, Guid> roleMenuRepo,
            IRepository<MenuInfo, Guid> menuRepo)
        {
            _roleMenuRepo = roleMenuRepo;
            _menuRepo = menuRepo;
        }

        /// <summary>
        /// 为角色分配菜单权限
        /// 采用先删除后添加的策略，确保角色菜单权限的准确性
        /// </summary>
        /// <param name="input">角色菜单创建数据传输对象，包含角色ID和要分配的菜单ID列表</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult> AddRoleMenusAsync(CreateRoleMenuDto input)
        {
            try
            {
                //先查看 该角色的菜单权限
                var roleMenus = await _roleMenuRepo.GetListAsync(x => x.RoleId == input.RoleId);
                //判断 该角色是否已经存在菜单权限
                if (roleMenus.Any())
                {
                    // 先删除该角色的所有现有菜单权限，确保权限的准确性
                    await _roleMenuRepo.DeleteAsync(x => x.RoleId == input.RoleId);
                }
                // 根据传入的菜单ID列表，创建新的角色菜单关联实体
                var entities = input.MenuIds.Select(menuId => new RoleMenuInfo
                {
                    RoleId = input.RoleId,    // 设置角色ID
                    MenuId = menuId           // 设置菜单ID
                }).ToList();

                // 批量插入新的角色菜单关联记录
                await _roleMenuRepo.InsertManyAsync(entities, autoSave: true);

                // 返回成功结果
                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                // 异常处理，返回失败信息
                return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 根据角色ID获取该角色拥有的所有菜单
        /// 通过关联查询获取角色的完整菜单信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>该角色拥有的菜单列表</returns>
        public async Task<ApiResult<List<MenuDto>>> GetMenusByRoleIdAsync(Guid roleId)
        {
            try
            {
                // 第一步：根据角色ID查询该角色关联的所有菜单ID
                var menuIds = await (await _roleMenuRepo.GetQueryableAsync())
                    .Where(x => x.RoleId == roleId)    // 筛选指定角色的记录
                    .Select(x => x.MenuId)             // 只选择菜单ID字段
                    .ToListAsync();                    // 异步执行查询

                // 第二步：根据菜单ID列表查询完整的菜单信息
                var menus = await (await _menuRepo.GetQueryableAsync())
                    .Where(x => menuIds.Contains(x.Id))    // 筛选在菜单ID列表中的菜单
                    .ToListAsync();                        // 异步执行查询

                // 第三步：使用ABP框架的ObjectMapper将实体列表转换为DTO列表
                var dtos = ObjectMapper.Map<List<MenuInfo>, List<MenuDto>>(menus);

                // 返回成功结果，包含菜单列表
                return ApiResult<List<MenuDto>>.Success(ResultCode.Success, dtos);
            }
            catch (Exception ex)
            {
                // 异常处理，返回失败信息
                return ApiResult<List<MenuDto>>.Fail(ex.Message, ResultCode.Fail);
            }
        }
    }
}

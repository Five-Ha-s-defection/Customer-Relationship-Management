using CustomerRelationshipManagement.RBAC.Menus;
using CustomerRelationshipManagement.RBAC.Permissions;
using CustomerRelationshipManagement.RBAC.RolePermissions;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.UserPermissions;
using CustomerRelationshipManagement.RBAC.UserRoles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Menus;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class UserProfileManager:DomainService
    {
        private readonly IRepository<UserInfo, Guid> userRep;
        private readonly IRepository<UserRoleInfo, Guid> userRoleRepo;
        private readonly IRepository<RoleInfo, Guid> roleRepo;
        private readonly IRepository<RolePermissionInfo, Guid> rolePermissionRepo;
        private readonly IRepository<PermissionInfo, Guid> permissionRepo;
        private readonly IRepository<UserPermissionInfo, Guid> userPermissionRepo;
        private readonly IRepository<MenuInfo> menuRepo;
        private readonly IObjectMapper objectMapper;

        public UserProfileManager(IRepository<UserInfo, Guid> userRep,CurrentUser currentUser, IRepository<UserRoleInfo, Guid> userRoleRepo, IRepository<RoleInfo, Guid> roleRepo, IRepository<RolePermissionInfo, Guid> rolePermissionRepo, IRepository<PermissionInfo, Guid> permissionRepo, IRepository<UserPermissionInfo, Guid> userPermissionRepo, IRepository<MenuInfo> menuRepo,IObjectMapper objectMapper)
        {
            this.userRep = userRep;
            this.userRoleRepo = userRoleRepo;
            this.roleRepo = roleRepo;
            this.rolePermissionRepo = rolePermissionRepo;
            this.permissionRepo = permissionRepo;
            this.userPermissionRepo = userPermissionRepo;
            this.menuRepo = menuRepo;
            this.objectMapper = objectMapper;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfoDto> BuildUserProfileAsync(Guid userId)
        {
            try
            {
                //获取用户信息
                var userInfo = await userRep.GetAsync(userId);
                //映射用户信息
                var userInfoDto = objectMapper.Map<UserInfo, UserInfoDto>(userInfo);

                // 获取用户角色
                var roleIds = await (await userRoleRepo.GetQueryableAsync())
                    .Where(x => x.UserId == userId)
                    .Select(x => x.RoleId)
                    .ToListAsync();

                //通过角色id来获取角色名称
                var roleNames = await (await roleRepo.GetQueryableAsync())
                 .Where(x => roleIds.Contains(x.Id))
                 .Select(x => x.RoleName)
                 .ToListAsync();

                //通过用户信息来获取用户权限的id
                var userPermissionIds = await (await userPermissionRepo.GetQueryableAsync())
                   .Where(x => x.UserId == userId)
                   .Select(x => x.PermissionId)
                   .ToListAsync();

                //通过角色信息来获取角色权限的id
                var rolePermissionIds = await (await rolePermissionRepo.GetQueryableAsync())
                            .Where(x => roleIds.Contains(x.RoleId))
                            .Select(x => x.PermissionId)
                            .ToListAsync();
                //获取所有的权限id
                var allPermissionIds = userPermissionIds.Union(rolePermissionIds).Distinct().ToList();



                // 获取所有的权限码
                var permissionCodes = await (await permissionRepo.GetQueryableAsync())
                    .Where(x => allPermissionIds.Contains(x.Id))
                    .Select(x => x.PermissionCode)
                    .ToListAsync();

                // 查询可访问的菜单（根据权限码)
                var menus = await (await menuRepo.GetQueryableAsync())
                    .Where(m => string.IsNullOrEmpty(m.PermissionCode) || permissionCodes.Contains(m.PermissionCode))
                    .OrderBy(m => m.Sort)
                    .ToListAsync();

                // 构建菜单树
                List<MenuDto> BuildMenuTree(List<MenuInfo> allMenus, Guid? parentId = null)
                {
                    return allMenus
                        .Where(m => m.ParentId == parentId)
                        .Select(x => new MenuDto
                        {
                            MenuName = x.MenuName,
                            Path = x.Path,
                            Component = x.Component,
                            Icon = x.Icon,
                            PermissionCode = x.PermissionCode,
                            Hidden = !x.IsVisible,
                            Sort = x.Sort,
                            Children = BuildMenuTree(allMenus, x.Id)
                        })
                        .OrderBy(x => x.Sort)
                        .ToList();

                }
                //获取所有菜单
                var menuTree = BuildMenuTree(menus);
                //将数据补充到dto中
                userInfoDto.Roles = roleNames;
                userInfoDto.Permissions = permissionCodes;
                userInfoDto.Menus = menuTree;

               return userInfoDto;

            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }
    }
}

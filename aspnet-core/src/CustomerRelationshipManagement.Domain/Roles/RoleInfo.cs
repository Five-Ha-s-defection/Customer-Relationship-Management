using CustomerRelationshipManagement.RoleMenus;
using CustomerRelationshipManagement.RolePermissions;
using CustomerRelationshipManagement.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.Roles
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleInfo:FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = string.Empty;
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 是否系统内置角色
        /// </summary>
        public bool IsStatic { get; set; } = false;
        /// <summary>
        /// 用户角色导航属性
        /// </summary>
        public ICollection<UserRoleInfo> UserRoles { get; set; }=new List<UserRoleInfo>();
        /// <summary>
        /// 角色权限导航属性
        /// </summary>
        public ICollection<RolePermissionInfo> RolePermissions { get; set; } = new List<RolePermissionInfo>();
        /// <summary>
        /// 角色菜单导航属性
        /// </summary>
        public ICollection<RoleMenuInfo> RoleMenus { get; set; } = new List<RoleMenuInfo>();

    }
}

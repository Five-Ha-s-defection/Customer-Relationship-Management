using CustomerRelationshipManagement.RBAC.Menus;
using CustomerRelationshipManagement.RBAC.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.RBAC.RoleMenus
{
    /// <summary>
    /// 角色菜单关联信息
    /// </summary>
    public class RoleMenuInfo:Entity<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 角色导航属性
        /// </summary>
        public RoleInfo Role { get; set; } = new RoleInfo();
        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid MenuId { get; set; }
        /// <summary>
        /// 菜单导航属性
        /// </summary>
        public MenuInfo Menu { get; set; } = new MenuInfo();
    }
}

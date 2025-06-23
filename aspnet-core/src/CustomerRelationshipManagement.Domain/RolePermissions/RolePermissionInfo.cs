using CustomerRelationshipManagement.Permissions;
using CustomerRelationshipManagement.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.RolePermissions
{
    /// <summary>
    /// 角色权限信息
    /// </summary>
    public class RolePermissionInfo:Entity<Guid>
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
        /// 权限Id
        /// </summary>
        public Guid PermissionId { get; set; }
        /// <summary>
        /// 权限导航属性
        /// </summary>
        public PermissionInfo Permission { get; set; } = new PermissionInfo();
    }
}

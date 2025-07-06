using CustomerRelationshipManagement.RBAC.Permissions;
using CustomerRelationshipManagement.RBAC.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.RBAC.RolePermissions
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
        /// 权限Id
        /// </summary>
        public Guid PermissionId { get; set; }
        /// <summary>
        /// 角色导航属性
        /// </summary>
        public virtual RoleInfo Role { get; set; }  // ✅ 只做导航，不手动赋值
        /// <summary>
        /// 权限导航属性
        /// </summary>
        public virtual PermissionInfo Permission { get; set; }
    }
}

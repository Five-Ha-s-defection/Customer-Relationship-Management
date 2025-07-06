using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.RBAC.UserRoles
{
    /// <summary>
    /// 用户角色信息
    /// </summary>
    public class UserRoleInfo:Entity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户导航属性
        /// </summary>
        public virtual UserInfo User { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 角色导航属性
        /// </summary>
        public virtual RoleInfo Role { get; set; }

    }
}

using CustomerRelationshipManagement.RBAC.Permissions;
using CustomerRelationshipManagement.RBAC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.RBAC.UserPermissions
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public class UserPermissionInfo:Entity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户导航属性
        /// </summary>
        public UserInfo User { get; set; }=new UserInfo();
        /// <summary>
        /// 权限Id
        /// </summary>
        public Guid PermissionId { get; set; }
        /// <summary>
        /// 权限导航属性
        /// </summary>
        public PermissionInfo Permission { get; set; }=new PermissionInfo();
    }
}

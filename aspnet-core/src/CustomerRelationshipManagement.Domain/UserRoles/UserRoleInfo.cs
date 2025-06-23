using CustomerRelationshipManagement.Roles;
using CustomerRelationshipManagement.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.UserRoles
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
        public UserInfo User { get; set; }=new UserInfo();
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 角色导航属性
        /// </summary>
        public RoleInfo Role { get; set; } = new RoleInfo();

    }
}

using CustomerRelationshipManagement.RBAC.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.UserRoles
{
    public class CreateUserRoleDto
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public List<Guid> RoleIds { get; set; }= new List<Guid>();

    }
}

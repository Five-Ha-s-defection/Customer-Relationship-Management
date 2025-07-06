using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.RolePermissions
{
    public class AssignPermissionsDto
    {

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 权限Id
        /// </summary>
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}

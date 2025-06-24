using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Roles
{
    public class CreateOrUpdateRoleDto
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
    }
}

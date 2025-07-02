using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Permissions
{
    public class PermissionDto
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; set; } = string.Empty;
        /// <summary>
        /// 权限代码
        /// </summary>
        public string PermissionCode { get; set; } = string.Empty;
        /// <summary>
        /// 权限分组
        /// </summary>
        public string GroupName { get; set; } = string.Empty;
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}

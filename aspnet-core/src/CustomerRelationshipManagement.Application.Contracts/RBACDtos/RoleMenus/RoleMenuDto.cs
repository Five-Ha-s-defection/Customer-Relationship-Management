using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.RoleMenus
{
    /// <summary>
    /// 角色菜单数据传输对象
    /// 表示单个角色与单个菜单的关联关系
    /// </summary>
    public class RoleMenuDto
    {
        /// <summary>
        /// 角色ID
        /// 关联的角色标识
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// 关联的菜单标识
        /// </summary>
        public Guid MenuId { get; set; }
    }
}

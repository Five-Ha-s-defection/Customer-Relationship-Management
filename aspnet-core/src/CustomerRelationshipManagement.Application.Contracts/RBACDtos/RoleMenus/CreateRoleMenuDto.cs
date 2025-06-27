using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.RoleMenus
{
    /// <summary>
    /// 角色菜单创建数据传输对象
    /// 用于为指定角色分配菜单权限，支持批量分配多个菜单
    /// </summary>
    public class CreateRoleMenuDto
    {
        /// <summary>
        /// 角色ID
        /// 指定要分配菜单权限的角色
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 菜单ID列表
        /// 要分配给该角色的菜单ID集合，支持同时分配多个菜单
        /// </summary>
        public List<Guid> MenuIds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Menus
{
    public class MenuDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; } = string.Empty;
        /// <summary>
        /// 路径 如 /order/list
        /// </summary>
        public string Path { get; set; } = string.Empty;
        /// <summary>
        ///  前端组件路径，如 "views/order/List.vue"
        /// </summary>
        public string Component { get; set; } = string.Empty;
        /// <summary>
        /// 权限图标
        /// </summary>
        public string Icon { get; set; } = string.Empty;
        /// <summary>
        /// 权限码（用于路由及权限判断）
        /// </summary>
        public string PermissionCode { get; set; } = string.Empty;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Hidden { get; set; } = true;
        /// <summary>
        ///  排序（前端的侧边栏进行排序）
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 子菜单(构架树形结构)
        /// </summary>
        public List<MenuDto> Children { get; set; }


    }
}

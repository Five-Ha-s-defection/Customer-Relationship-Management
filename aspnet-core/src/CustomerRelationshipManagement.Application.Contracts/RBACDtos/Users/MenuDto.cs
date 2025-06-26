using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Users
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
        /// 权限码
        /// </summary>
        public string PermissionCode { get; set; } = string.Empty;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; } = true;
        /// <summary>
        ///  排序
        /// </summary>
        public int Sort { get; set; }


    }
}

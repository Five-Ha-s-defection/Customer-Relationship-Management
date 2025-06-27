using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Menus
{
    public class CreateOrUpdateMenuDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        ///  权限码
        /// </summary>
        public string PermissionCode { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; } = true;
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }  // 根节点为空
    }
}

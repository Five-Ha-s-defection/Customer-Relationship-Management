using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.SearchMenus.Dto
{
    /// <summary>
    /// 菜单搜索结果数据传输对象
    /// </summary>
    public class MenuSearchResultDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 权限码
        /// </summary>
        public string PermissionCode { get; set; } = string.Empty;

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 父级菜单名称
        /// </summary>
        public string ParentMenuName { get; set; } = string.Empty;

        /// <summary>
        /// 菜单层级路径（如：系统管理/用户管理/用户列表）
        /// </summary>
        public string MenuPath { get; set; } = string.Empty;

        /// <summary>
        /// 搜索匹配度分数
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 匹配的关键字高亮
        /// </summary>
        public string HighlightedMenuName { get; set; } = string.Empty;
    }
}

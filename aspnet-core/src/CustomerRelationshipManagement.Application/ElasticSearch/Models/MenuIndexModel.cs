using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.ElasticSearch.Models
{
    /// <summary>
    /// ElasticSearch菜单索引模型
    /// </summary>
    public class MenuIndexModel
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
        /// 菜单名称（用于全文搜索）
        /// </summary>
        public string MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 菜单名称拼音（用于拼音搜索）
        /// </summary>
        public string MenuNamePinyin { get; set; } = string.Empty;

        /// <summary>
        /// 菜单名称首字母（用于首字母搜索）
        /// </summary>
        public string MenuNameInitials { get; set; } = string.Empty;

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
        /// 菜单层级路径
        /// </summary>
        public string MenuPath { get; set; } = string.Empty;

        /// <summary>
        /// 菜单层级深度
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModificationTime { get; set; }

        /// <summary>
        /// 搜索关键字（包含所有可搜索字段的文本）
        /// </summary>
        public string SearchText { get; set; } = string.Empty;
    }
} 
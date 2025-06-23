using CustomerRelationshipManagement.RBAC.RoleMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.RBAC.Menus
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    public class MenuInfo : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 父级菜单Id
        /// </summary>
        public Guid? ParentId { get; set; }
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
        public string Icon { get; set; }= string.Empty;
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
        /// <summary>
        /// 角色菜单
        /// </summary>
        public ICollection<RoleMenuInfo> RoleMenus { get; set; } = new List<RoleMenuInfo>();

    }
}

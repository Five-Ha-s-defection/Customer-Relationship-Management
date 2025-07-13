using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.SearchMenus.Dto
{
    /// <summary>
    /// 菜单搜索数据传输对象
    /// </summary>
    public class MenuSearchDto
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 是否只搜索可见菜单
        /// </summary>
        public bool OnlyVisible { get; set; } = true;

        /// <summary>
        /// 父级菜单ID（用于搜索特定父级下的菜单）
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}

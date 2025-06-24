using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CXS.DTOS.CategoryMangamentDto
{
    public class CreateUpdateCategoryDtos
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName { get; set; } = string.Empty;

        /// <summary>
        /// 父级分类ID (可为空，顶级分类没有父级)
        /// </summary>
        public Guid ParentId { get; set; } = Guid.Empty;

        /// <summary>
        /// 分类描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}

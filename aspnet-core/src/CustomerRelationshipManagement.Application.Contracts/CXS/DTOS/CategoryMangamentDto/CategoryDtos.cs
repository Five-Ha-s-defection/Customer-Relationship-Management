using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/ProductCategory/Categorys/Category.cs
namespace CustomerRelationshipManagement.ProductCategory.Categorys
========
namespace CustomerRelationshipManagement.CXS.DTOS.CategoryMangamentDto
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Application.Contracts/CXS/DTOS/CategoryMangamentDto/CategoryDtos.cs
{
    public class CategoryDtos:FullAuditedEntityDto<Guid>
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
<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/ProductCategory/Categorys/Category.cs


========
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Application.Contracts/CXS/DTOS/CategoryMangamentDto/CategoryDtos.cs
    }
}

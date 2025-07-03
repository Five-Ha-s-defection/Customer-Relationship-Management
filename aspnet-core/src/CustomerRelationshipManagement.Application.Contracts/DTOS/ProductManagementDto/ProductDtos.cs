using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.ProductManagementDto
{
    /// <summary>
    /// 产品管理DTOS
    /// </summary>
    public class ProductDtos:FullAuditedEntityDto<Guid>
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid? CategoryId { get; set; } 
        /// <summary>
        /// 父级分类ID (可为空，顶级分类没有父级)
        /// </summary>
        public Guid? ParentId { get; set; } 
        /// <summary>
        /// 产品图片
        /// </summary>
        public string? ProductImageUrl { get; set; } 
        /// <summary>
        /// 门幅
        /// </summary>
        public string? ProductBrand { get; set; } 
        /// <summary>
        //// 供应商
        /// </summary>
        public string? ProductSupplier { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string? ProductCode { get; set; } 

        /// <summary>
        /// 产品描述
        /// </summary>
        public string? ProductDescription { get; set; } 
        /// <summary>
        /// 建议售价
        /// </summary>
        public decimal? SuggestedPrice { get; set; } 
        /// <summary>
        /// 备注
        /// </summary>
        public string? ProductRemark { get; set; }
        /// <summary>
        /// 上架未上架
        /// </summary>
        public bool? ProductStatus { get; set; }
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal? DealPrice { get; set; } 


        /// <summary>
        /// 子产品集合（如有产品树结构）
        /// </summary>
        public List<ProductDtos> Children { get; set; } = new List<ProductDtos>();
    }
}

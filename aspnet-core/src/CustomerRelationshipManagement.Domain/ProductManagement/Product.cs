using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.ProductManagement
{
    /// <summary>
    /// 产品管理模块
    /// </summary>
    public class Product: FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 产品分类
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public string ProductImageUrl { get; set; }=string.Empty;
        /// <summary>
        /// 门幅
        /// </summary>
        public string ProductBrand { get; set; }= string.Empty;
        /// <summary>
        //// 供应商
        /// </summary>
        public string ProductSupplier { get; set; }= string.Empty;
        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;
 
        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProductDescription { get; set; } = string.Empty;
        /// <summary>
        /// 建议售价
        /// </summary>
        public decimal? SuggestedPrice { get; set; } = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public string ProductRemark { get; set; } = string.Empty;
        /// <summary>
        /// 上架未上架
        /// </summary>
        public bool ProductStatus { get; set; }= false;
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal? DealPrice { get; set; }=0;
    }
}

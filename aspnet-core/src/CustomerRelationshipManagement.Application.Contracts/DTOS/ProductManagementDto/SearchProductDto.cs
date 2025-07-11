﻿using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.ProductManagementDto
{
    public class SearchProductDto: PagingInfo
    {
        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid?  CategoryId { get; set; }=Guid.Empty;

        /// <summary>
        /// 分类名称名称
        /// </summary>
        public string? CategoryName { get; set; } 
        /// <summary>
        /// 父级分类ID (可为空，顶级分类没有父级)
        /// </summary>
        public Guid? ParentId { get; set; } = Guid.Empty;
        /// <summary>
        /// 产品图片
        /// </summary>
        public string? ProductImageUrl { get; set; } = string.Empty; 
        /// <summary>
        /// 门幅
        /// </summary>
        public string? ProductBrand { get; set; }= string.Empty;
        /// <summary>
        /// 供应商
        /// </summary>
        public string? ProductSupplier { get; set; }= string.Empty;
        /// <summary>
        /// 产品编号
        /// </summary>
        public string? ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 产品描述
        /// </summary>
        public string? ProductDescription { get; set; }= string.Empty;
        /// <summary>
        /// 建议售价
        /// </summary>
        public decimal? SuggestedPrice { get; set; } = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public string? ProductRemark { get; set; } = string.Empty;
        /// <summary>
        /// 上架未上架
        /// </summary>
        public bool ProductStatus { get; set; } 
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal? DealPrice { get; set; } = 0;


        public string? Keyword { get; set; }
    }
}

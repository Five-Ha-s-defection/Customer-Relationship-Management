﻿using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CategoryMangamentDto
{
    public class CategoryDtos:PagingInfo
    {
        public Guid Id { get; set; } = Guid.Empty;
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

        /// <summary>
        /// 子分类集合（树形结构的关键，递归嵌套）
        /// </summary>
        public List<CategoryDtos> Children { get; set; } = new List<CategoryDtos>();
    }
}

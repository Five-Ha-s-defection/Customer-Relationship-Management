﻿using CustomerRelationshipManagement.Clues;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace CustomerRelationshipManagement.Paging
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// 页索引
        /// </summary>
        [DefaultValue("1")]
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        [DefaultValue("10")]
        public int PageSize { get; set; }
    }

    /// <summary>
    ///  分页返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CacheName("PageInfo")]
    public class PageInfoCount<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> Data { get; set; }
    }
}

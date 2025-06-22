using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
    }
    /// <summary>
    ///  分页返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

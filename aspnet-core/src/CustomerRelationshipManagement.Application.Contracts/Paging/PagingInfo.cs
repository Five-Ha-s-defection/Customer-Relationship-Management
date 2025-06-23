using CustomerRelationshipManagement.Clues;
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
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keyword { get; set; }

        // 新增字段
        public Guid? CreatedBy { get; set; }         // 创建人
        public Guid? AssignedTo { get; set; }        // 负责人
        public ClueStatus? Status { get; set; }            // 状态（未跟进、跟进中、已转换）
        // 时间范围
        public DateTime? StartTime { get; set; }     // 时间范围起
        public DateTime? EndTime { get; set; }       // 时间范围止

        /// <summary>
        /// 时间筛选类型：CreateTime | NextContact | LastFollow（创建、下次联系、最近跟进）
        /// </summary>
        public TimeField? TimeType { get; set; }

        /// <summary>
        /// 排序字段：CreateTime | NextContact | LastFollow（创建、下次联系、最近跟进）
        /// </summary>
        public TimeField? OrderBy { get; set; }

        /// <summary>
        /// 是否倒序
        /// </summary>
        public bool OrderDesc { get; set; } = true;
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

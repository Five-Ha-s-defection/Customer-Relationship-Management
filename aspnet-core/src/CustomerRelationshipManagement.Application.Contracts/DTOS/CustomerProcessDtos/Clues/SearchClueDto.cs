using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues
{
    public class SearchClueDto:PagingInfo
    {
        /// <summary>
        /// 显示类型 0 显示所有 1 显示我负责的
        /// </summary>
        public int type { get; set; } = 0;
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


        /// <summary>
        /// 关键字(模糊查询)
        /// </summary>
        public string? Keyword { get; set; }

    }
}

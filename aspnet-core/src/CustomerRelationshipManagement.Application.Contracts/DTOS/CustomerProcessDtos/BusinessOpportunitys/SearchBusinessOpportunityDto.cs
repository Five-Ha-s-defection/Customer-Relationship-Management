using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys
{
    public class SearchBusinessOpportunityDto:PagingInfo
    {
        /// <summary>
        /// 显示类型 0 显示所有 1 显示我负责的
        /// </summary>
        public int type { get; set; } = 0;

        public Guid? AssignedTo { get; set; }        // 负责人

        /// <summary>
        /// 销售进度（可多选，存销售进度的枚举值或ID）
        /// </summary>
        public List<Guid> SalesProgressList { get; set; } = new List<Guid>();

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

        public string? Keyword { get; set; }

    }
}

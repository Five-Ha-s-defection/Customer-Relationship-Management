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
        public Guid? CreatedBy { get; set; }         // 创建人

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



        //=================高级筛选====================================
        /// <summary>
        /// 商机负责人（多选）
        /// </summary>
        public List<Guid>? UserIds { get; set; }

        /// <summary>
        /// 商机创建人（多选）
        /// </summary>
        public List<Guid>? CreatedByIds { get; set; }


        /// <summary>
        /// 所属客户
        /// </summary>
        public Guid? CustomerId { get; set; }= Guid.Empty;

        /// <summary>
        /// 商机编号
        /// </summary>
        public string? BusinessOpportunityCode { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public Guid? PriorityId { get; set; }= Guid.Empty;

        /// <summary>
        /// 商机名称
        /// </summary>
        public string? BusinessOpportunityName { get; set; }

        /// <summary>
        /// 销售进度
        /// </summary>
        public Guid? SalesProgressId { get; set; } = Guid.Empty;

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal? Budget { get; set; }

        /// <summary>
        /// 预计成交日期
        /// </summary>
        public DateTime? ExpectedDate { get; set; }

        /// <summary>
        /// 匹配模式：0=全部满足(AND)，1=部分满足(OR)（单选）
        /// </summary>
        public int MatchMode { get; set; } = 0;
    }
}

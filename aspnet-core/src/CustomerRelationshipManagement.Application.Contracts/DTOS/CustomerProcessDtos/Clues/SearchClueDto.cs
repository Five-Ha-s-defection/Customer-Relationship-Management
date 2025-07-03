using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<ClueStatus>? Status { get; set; }            // 状态（未跟进、跟进中、已转换）
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


        //===================================高级筛选============================================================
        /// <summary>
        /// 线索负责人（多选）
        /// </summary>
        public List<Guid>? UserIds { get; set; }

        /// <summary>
        /// 线索创建人（多选）
        /// </summary>
        public List<Guid>? CreatedByIds { get; set; }


        /// <summary>
        /// 线索编号
        /// </summary>
        public string? ClueCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string? ClueName { get; set; } 

        /// <summary>
        /// 电话
        /// </summary>
        public string? CluePhone { get; set; }

        /// <summary>
        /// 线索来源（单选）
        /// </summary>
        public Guid ClueSourceId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? ClueEmail { get; set; } 

        /// <summary>
        /// 微信号
        /// </summary>
        public string? ClueWechat { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string? ClueQQ { get; set; } 

        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; } 

        /// <summary>
        /// 行业（单选）
        /// </summary>
        public Guid IndustryId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; } 

        /// <summary>
        /// 匹配模式：0=全部满足(AND)，1=部分满足(OR)（单选）
        /// </summary>
        public int MatchMode { get; set; } = 0;
    }
}

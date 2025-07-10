using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers
{
    public class SearchCustomerDto: PagingInfo
    {
        /// <summary>
        /// 显示类型 0 显示所有 1 显示我负责的
        /// </summary>
        public int type { get; set; } = 0;

        public Guid? CreatedBy { get; set; }         // 创建人

        public Guid? AssignedTo { get; set; }        // 负责人

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
        /// 客户负责人（多选）
        /// </summary>
        public List<Guid>? UserIds { get; set; }

        /// <summary>
        /// 客户创建人（多选）
        /// </summary>
        public List<Guid>? CreatedByIds { get; set; }


        /// <summary>
        /// 客户编号（类似C-202506240038-3B7C形式）
        /// </summary>
        public string? CustomerCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? CustomerExpireTime { get; set; }

        /// <summary>
        /// 体检金额
        /// </summary>
        public decimal? CheckAmount { get; set; }

        /// <summary>
        /// 车架号（外键）
        /// </summary>
        public Guid? CarFrameNumberId { get; set; }= Guid.Empty;

        /// <summary>
        /// 客户级别（外键）
        /// </summary>
        public Guid? CustomerLevelId { get; set; } = Guid.Empty;

        /// <summary>
        /// 联系电话
        /// </summary>
        public string? CustomerPhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? CustomerEmail { get; set; }

        /// <summary>
        /// 客户类别（外键）
        /// </summary>
        public Guid? CustomerTypeId { get; set; } = Guid.Empty;

        /// <summary>
        ///  客户来源（外键）
        /// </summary>
        public Guid? CustomerSourceId { get; set; } = Guid.Empty;

        /// <summary>
        /// 客户地区（外键）
        /// </summary>
        public Guid? CustomerRegionId { get; set; } = Guid.Empty;

        /// <summary>
        /// 客户地址
        /// </summary>
        public string? CustomerAddress { get; set; }

        ///// <summary>
        ///// 联系人姓名
        ///// </summary>
        //public string? ContactName { get; set; }

        ///// <summary>
        ///// 手机
        ///// </summary>
        //public string? Mobile { get; set; }

        ///// <summary>
        ///// 邮箱
        ///// </summary>
        //public string? Email { get; set; }


        /// <summary>
        /// 匹配模式：0=全部满足(AND)，1=部分满足(OR)（单选）
        /// </summary>
        public int MatchMode { get; set; } = 0;
    }
}

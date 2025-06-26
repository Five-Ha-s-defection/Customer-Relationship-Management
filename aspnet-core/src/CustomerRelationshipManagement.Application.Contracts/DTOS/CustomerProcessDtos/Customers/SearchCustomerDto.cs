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

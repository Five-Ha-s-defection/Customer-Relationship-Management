using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts
{
    public class SearchCustomerContactDto:PagingInfo
    {
        /// <summary>
        /// 显示类型 0 显示所有 1 显示我负责的 2显示我创建的
        /// </summary>
        public int type { get; set; } = 0;

        public Guid? AssignedTo { get; set; }        // 负责人

        public DateTime? StartTime { get; set; }     // 时间范围起
        public DateTime? EndTime { get; set; }       // 时间范围止

        public string? Keyword { get; set; }

    }
}

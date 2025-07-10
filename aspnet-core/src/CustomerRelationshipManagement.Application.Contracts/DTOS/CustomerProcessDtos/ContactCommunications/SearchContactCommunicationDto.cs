using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.ContactCommunications;
using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactCommunications
{
    public class SearchContactCommunicationDto: PagingInfo
    {
        /// <summary>
        /// 显示类型 0 显示所有 1 显示我负责的
        /// </summary>
        public int type { get; set; } = 0;

        public Guid? AssignedTo { get; set; }        // 负责人

        public DateTime? StartTime { get; set; }     // 时间范围起（最后跟进时间 LastFollow）
        public DateTime? EndTime { get; set; }       // 时间范围止（最后跟进时间 LastFollow）

        /// <summary>
        /// 排序字段：CreateTime | NextContact（创建、下次联系）
        /// </summary>
        public TimeField? OrderBy { get; set; }

        /// <summary>
        /// 是否倒序
        /// </summary>
        public bool OrderDesc { get; set; } = true;

        /// <summary>
        /// 搜索关键字（联系内容）
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 联系对象类型（0=全部，1=客户，2=客户池，3=商机，4=线索，5=线索池）
        /// </summary>
        public ContactTargetType ContactTargetType { get; set; } = ContactTargetType.All;
    }
}

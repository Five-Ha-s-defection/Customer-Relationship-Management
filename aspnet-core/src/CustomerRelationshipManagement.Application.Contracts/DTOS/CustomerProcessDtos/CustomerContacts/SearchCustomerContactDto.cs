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

        public DateTime? StartTime { get; set; }     // 时间范围起
        public DateTime? EndTime { get; set; }       // 时间范围止

        /// <summary>
        /// 负责人
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }
        /// <summary>
        /// 所属客户ID（外键）
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string? ContactName { get; set; }

        /// <summary>
        /// 联系人关系（外键）
        /// </summary>
        public Guid? ContactRelationId { get; set; }

        /// <summary>
        /// 角色（外键）
        /// </summary>
        public Guid? RoleId { get; set; }

        /// <summary>
        /// 尊称（先生/女士）
        /// </summary>
        public bool? Salutation { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string? Mobile { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string? QQ { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string? Wechat { get; set; }


        public string? Keyword { get; set;}

    }
}

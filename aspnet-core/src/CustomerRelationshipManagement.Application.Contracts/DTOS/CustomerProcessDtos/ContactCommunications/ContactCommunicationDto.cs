using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactCommunications
{
    public class ContactCommunicationDto: FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; } 

        /// <summary>
        /// 选择客户（外键）
        /// </summary>
        public Guid? CustomerId { get; set; }

        //===========================================
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户负责人(外键)
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 客户负责人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 选择线索（外键）
        /// </summary>
        public Guid? ClueId { get; set; }

        //===========================================
        /// <summary>
        /// 姓名
        /// </summary>
        public string ClueName { get; set; }

        /// <summary>
        /// 选择商机（外键）
        /// </summary>
        public Guid? BusinessOpportunityId { get; set; }

        //===========================================
        /// <summary>
        /// 商机名称
        /// </summary>
        public string BusinessOpportunityName { get; set; }

        /// <summary>
        /// 上传附件
        /// </summary>
        public string? AttachmentUrl { get; set; }

        /// <summary>
        /// 沟通类型（外键）
        /// </summary>
        public Guid? ExpectedDateId { get; set; }

        //===========================================
        /// <summary>
        /// 中文类型名称
        /// </summary>
        public string CommunicationTypeName { get; set; }

        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime NextContactTime { get; set; }

        /// <summary>
        /// 跟进状态
        /// </summary>
        public int FollowUpStatus { get; set; } = 0;

        /// <summary>
        /// 评论
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// 联系对象类型及名称（只读属性，优先级：客户>线索>商机）
        /// </summary>
        public string ContactTargetTypeName
        {
            get
            {
                if (CustomerId != null && !string.IsNullOrEmpty(CustomerName))
                    return $"客户|{CustomerName}";
                if (ClueId != null && !string.IsNullOrEmpty(ClueName))
                    return $"线索|{ClueName}";
                if (BusinessOpportunityId != null && !string.IsNullOrEmpty(BusinessOpportunityName))
                    return $"商机|{BusinessOpportunityName}";
                return "未知";
            }
        }

        //===================新增字段=====================================
        /// <summary>
        /// 自定义回复（外键）
        /// </summary>
        public Guid CustomReplyId { get; set; }

        /// <summary>
        /// 中文内容
        /// </summary>
        public string CustomReplyName { get; set; }

        /// <summary>
        /// 保存为模版
        /// </summary>
        public bool IsServe { get; set; } = false;
    }
}

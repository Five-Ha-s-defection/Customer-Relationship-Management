using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/ProcessManegement/ContactCommunications/ContactCommunication.cs
namespace CustomerRelationshipManagement.CustomerProcess.ProcessManegement.ContactCommunications
========
namespace CustomerRelationshipManagement.CustomerProcess.ContactCommunications
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/ContactCommunications/ContactCommunication.cs
{
    /// <summary>
    /// 联系记录表
    /// </summary>
    public class ContactCommunication:FullAuditedAggregateRoot<Guid>

    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }=string.Empty;

        /// <summary>
        /// 选择客户（外键）
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 选择线索（外键）
        /// </summary>
        public Guid? ClueId { get; set; }

        /// <summary>
        /// 选择商机（外键）
        /// </summary>
        public Guid? BusinessOpportunityId { get; set; }

        /// <summary>
        /// 上传附件
        /// </summary>
        public string? AttachmentUrl { get; set; }

        /// <summary>
        /// 沟通类型（外键）
        /// </summary>
        public Guid? ExpectedDateId { get; set; }

        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime NextContactTime { get; set; }

        /// <summary>
        /// 跟进状态
        /// </summary>
        public int FollowUpStatus { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public string Comments { get; set; }

    }
}

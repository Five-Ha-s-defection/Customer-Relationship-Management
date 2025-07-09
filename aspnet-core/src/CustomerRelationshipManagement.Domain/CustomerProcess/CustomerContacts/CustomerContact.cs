using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;


namespace CustomerRelationshipManagement.CustomerProcess.CustomerContacts
{
    /// <summary>
    /// 联系人表
    /// </summary>
    public class CustomerContact: FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 所属客户ID（外键）
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人关系（外键）
        /// </summary>
        public Guid ContactRelationId { get; set; }

        /// <summary>
        /// 角色（外键）
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 尊称（先生/女士）
        /// </summary>
        public bool Salutation { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string Wechat { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        //=========================================================================
        /// <summary>
        /// 是否主联系人
        /// </summary>
        public bool IsPrimary { get; set; }

    }
}

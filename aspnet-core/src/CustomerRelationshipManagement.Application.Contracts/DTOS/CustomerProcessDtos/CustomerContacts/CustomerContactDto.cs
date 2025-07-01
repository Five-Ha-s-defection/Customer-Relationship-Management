using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts
{
    public class CustomerContactDto : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 所属客户ID（外键）
        /// </summary>
        public Guid CustomerId { get; set; }=Guid.Empty;

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }=string.Empty;

        /// <summary>
        /// 联系人关系（外键）
        /// </summary>
        public Guid ContactRelationId { get; set; } = Guid.Empty;

        /// <summary>
        /// 角色（外键）
        /// </summary>
        public Guid RoleId { get; set; } = Guid.Empty;

        /// <summary>
        /// 尊称（先生/女士）
        /// </summary>
        public bool Salutation { get; set; }= false;

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; } = string.Empty;

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 微信号
        /// </summary>
        public string Wechat { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

    }
}

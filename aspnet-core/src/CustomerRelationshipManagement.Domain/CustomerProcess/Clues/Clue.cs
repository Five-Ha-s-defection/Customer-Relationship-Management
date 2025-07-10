using CustomerRelationshipManagement.Clues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;


namespace CustomerRelationshipManagement.CustomerProcess.Clues
{
    /// <summary>
    /// 线索表
    /// </summary>
    public class Clue : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 线索负责人
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string ClueName { get; set; } = string.Empty;

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        public string CluePhone { get; set; } = string.Empty;

        /// <summary>
        /// 线索来源
        /// </summary>
        [Required]
        public Guid ClueSourceId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string ClueEmail { get; set; } = string.Empty;

        /// <summary>
        /// 微信号
        /// </summary>
        public string ClueWechat { get; set; } = string.Empty;

        /// <summary>
        /// QQ
        /// </summary>
        public string ClueQQ { get; set; } = string.Empty;

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// 行业
        /// </summary>
        public Guid IndustryId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 备注（富文本）
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 线索状态（枚举）
        /// </summary>
        public ClueStatus Status { get; set; }

        /// <summary>
        /// 最后跟进时间
        /// </summary>
        public DateTime LastFollowTime { get; set; }

        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime NextContactTime { get; set; }

        /// <summary>
        /// 线索编号
        /// </summary>
        public string ClueCode { get; set; }

        /// <summary>
        /// 线索分配/领取状态
        /// 0 未领取/未分配
        /// 1 已领取/已分配
        /// 2 已放弃
        /// </summary>
        public int CluePoolStatus { get; set; } = 0;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.ClientManagement.ClueManagement
{
    /// <summary>
    /// 线索
    /// </summary>
    public class Clue: FullAuditedAggregateRoot<Guid>
    {  /// <summary>
       /// 线索负责人
       /// </summary>
        [Required]
        public string Owner { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string ClueName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        public string CluePhone { get; set; }
        /// <summary>
        /// 线索来源
        /// </summary>
        [Required]
        public int ClueSourceId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string ClueEmail { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string ClueWechat { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string ClueQQ { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 行业
        /// </summary>
        public int IndustryId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }        
        /// <summary>
        /// 备注（富文本）
        /// </summary>
        public string Remark { get; set; }
    }
}

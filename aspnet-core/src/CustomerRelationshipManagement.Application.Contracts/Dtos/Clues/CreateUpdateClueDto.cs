using CustomerRelationshipManagement.Clues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Dtos.Clues
{
    public class CreateUpdateClueDto
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
        [StringLength(50)]
        public string ClueName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [StringLength(11)]
        public string CluePhone { get; set; }

        /// <summary>
        /// 线索来源
        /// </summary>
        [Required]
        public Guid ClueSourceId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ClueEmail { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        [Required,StringLength(50)]
        public string ClueWechat { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [Required,StringLength(50)]
        public string ClueQQ { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [Required,StringLength(50)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        [Required]
        public Guid IndustryId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required,StringLength(50)]
        public string Address { get; set; }

        /// <summary>
        /// 备注（富文本）
        /// </summary>
        [Required,StringLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 线索状态（枚举）
        /// </summary>
        [Required]
        public ClueStatus Status { get; set; }

        /// <summary>
        /// 最后跟进时间
        /// </summary>
        [Required]
        public DateTime LastFollowTime { get; set; }

        /// <summary>
        /// 下次联系时间
        /// </summary>
        [Required]
        public DateTime NextContactTime { get; set; }
    }
}

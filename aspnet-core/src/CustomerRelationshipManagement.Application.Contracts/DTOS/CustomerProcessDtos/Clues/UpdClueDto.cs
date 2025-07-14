using CustomerRelationshipManagement.Clues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues
{
    public class UpdClueDto
    {

        /// <summary>
        /// 姓名
        /// </summary>
        public string ClueName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string CluePhone { get; set; }

        /// <summary>
        /// 线索来源
        /// </summary>
        public Guid? ClueSourceId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? ClueEmail { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string? ClueWechat { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string? ClueQQ { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        public Guid? IndustryId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 备注（富文本）
        /// </summary>
        public string? Remark { get; set; }


        //=============================区分线索和线索池==================================================================
        /// <summary>
        /// 线索分配/领取状态
        /// 0 未领取/未分配
        /// 1 已领取/已分配
        /// 2 已放弃
        /// </summary>
        public int CluePoolStatus { get; set; } = 1;

    }
}

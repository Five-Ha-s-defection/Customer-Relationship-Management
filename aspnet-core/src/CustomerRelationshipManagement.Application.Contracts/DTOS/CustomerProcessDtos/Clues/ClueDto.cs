using CustomerRelationshipManagement.Clues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues
{
    [CacheName("ClueDtoCache")]
    public class ClueDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 线索负责人
        /// </summary>
        public Guid UserId { get; set; }

        //============================================================================
        /// <summary>  
        /// 用户名  
        /// </summary>  
        public string UserName { get; set; }

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
        public Guid ClueSourceId { get; set; }

        //===================================================================
        /// <summary>
        /// 线索来源名称
        /// </summary>
        public string ClueSourceName { get; set; } 

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
        public Guid IndustryId { get; set; }

        //=======================================================================
        /// <summary>
        /// 行业名称
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } 

        /// <summary>
        /// 备注（富文本）
        /// </summary>
        public string Remark { get; set; }

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
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;


namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers
{
    public class CustomerDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 客户负责人(外键)
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// 体检金额
        /// </summary>
        public decimal? CheckAmount { get; set; }

        /// <summary>
        /// 车架号（外键）
        /// </summary>
        public Guid CarFrameNumberId { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime CustomerExpireTime { get; set; }

        /// <summary>
        /// 客户级别（外键）
        /// </summary>
        public Guid CustomerLevelId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string CustomerPhone { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string CustomerEmail { get; set; } = string.Empty;

        /// <summary>
        /// 客户类别（外键）
        /// </summary>
        public Guid CustomerTypeId { get; set; }

        /// <summary>
        ///  客户来源（外键）
        /// </summary>
        public Guid CustomerSourceId { get; set; }

        /// <summary>
        /// 客户地区（外键）
        /// </summary>
        public Guid CustomerRegionId { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string? CustomerAddress { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string CustomerRemark { get; set; } = string.Empty;

        /// <summary>
        /// 线索Id（外键）
        /// </summary>
        public Guid ClueId { get; set; }

        //======================================================================================
        /// <summary>
        /// 微信号(线索外键)
        /// </summary>
        public string ClueWechat { get; set; }
        /// <summary>
        /// 最后跟进时间(线索外键)
        /// </summary>
        public DateTime? LastFollowTime { get; set; }

        /// <summary>
        /// 下次联系时间(线索外键)
        /// </summary>
        public DateTime? NextContactTime { get; set; }

        //======================================================================================
        /// <summary>  
        /// 创建人姓名（createId外键 连接 用户表Id）  
        /// </summary>  
        public string CreateName { get; set; } 
    }
}

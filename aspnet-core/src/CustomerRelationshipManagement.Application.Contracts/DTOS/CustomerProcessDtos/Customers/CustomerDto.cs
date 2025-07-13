using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts;
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

        //========================================================================================
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 体检金额
        /// </summary>
        public decimal? CheckAmount { get; set; }

        /// <summary>
        /// 车架号（外键）
        /// </summary>
        public Guid CarFrameNumberId { get; set; }

        //=========================================================================================
        /// <summary>
        /// 车架号名称
        /// </summary>
        public string CarFrameNumberName { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime CustomerExpireTime { get; set; }

        /// <summary>
        /// 客户级别（外键）
        /// </summary>
        public Guid CustomerLevelId { get; set; }

        //===========================================================================================
        /// <summary>
        /// 客户级别名称
        /// </summary>
        public string CustomerLevelName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string CustomerPhone { get; set; } 

        /// <summary>
        /// 邮箱
        /// </summary>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// 客户类别（外键）
        /// </summary>
        public Guid CustomerTypeId { get; set; }

        //===========================================================================================
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public string CustomerTypeName { get; set; }

        /// <summary>
        ///  客户来源（外键）
        /// </summary>
        public Guid CustomerSourceId { get; set; }

        //===========================================================================================
        /// <summary>
        /// 线索来源名称
        /// </summary>
        public string ClueSourceName { get; set; }

        /// <summary>
        /// 客户地区（外键）
        /// </summary>
        public Guid CustomerRegionId { get; set; }

        //===========================================================================================
        /// <summary>
        /// 客户地区名称
        /// </summary>
        public string CustomerRegionName { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string? CustomerAddress { get; set; } 

        /// <summary>
        /// 备注
        /// </summary>
        public string CustomerRemark { get; set; } 

        /// <summary>
        /// 线索Id（外键）
        /// </summary>
        public Guid ClueId { get; set; }

        /// <summary>
        /// 客户编号（类似C-202506240038-3B7C形式）
        /// </summary>
        public string CustomerCode { get; set; }

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

        //========================================================================================
        ///// <summary>
        ///// 所属客户ID（外键）
        ///// </summary>
        //public Guid CustomerId { get; set; }

        ///// <summary>
        ///// 联系人姓名
        ///// </summary>
        //public string ContactName { get; set; }

        ///// <summary>
        ///// 手机
        ///// </summary>
        //public string Mobile { get; set; }

        ///// <summary>
        ///// 邮箱
        ///// </summary>
        //public string Email { get; set; }

        //=============================区分客户和客户池==================================================================
        /// <summary>
        /// 客户分配/领取状态
        /// 0 未领取/未分配
        /// 1 已领取/已分配
        /// 2 已放弃
        /// </summary>
        public int CustomerPoolStatus { get; set; } = 0;
    }
}

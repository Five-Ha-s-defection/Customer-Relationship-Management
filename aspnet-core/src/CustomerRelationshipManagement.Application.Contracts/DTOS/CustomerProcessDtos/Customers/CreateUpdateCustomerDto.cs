using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers
{
    public class CreateUpdateCustomerDto
    {
        /// <summary>
        /// 客户负责人(外键)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [StringLength(50)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 体检金额
        /// </summary>
        public decimal? CheckAmount { get; set; }

        /// <summary>
        /// 车架号（外键）
        /// </summary>
        public Guid? CarFrameNumberId { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? CustomerExpireTime { get; set; }

        /// <summary>
        /// 客户级别（外键）
        /// </summary>
        public Guid? CustomerLevelId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(11)]
        public string CustomerPhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(50)]
        public string? CustomerEmail { get; set; } = string.Empty;

        /// <summary>
        /// 客户类别（外键）
        /// </summary>
        public Guid? CustomerTypeId { get; set; }

        /// <summary>
        ///  客户来源（外键）
        /// </summary>
        public Guid? CustomerSourceId { get; set; }

        /// <summary>
        /// 客户地区（外键）
        /// </summary>
        public Guid? CustomerRegionId { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        [StringLength(100)]
        public string? CustomerAddress { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string? CustomerRemark { get; set; } = string.Empty;

        /// <summary>
        /// 线索Id（外键）
        /// </summary>
        public Guid? ClueId { get; set; }= Guid.Empty;

        /// <summary>
        /// 客户编号（类似C-202506240038-3B7C形式）
        /// </summary>
        public string CustomerCode { get; set; }
    }
}

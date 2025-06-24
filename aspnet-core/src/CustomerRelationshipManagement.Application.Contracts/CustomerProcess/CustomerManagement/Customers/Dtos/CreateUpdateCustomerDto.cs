using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Customers.Dtos
{
    public class CreateUpdateCustomerDto
    {
        /// <summary>
        /// 客户负责人(外键)
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 体检金额
        /// </summary>
        [Required]
        public decimal? CheckAmount { get; set; }

        /// <summary>
        /// 车架号（外键）
        /// </summary>
        [Required]
        public Guid CarFrameNumberId { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        [Required]
        public DateTime CustomerExpireTime { get; set; }

        /// <summary>
        /// 客户级别（外键）
        /// </summary>
        [Required]
        public Guid CustomerLevelId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Required]
        [StringLength(11)]
        public string CustomerPhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CustomerEmail { get; set; } = string.Empty;

        /// <summary>
        /// 客户类别（外键）
        /// </summary>
        [Required]
        public Guid CustomerTypeId { get; set; }

        /// <summary>
        ///  客户来源（外键）
        /// </summary>
        [Required]
        public Guid CustomerSourceId { get; set; }

        /// <summary>
        /// 客户地区（外键）
        /// </summary>
        [Required]
        public Guid CustomerRegionId { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? CustomerAddress { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [StringLength(200)]
        public string CustomerRemark { get; set; } = string.Empty;

        /// <summary>
        /// 线索Id（外键）
        /// </summary>
        [Required]
        public Guid ClueId { get; set; }
    }
}

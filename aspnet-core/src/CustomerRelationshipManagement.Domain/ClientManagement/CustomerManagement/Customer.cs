using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.ClientManagement.CustomerManagement
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Customer: FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 客户负责人
        /// </summary>
        public string? CustomerOwner { get; set; }      
        /// <summary>
        /// 客户名称
        /// </summary>
        public string? CustomerName { get; set; }       
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? CustomerDate { get; set; }            
        /// <summary>
        /// 体检金额
        /// </summary>
        public decimal? CheckAmount { get; set; }      
        /// <summary>
        /// 车架号
        /// </summary>
        public string? CarFrameNumber { get; set; }     
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? CustomerExpireTime { get; set; }      
        /// <summary>
        ///   客户级别
        /// </summary>
        public string? CustomerLevel { get; set; }      
        /// <summary>
        /// 联系电话
        /// </summary>
        public string? CustomerPhone { get; set; }              
        /// <summary>
        /// 邮箱
        /// </summary>

        public string? CustomerEmail { get; set; }              
        /// <summary>
        /// 客户类别
        /// </summary>

        public string? CustomerType { get; set; }       
        /// <summary>
        /// 客户类别X
        /// </summary>

        public string? CustomerTypeX { get; set; }     
        /// <summary>
        /// 客户来源X
        /// </summary>

        public int CustomerSourceId { get; set; }    
        /// <summary>
        /// 客户地区X
        /// </summary>

        public int CustomerRegionId { get; set; }    
        /// <summary>
        /// 客户地址X
        /// </summary>

        public string? CustomerAddress { get; set; }    
        /// <summary>
        /// 备注
        /// </summary>

        public string? CustomerRemark { get; set; }            
        /// <summary>
        /// 线索Id
        /// </summary>

        public int ClueId { get; set; }            
    }
}

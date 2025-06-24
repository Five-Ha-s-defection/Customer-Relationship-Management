using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.CustomerRegions
{
    /// <summary>
    /// 客户地区表
    /// </summary>
    public class CustomerRegion:Entity<Guid>
    {
        /// <summary>
        /// 客户地区名称
        /// </summary>
        public string CustomerRegionName { get; set; }
        /// <summary>
        /// 客户地区父级ID
        /// </summary>
        public Guid ParentId { get; set; }
    }
}

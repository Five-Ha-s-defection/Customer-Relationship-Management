using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.ClientManagement.CustomerManagement
{
    /// <summary>
    /// 客户地区(省市级联操作)
    /// </summary>
    public class CustomArea: FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 地区名称
        /// </summary>
        public string? CustomAreaName { get; set; }      
        /// <summary>
        /// 父级Id
        /// </summary>
        public int ParentId { get; set; }        
    }
}

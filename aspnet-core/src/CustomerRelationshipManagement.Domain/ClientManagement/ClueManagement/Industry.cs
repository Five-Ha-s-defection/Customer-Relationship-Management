using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.ClientManagement.ClueManagement
{
    /// <summary>
    /// 行业
    /// </summary>
    public class Industry: FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 行业名称
        /// </summary>
        public string? IndustryName { get; set; }      
    }
}

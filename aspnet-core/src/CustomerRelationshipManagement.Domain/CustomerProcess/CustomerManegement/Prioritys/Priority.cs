using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Prioritys
{
    /// <summary>
    /// 优先级表
    /// </summary>
    public class Priority:Entity<Guid>
    {
        /// <summary>
        /// 优先级名称
        /// </summary>
        public string PriorityName { get; set; }
    }
}

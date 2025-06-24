using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerLevels
{
    /// <summary>
    /// 客户级别表
    /// </summary>
    public class CustomerLevel:Entity<Guid>
    {
        /// <summary>
        /// 客户级别名称
        /// </summary>
        public string CustomerLevelName { get; set; }
    }
}

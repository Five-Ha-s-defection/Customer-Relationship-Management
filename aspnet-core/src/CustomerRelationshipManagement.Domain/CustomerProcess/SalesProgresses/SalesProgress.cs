using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerProcess.SalesProgresses
{
    /// <summary>
    /// 销售进度表
    /// </summary>
    public class SalesProgress:Entity<Guid>
    {
        /// <summary>
        /// 销售进度名称
        /// </summary>
        public string SalesProgressName { get; set; }
    }
}

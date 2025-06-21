using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.ProductManagement
{
    /// <summary>
    /// 产品分类
    /// </summary>
    public class Category : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 产品分类名称
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
    }
}

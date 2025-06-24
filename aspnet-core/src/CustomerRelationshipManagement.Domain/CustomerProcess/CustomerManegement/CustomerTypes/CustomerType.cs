using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.CustomerTypes
{
    /// <summary>
    /// 客户类型表
    /// </summary>
    public class CustomerType:Entity<Guid>
    {
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public string CustomerTypeName { get; set; }
    }
}

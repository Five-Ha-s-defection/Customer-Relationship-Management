using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/CustomerLevels/CustomerLevel.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.CustomerLevels
========
namespace CustomerRelationshipManagement.CustomerProcess.CustomerLevels
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerLevels/CustomerLevel.cs
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

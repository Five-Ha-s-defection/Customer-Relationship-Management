using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/CustomerRegions/CustomerRegion.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.CustomerRegions
========
namespace CustomerRelationshipManagement.CustomerProcess.CustomerRegions
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerRegions/CustomerRegion.cs
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

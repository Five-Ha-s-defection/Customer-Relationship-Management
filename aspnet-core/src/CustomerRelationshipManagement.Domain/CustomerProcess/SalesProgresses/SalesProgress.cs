using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/SalesProgresses/SalesProgress.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.SalesProgresses
========
namespace CustomerRelationshipManagement.CustomerProcess.SalesProgresses
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/SalesProgresses/SalesProgress.cs
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

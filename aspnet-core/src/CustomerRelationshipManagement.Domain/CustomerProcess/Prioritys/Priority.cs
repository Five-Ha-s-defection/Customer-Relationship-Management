using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/Prioritys/Priority.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Prioritys
========
namespace CustomerRelationshipManagement.CustomerProcess.Prioritys
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/Prioritys/Priority.cs
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

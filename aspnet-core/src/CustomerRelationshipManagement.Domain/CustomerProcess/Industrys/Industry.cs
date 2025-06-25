using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/Industrys/Industry.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Industrys
========
namespace CustomerRelationshipManagement.CustomerProcess.Industrys
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/Industrys/Industry.cs
{
    /// <summary>
    /// 行业表
    /// </summary>
    public class Industry:Entity<Guid>
    {
        /// <summary>
        /// 行业名称
        /// </summary>
        public string IndustryName { get; set; }      
    }
}

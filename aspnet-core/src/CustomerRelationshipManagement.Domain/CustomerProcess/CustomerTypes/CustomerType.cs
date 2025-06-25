using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/CustomerTypes/CustomerType.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.CustomerTypes
========
namespace CustomerRelationshipManagement.CustomerProcess.CustomerTypes
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerTypes/CustomerType.cs
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

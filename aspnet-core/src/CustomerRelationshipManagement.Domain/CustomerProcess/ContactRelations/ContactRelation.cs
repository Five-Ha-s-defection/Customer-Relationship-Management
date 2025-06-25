using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/ContactRelations/ContactRelation.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.ContactRelations
========
namespace CustomerRelationshipManagement.CustomerProcess.ContactRelations
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/ContactRelations/ContactRelation.cs
{
    /// <summary>
    /// 联系人关系表
    /// </summary>
    public class ContactRelation:Entity<Guid>
    {
        /// <summary>
        /// 联系人关系名称
        /// </summary>
        public string ContactRelationName { get; set; }
    }
}

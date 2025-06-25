using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerProcess.ContactRelations
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

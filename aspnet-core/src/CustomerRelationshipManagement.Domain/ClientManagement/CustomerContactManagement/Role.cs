using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.ClientManagement.CustomerContactManagement
{
    /// <summary>
    /// 联系人角色
    /// </summary>
    public class Role: FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 角色名称 
        /// </summary>
        public string? RoleName { get; set; }      
    }
}

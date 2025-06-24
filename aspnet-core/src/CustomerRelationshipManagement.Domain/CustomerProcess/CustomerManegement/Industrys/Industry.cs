using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Industrys
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.Cards
{
    /// <summary>
    /// 车架号表
    /// </summary>
    public class CarFrameNumber:Entity<Guid>
    {
        /// <summary>
        /// 车架号名称
        /// </summary>
        public string CarFrameNumberName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Regions
{
    public class CreateUpdateRegionDto
    {
        /// <summary>
        /// 客户地区名称
        /// </summary>
        public string CustomerRegionName { get; set; }
        /// <summary>
        /// 客户地区父级ID
        /// </summary>
        public Guid ParentId { get; set; } = Guid.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions
{
    public class RegionDto
    {
        /// <summary>
        /// 客户地区ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 客户地区名称
        /// </summary>
        public string CustomerRegionName { get; set; }
        /// <summary>
        /// 客户地区父级ID
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 子地区
        /// </summary>
        public List<RegionDto> Children { get; set; } = new List<RegionDto>();
    }
}

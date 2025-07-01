using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions
{
    public class CustomerRegionDto : FullAuditedEntityDto<Guid>
    {
         /// <summary>
        /// 客户地区名称
        /// </summary>
        public string CustomerRegionName { get; set; }
        /// <summary>
        /// 客户地区父级ID
        /// </summary>
        public Guid ParentId { get; set; }
    }
}

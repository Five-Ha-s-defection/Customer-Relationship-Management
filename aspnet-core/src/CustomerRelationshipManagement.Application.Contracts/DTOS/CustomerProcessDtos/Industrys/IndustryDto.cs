using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Industrys
{
    public class IndustryDto : EntityDto<Guid>
    {
        /// <summary>
        /// 行业名称
        /// </summary>
        public string IndustryName { get; set; }
    }
}

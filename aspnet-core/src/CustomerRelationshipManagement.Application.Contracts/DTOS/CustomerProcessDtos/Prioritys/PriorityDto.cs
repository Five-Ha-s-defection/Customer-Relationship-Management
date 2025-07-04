using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Prioritys
{
    public class PriorityDto:EntityDto<Guid>
    {
        /// <summary>
        /// 优先级名称
        /// </summary>
        public string PriorityName { get; set; }
    }
}

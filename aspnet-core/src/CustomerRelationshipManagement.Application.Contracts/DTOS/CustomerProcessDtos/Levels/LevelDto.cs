using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Levels
{
    public class LevelDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 客户级别名称
        /// </summary>
        public string CustomerLevelName { get; set; }
    }
}

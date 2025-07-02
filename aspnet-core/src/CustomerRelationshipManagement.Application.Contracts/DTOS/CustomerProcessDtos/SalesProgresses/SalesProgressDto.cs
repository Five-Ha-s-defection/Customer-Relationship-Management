using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.SalesProgresses
{
    public class SalesProgressDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 销售进度名称
        /// </summary>
        public string SalesProgressName { get; set; }
    }
}

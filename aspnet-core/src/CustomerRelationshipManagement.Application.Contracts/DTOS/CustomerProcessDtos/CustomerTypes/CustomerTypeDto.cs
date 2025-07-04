using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes
{
    public class CustomerTypeDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public string CustomerTypeName { get; set; }
    }
}

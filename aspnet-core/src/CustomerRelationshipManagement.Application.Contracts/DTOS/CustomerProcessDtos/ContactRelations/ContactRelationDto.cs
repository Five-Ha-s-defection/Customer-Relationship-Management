using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactRelations
{
    public class ContactRelationDto:EntityDto<Guid>
    {
        /// <summary>
        /// 联系人关系名称
        /// </summary>
        public string ContactRelationName { get; set; }
    }
}

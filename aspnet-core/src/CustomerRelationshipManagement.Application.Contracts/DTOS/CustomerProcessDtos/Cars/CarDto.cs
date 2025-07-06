using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Cars
{
    public class CarDto : EntityDto<Guid>
    {
        /// <summary>
        /// 车架号名称
        /// </summary>
        public string CarFrameNumberName { get; set; }
    }
}

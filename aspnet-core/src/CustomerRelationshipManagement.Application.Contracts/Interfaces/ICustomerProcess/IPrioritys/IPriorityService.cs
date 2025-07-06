using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Prioritys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IPrioritys
{
    public interface IPriorityService:IApplicationService
    {
        /// <summary>
        /// 创建商机的优先级
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<PriorityDto>> AddPriority(CreateUpdatePriorityDto dto);
    }
}

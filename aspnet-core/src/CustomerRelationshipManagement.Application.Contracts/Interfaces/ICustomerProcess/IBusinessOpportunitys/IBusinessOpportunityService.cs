using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IBusinessOpportunitys
{
    public interface IBusinessOpportunityService:IApplicationService
    {
        /// <summary>
        /// 添加商机
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<BusinessOpportunityDto>> AddBusinessOpportunity(CreateUpdateBusinessOpportunityDto dto);
    }
}

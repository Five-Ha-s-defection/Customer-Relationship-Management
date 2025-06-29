using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Industrys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IIndustrys
{
    public interface IIndustryService:IApplicationService
    {
        /// <summary>
        /// 新增行业
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<IndustryDto>> AddIndustry(CreateUpdateIndustryDto dto);
    }
}

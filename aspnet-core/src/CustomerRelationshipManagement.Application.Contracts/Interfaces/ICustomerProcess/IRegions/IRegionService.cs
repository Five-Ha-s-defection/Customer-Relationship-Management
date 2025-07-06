using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IRegions
{
    public interface IRegionService:IApplicationService
    {
        /// <summary>
        /// 添加客户地区
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<RegionDto>> AddRegion(CreateUpdateRegionDto dto);
    }
}

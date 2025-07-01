using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.SalesProgresses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.ISalesProgresses
{
    public interface ISalesProgressService:IApplicationService
    {
        /// <summary>
        /// 添加销售进度
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<SalesProgressDto>> AddSalesProgress(CreateUpdateSalesProgressDto dto);
    }
}

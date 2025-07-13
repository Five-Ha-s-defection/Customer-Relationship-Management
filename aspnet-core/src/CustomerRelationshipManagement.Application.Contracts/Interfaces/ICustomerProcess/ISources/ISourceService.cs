using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.ISources
{
    public interface ISourceService:IApplicationService
    {
        /// <summary>
        /// 添加线索/客户来源
        /// </summary>
        /// <param name="sourceDto"></param>
        /// <returns></returns>
        Task<ApiResult<SourceDto>> AddSource(CreateUpdateSourceDto sourceDto);
    }
}

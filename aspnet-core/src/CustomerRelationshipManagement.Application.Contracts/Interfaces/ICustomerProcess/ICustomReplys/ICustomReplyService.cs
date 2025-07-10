using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomReplys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomReplys
{
    public interface ICustomReplyService:IApplicationService
    {
        /// <summary>
        /// 添加自定义回复
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<CustomReplyDto>> AddCustomerType(CreateUpdateCustomReplyDto dto);
    }
}

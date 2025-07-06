using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomerTypes
{
    public interface ICustomerTypeService:IApplicationService
    {
        /// <summary>
        /// 添加客户类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<CustomerTypeDto>> AddCustomerType(CreateUpdateCustomerTypeDto dto);
    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactRelations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IContactRelations
{
    public interface IContactRelationService:IApplicationService
    {
        /// <summary>
        /// 添加联系人关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<ContactRelationDto>> AddContactRelation(CreateUpdateContactRelationsDto dto);
    }
}

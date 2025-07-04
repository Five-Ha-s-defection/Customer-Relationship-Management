using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactCommunications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IContactCommunications
{
    public interface IContactCommunicationService:IApplicationService
    {
        /// <summary>
        /// 添加联系沟通
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<ContactCommunicationDto>> AddContactCommunication(CreateUpdateContactCommunicationDto dto);
    }
}

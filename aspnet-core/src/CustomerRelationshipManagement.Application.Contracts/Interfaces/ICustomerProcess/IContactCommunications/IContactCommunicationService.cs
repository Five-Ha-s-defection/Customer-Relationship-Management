using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CommunicationTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactCommunications;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomReplys;
using CustomerRelationshipManagement.Paging;
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

        /// <summary>
        /// 显示联系沟通列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<PageInfoCount<ContactCommunicationDto>>> GetContactCommunicationList([FromQuery] SearchContactCommunicationDto dto);

        /// <summary>
        /// 无查询条件、无分页，显示所有联系沟通列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<ContactCommunicationDto>>> GetAllContactCommunications(Guid id, int targetType);

        /// <summary>
        /// 获取联系沟通详情信息
        /// </summary>
        /// <param name="id">要查询的联系沟通ID</param>
        /// <returns></returns>
        /// 
        Task<ApiResult<ContactCommunicationDto>> GetcontactCommunicationById(Guid id);

        /// <summary>
        /// 删除联系沟通信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<ContactCommunicationDto>> DelcontactCommunication(Guid id);

        /// <summary>
        /// 修改联系沟通信息
        /// </summary>
        /// <param name="id">要修改的联系沟通ID</param>
        /// <param name="dto">联系沟通信息</param>
        /// <returns></returns>
        /// 
        Task<ApiResult<CreateUpdateContactCommunicationDto>> UpdcontactCommunication(Guid id, CreateUpdateContactCommunicationDto dto);

        /// <summary>
        /// 获取沟通类型下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<CommunicationTypeDto>>> GetCommunicationTypeSelectList();

        /// <summary>
        /// 获取自定义回复下拉框数据(根据沟通类型ID获取自定义回复数据)
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<CustomReplyDto>>> GetCustomReplyByType(Guid typeId);
    }
}

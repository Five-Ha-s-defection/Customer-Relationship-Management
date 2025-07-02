using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.ContactRelations;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBACDtos.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomerContacts
{
    public interface ICustomerContactService : IApplicationService
    {
        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="dto">要添加的联系人信息</param>
        /// <returns></returns>
        Task<ApiResult<CustomerContactDto>> AddCustomerContact(CreateUpdateCustomerContactDto dto);

        /// <summary>
        /// 显示联系人信息
        /// </summary>
        /// <param name="dto">要查询的信息</param>
        /// <returns></returns>
        Task<ApiResult<PageInfoCount<CustomerContactDto>>> ShowCustomerContact([FromQuery] SearchCustomerContactDto dto);

        /// <summary>
        /// 获取联系人详情信息
        /// </summary>
        /// <param name="id">要查询的联系人ID</param>
        /// <returns></returns>
        Task<ApiResult<CustomerContactDto>> GetCustomerContactById(Guid id);

        /// <summary>
        /// 删除联系人信息 
        /// </summary>
        /// <param name="id">要删除的联系人ID</param>
        /// <returns></returns>
        Task<ApiResult<CustomerContactDto>> DelCustomerContact(Guid id);

        /// <summary>
        /// 修改联系人信息 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<CreateUpdateCustomerContactDto>> UpdCustomerContact(Guid id, CreateUpdateCustomerContactDto dto);

        /// <summary>
        /// 获取客户下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<CustomerDto>>> GetCustomerList();

        /// <summary>
        /// 获取联系人关系下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<CustomerRegionDto>>> GetContactRelationList();

        /// <summary>
        /// 获取角色下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<RoleDto>>> GetRoleDtoList();


    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Cars;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Levels;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomers
{
    public interface ICustomerService:IApplicationService
    {
        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="dto">要添加的客户信息</param>
        /// <returns></returns>
        Task<ApiResult<CustomerDto>> AddCustomer(CreateUpdateCustomerDto dto);

        /// <summary>
        /// 显示客户信息
        /// </summary>
        /// <param name="dto">要查询的信息</param>
        /// <returns></returns>
        Task<ApiResult<PageInfoCount<CustomerDto>>> ShowCustomer([FromQuery] SearchCustomerDto dto);

        /// <summary>
        /// 获取客户详情信息
        /// </summary>
        /// <param name="id">要查询的客户ID</param>
        /// <returns></returns>
        Task<ApiResult<CustomerDto>> GetCustomerById(Guid id);

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="id">要删除的客户ID</param>
        /// <returns></returns>
        Task<ApiResult<CustomerDto>> DelCustomer(Guid id);

        /// <summary>
        /// 修改客户信息
        /// </summary>
        /// <param name="id">要修改的客户ID</param>
        /// <param name="dto">客户信息</param>
        /// <returns></returns>
        Task<ApiResult<CreateUpdateCustomerDto>> UpdCustomer(Guid id, CreateUpdateCustomerDto dto);

        /// <summary>
        /// 获取用户下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<UserInfoDto>>> GetUserSelectList();

        /// <summary>
        /// 获取车架号下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<CarDto>>> GetCarSelectList();

        /// <summary>
        /// 获取客户级别下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<LevelDto>>> GetLevelSelectList();

        /// <summary>
        /// 获取来源下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SourceDto>>> GetSourceSelectList();


        /// <summary>
        /// 获取客户地区下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<RegionDto>>> GetRegionCascadeList();

        /// <summary>
        /// 获取客户类别下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<CustomerTypeDto>>> GetCustomerType();
    }
}

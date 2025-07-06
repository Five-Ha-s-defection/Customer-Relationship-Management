using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Prioritys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.SalesProgresses;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.ICustomerProcess.IBusinessOpportunitys
{
    public interface IBusinessOpportunityService:IApplicationService
    {
        /// <summary>
        /// 添加商机
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<BusinessOpportunityDto>> AddBusinessOpportunity(CreateUpdateBusinessOpportunityDto dto);

        /// <summary>
        /// 获取客户下拉列表
        /// </summary>
        /// <returns></returns>
       Task<ApiResult<List<CustomerSimpleDto>>> GetCustomerSelectList();

        /// <summary>
        /// 获取商机优先级下拉列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<PriorityDto>>> GetPrioritySelectList();

        /// <summary>
        /// 获取商机销售进度下拉列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<SalesProgressDto>>> GetSalesProgressSelectList();

        /// <summary>
        /// 获取产品下拉列表
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<ProductDtos>>> GetProductSelectList();

        /// <summary>
        /// 显示商机列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ApiResult<PageInfoCount<BusinessOpportunityDto>>> ShowBusinessOpportunity([FromQuery] SearchBusinessOpportunityDto dto);

        /// <summary>
        /// 删除商机列表
        /// </summary>
        /// <typeparam name="BusinessOpportunity"></typeparam>
        /// <returns></returns>
        Task<ApiResult<BusinessOpportunityDto>> DelBusinessOpportunity(Guid id);
    }
}

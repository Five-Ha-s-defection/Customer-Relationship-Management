using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Customers.Dtos;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Customers
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
        /// <param name="dto">要查询的条件</param>
        /// <returns></returns>
        Task<ApiResult<PageInfoCount<CustomerDto>>> ShowCustomer([FromQuery] SearchCustomerDto dto);
    }
}

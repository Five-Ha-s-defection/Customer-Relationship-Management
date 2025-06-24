using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Dtos.Customers;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Customers
{
    public interface ICustomerService:IApplicationService
    {
        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="dto">要添加的客户信息</param>
        /// <returns></returns>
        Task<ApiResult<CustomerDto>> AddCustomer(CreateUpdateCustomerDto dto);
    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Dtos.Customers;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Customers
{
    public class CustomerService:ApplicationService,ICustomerService
    {
        private readonly IRepository<Customer> repository;
        private readonly ILogger<CustomerService> logger;
        private readonly IDistributedCache<PageInfoCount<CustomerDto>> cache;
        public CustomerService(IRepository<Customer> repository, ILogger<CustomerService> logger, IDistributedCache<PageInfoCount<CustomerDto>> cache)
        {
            this.repository = repository;
            this.logger = logger;
            this.cache = cache;
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="dto">要添加的客户信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CustomerDto>> AddCustomer(CreateUpdateCustomerDto dto)
        {
            try
            {
                var customer=ObjectMapper.Map<CreateUpdateCustomerDto, Customer>(dto);
                var list = await repository.InsertAsync(customer);
                return ApiResult<CustomerDto>.Success(ResultCode.Success, ObjectMapper.Map<Customer, CustomerDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError("添加客户信息出错！" + ex.Message);
                throw;
            }
        }
    }
}

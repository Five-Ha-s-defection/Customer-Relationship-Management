using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Customers.Dtos;
using CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Customers;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Customers
{
    [ApiExplorerSettings(GroupName = "v1")]
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

        /// <summary>
        /// 显示客户信息
        /// </summary>
        /// <param name="dto">要查询的条件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<CustomerDto>>> ShowCustomer([FromQuery] SearchCustomerDto dto)
        {
            try
            {
                string cachKey = "CustomerRedis";
                var redislist = await cache.GetOrAddAsync(cachKey, async () =>
                {
                    var customerlist = await repository.GetQueryableAsync();
                    var res = customerlist.PageResult(dto.PageIndex,dto.PageSize);
                    var customerDtos = ObjectMapper.Map<List<Customer>, List<CustomerDto>>(res.Queryable.ToList());
                    var pageinfo = new PageInfoCount<CustomerDto>
                    {
                        TotalCount = res.RowCount,
                        PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                        Data = customerDtos
                    };
                    return pageinfo;
                },()=>new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                });
                return ApiResult<PageInfoCount<CustomerDto>>.Success(ResultCode.Success, redislist);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

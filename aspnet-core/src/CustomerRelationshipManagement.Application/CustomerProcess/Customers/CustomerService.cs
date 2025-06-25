using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomers;
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
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CustomerProcess.Customers
{
    public class CustomerService:ApplicationService,ICustomerService
    {
        private readonly IRepository<Customer> repository;
        private readonly IRepository<Clue> clueRepository;
        private readonly ILogger<CustomerService> logger;
        private readonly IDistributedCache<PageInfoCount<CustomerDto>> cache;
        public CustomerService(IRepository<Customer> repository, ILogger<CustomerService> logger, IDistributedCache<PageInfoCount<CustomerDto>> cache, IRepository<Clue> clueRepository)
        {
            this.repository = repository;
            this.logger = logger;
            this.cache = cache;
            this.clueRepository = clueRepository;
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
        /// <param name="dto">要查询的信息</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<CustomerDto>>> ShowCustomer([FromQuery] SearchCustomerDto dto)
        {
            try
            {
                ////构建缓存键名
                //string cacheKey = "CustomerRedis";
                ////使用Redis缓存获取或添加数据
                //var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
                //{
                    var customerlist = await repository.GetQueryableAsync();
                    var cluelist = await clueRepository.GetQueryableAsync();
                    var list=from cus in customerlist
                             join clu in cluelist 
                             on cus.ClueId equals clu.Id
                             select new CustomerWithClueDto
                             {
                                 Customer=cus,
                                 ClueWechat=clu.ClueWechat,
                             };
                    //查询条件
                    //根据客户姓名、（联系人）、电话、邮箱、（微信号）模糊查询
                    if (!string.IsNullOrEmpty(dto.Keyword))
                    {
                        list = list.Where(x => x.Customer.CustomerName.Contains(dto.Keyword)
                                               || x.Customer.CustomerPhone.Contains(dto.Keyword)
                                               || x.ClueWechat.Contains(dto.Keyword)
                                               || x.Customer.CustomerEmail.Contains(dto.Keyword));
                    }
                    // 时间筛选
                    if (dto.StartTime.HasValue && dto.EndTime.HasValue && dto.TimeType.HasValue)
                    {
                        list= dto.TimeType switch
                        {
                            TimeField.CreateTime => list.Where(x => x.Customer.CreationTime >= dto.StartTime && x.Customer.CreationTime <= dto.EndTime),
                            TimeField.NextContact => list.Where(x => x.NextContactTime >= dto.StartTime && x.NextContactTime <= dto.EndTime),
                            TimeField.LastFollow => list.Where(x => x.LastFollowTime >= dto.StartTime && x.LastFollowTime <= dto.EndTime),
                            _ => list
                        };
                    }

                    // 排序
                    if (dto.OrderBy.HasValue)
                    {
                        list = (dto.OrderBy.Value, dto.OrderDesc) switch
                        {
                            (TimeField.CreateTime, true) => list.OrderByDescending(x => x.Customer.CreationTime),
                            (TimeField.CreateTime, false) => list.OrderBy(x => x.Customer.CreationTime),

                            (TimeField.NextContact, true) => list.OrderByDescending(x => x.NextContactTime),
                            (TimeField.NextContact, false) => list.OrderBy(x => x.NextContactTime),

                            (TimeField.LastFollow, true) => list.OrderByDescending(x => x.LastFollowTime),
                            (TimeField.LastFollow, false) => list.OrderBy(x => x.LastFollowTime),

                            _ => list.OrderByDescending(x => x.LastFollowTime)
                        };
                    }

                    //用ABP框架的分页
                    var res = list.PageResult(dto.PageIndex, dto.PageSize);
                    //实体列表转换成DTO列表
                    var customerDtos = ObjectMapper.Map<List<CustomerWithClueDto>, List<CustomerDto>>(res.Queryable.ToList());
                    //构建分页结果对象
                    var pageInfo = new PageInfoCount<CustomerDto>
                    {
                        TotalCount = res.RowCount,
                        PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                        Data = customerDtos
                    };
                //    return pageInfo;
                //}, () => new DistributedCacheEntryOptions
                //{
                //    SlidingExpiration = TimeSpan.FromMinutes(5)     //设置缓存过期时间为5分钟
                //});

                return ApiResult<PageInfoCount<CustomerDto>>.Success(ResultCode.Success,pageInfo);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

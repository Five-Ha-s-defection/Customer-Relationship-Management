using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.CustomerProcess.Clues.Helpers;
using CustomerRelationshipManagement.CustomerProcess.ContactRelations;
using CustomerRelationshipManagement.CustomerProcess.CustomerContacts.Helpers;
using CustomerRelationshipManagement.CustomerProcess.CustomerRegions;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CustomerProcess.Customers.Helpers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactRelations;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomerContacts;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerContacts
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class CustomerContactService : ApplicationService, ICustomerContactService
    {
        private readonly IRepository<CustomerContact> repository;
        private readonly IRepository<RoleInfo> rolerepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<ContactRelation> contactrelationrepository;
        private readonly IRepository<UserInfo> userrepository;
        private readonly ILogger<CustomerContactService> logger;
        private readonly IDistributedCache<PageInfoCount<CustomerContactDto>> cache;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public CustomerContactService(IRepository<CustomerContact> repository, IRepository<RoleInfo> rolerepository, IRepository<Customer> customerRepository, ILogger<CustomerContactService> logger, IDistributedCache<PageInfoCount<CustomerContactDto>> cache, IRepository<ContactRelation> contactrelationrepository, IRepository<UserInfo> userrepository, IConnectionMultiplexer connectionMultiplexer)
        {
            this.repository = repository;
            this.rolerepository = rolerepository;
            this.customerRepository = customerRepository;
            this.logger = logger;
            this.cache = cache;
            this.contactrelationrepository = contactrelationrepository;
            this.userrepository = userrepository;
            this.connectionMultiplexer = connectionMultiplexer;
        }

        /// <summary>
        /// 清楚关于c:PageInfo,k的所有信息
        /// </summary>
        /// <returns></returns>
        public async Task ClearAbpCacheAsync()
        {
            var endpoints = connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: "c:PageInfo,k:*");//填写自己的缓存前缀
                foreach (var key in keys)
                {
                    await connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }

        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public async Task<ApiResult<CustomerContactDto>> AddCustomerContact(CreateUpdateCustomerContactDto dto)
        {
            try
            {
                var customerContact = ObjectMapper.Map<CreateUpdateCustomerContactDto,CustomerContact>(dto);
                var list =await repository.InsertAsync(customerContact);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<CustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CustomerContactDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError("添加联系人信息出错！" + ex.Message);
                throw;
            }
        }
      
        /// <summary>
        /// 获取联系人详情信息
        /// </summary>
        /// <param name="id">要查询的联系人ID</param>
        /// <returns></returns>
        /// 
        [HttpGet]

        public async Task<ApiResult<CustomerContactDto>> GetCustomerContactById(Guid id)
        {
            try
            {
                var customerContact = await repository.GetAsync(x => x.Id == id);
                if (customerContact == null)
                {
                    return ApiResult<CustomerContactDto>.Fail("联系人信息不存在", ResultCode.NotFound);
                }
                return ApiResult<CustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CustomerContactDto>(customerContact));

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 删除联系人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult<CustomerContactDto>> DelCustomerContact(Guid id)
        {
            try
            {
                var customerContact = await repository.GetAsync(x => x.Id == id);
                if (customerContact == null)
                {
                    return ApiResult<CustomerContactDto>.Fail("联系人信息不存在", ResultCode.NotFound);
                }
                customerContact.IsDeleted = true;
                await repository.UpdateAsync(customerContact);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<CustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CustomerContactDto>(customerContact));
            }
            catch (Exception ex)
            {
               logger.LogError("删除联系人信息出错！" + ex.Message);
               throw;
            }
        }


        /// <summary>
        /// 修改联系人信息
        /// </summary>
        /// <param name="id">要修改的联系人ID</param>
        /// <param name="dto">联系人信息</param>
        /// <returns></returns>
        /// 
        [HttpPut]
        public async Task<ApiResult<CreateUpdateCustomerContactDto>> UpdCustomerContact(Guid id, CreateUpdateCustomerContactDto dto)
        {
            try
            {
                var customerContact = await repository.GetAsync(x => x.Id == id);
                if (customerContact == null)
                {
                    return ApiResult<CreateUpdateCustomerContactDto>.Fail("联系人信息不存在", ResultCode.NotFound);
                }
                var customerContactDto = ObjectMapper.Map(dto, customerContact);
                await repository.UpdateAsync(customerContactDto);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<CreateUpdateCustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CreateUpdateCustomerContactDto>(customerContactDto));

            }
            catch (Exception ex)
            {
                logger.LogError("修改联系人信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<List<CustomerDto>>> GetCustomerList()
        {
            try
            {
                var list = await customerRepository.GetQueryableAsync();
                var customer = list.Select(u=>new CustomerDto
                {
                    Id = u.Id,
                    CustomerName = u.CustomerName,
                }).ToList();
                return ApiResult<List<CustomerDto>>.Success(ResultCode.Success, customer);
            }
            catch (Exception ex)
            {
                logger.LogError("获取客户列表出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取联系人关系列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]

        public async Task<ApiResult<List<ContactRelationDto>>> GetContactRelationList()
        {
            try
            {
                var contactrelationlist = await contactrelationrepository.GetQueryableAsync();
                var result = contactrelationlist
                    .Select(u => new
                    {
                        IdString = u.Id.ToString(), // 转为 string
                        u.ContactRelationName
                    })
                    .AsEnumerable() // 从数据库取出后处理 Guid
                    .Where(u => Guid.TryParse(u.IdString, out _)) // 过滤非法 Guid
                    .Select(u => new ContactRelationDto
                    {
                        Id = Guid.Parse(u.IdString),
                        ContactRelationName = u.ContactRelationName
                    })
                    .ToList();
                return ApiResult<List<ContactRelationDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取客户列表出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<List<RoleDto>>> GetRoleDtoList()
        {
            try
            {
                var role=await rolerepository.GetQueryableAsync();
                var result = role.Select(u => new RoleDto
                {
                    Id = u.Id,
                    RoleName=u.RoleName,
                   
                }).ToList();
                return ApiResult<List<RoleDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 显示联系人列表信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<CustomerContactDto>>> ShowCustomerContact([FromQuery] SearchCustomerContactDto dto)
        {
            try
            {
                //构建缓存键名
                string cacheKey = CustomerContactCacheKeyHelper.BuildReadableKey(dto);
                //使用Redis缓存获取或添加数据
                var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
                {
                    var userlist = await userrepository.GetQueryableAsync();    
                    var customerlist=await customerRepository.GetQueryableAsync();
                    var relationlist = await contactrelationrepository.GetQueryableAsync();
                    var rolelist = await rolerepository.GetQueryableAsync();
                    var contactlist = await repository.GetQueryableAsync();
                    var list = from contact in contactlist
                               join customer in customerlist
                               on contact.CustomerId equals customer.Id
                               join rela in relationlist
                               on contact.ContactRelationId equals rela.Id
                               join role in rolelist
                               on contact.RoleId equals role.Id
                               join user in userlist on customer.UserId equals user.Id into userGroup
                               from user in userGroup.DefaultIfEmpty()
                               select new CustomerContactDto
                               {
                                   Id = contact.Id,
                                   ContactName = contact.ContactName,
                                   ContactRelationId = contact.ContactRelationId,
                                   ContactRelationName= rela.ContactRelationName,
                                   RoleId = contact.RoleId,
                                   RoleName = role.RoleName,
                                   Mobile=contact.Mobile,
                                   Email=contact.Email,
                                   QQ=contact.QQ,
                                   Remark=contact.Remark,
                                   Wechat=contact.Wechat,
                                   CustomerId= contact.CustomerId,
                                   CustomerName = customer.CustomerName,
                                   UserId = customer.UserId,
                                   UserName = user.UserName,
                                   CreatorId = contact.CreatorId,
                                   CreateName = user.UserName,
                                   CreationTime = contact.CreationTime,
                               };
                    // type: 0=全部，1=我负责的，2=我创建的
                    // 查看范围过滤
                    if (dto.type == 1 && dto.AssignedTo.HasValue) // 我负责的
                    {
                        list = list.Where(x => x.UserId == dto.AssignedTo.Value);
                    }
                    else if (dto.type == 2 && dto.AssignedTo.HasValue) // 我创建的
                    {
                        list = list.Where(x => x.CreatorId == dto.AssignedTo.Value);
                    }
                    //查询条件
                    if (!string.IsNullOrEmpty(dto.Keyword))
                    {
                        list = list.Where(x => x.ContactName.Contains(dto.Keyword)
                                               || x.Mobile.Contains(dto.Keyword)
                                               || x.Email.Contains(dto.Keyword)
                                               || x.Wechat.Contains(dto.Keyword));
                    }
                    // 时间区间筛选（创建时间）
                    if (dto.StartTime.HasValue && dto.EndTime.HasValue)
                    {
                        list = list.Where(x => x.CreationTime >= dto.StartTime && x.CreationTime <= dto.EndTime);
                    }
                    else if (dto.StartTime.HasValue)
                    {
                        list = list.Where(x => x.CreationTime >= dto.StartTime);
                    }
                    else if (dto.EndTime.HasValue)
                    {
                        list = list.Where(x => x.CreationTime <= dto.EndTime);
                    }
                   
                    
                    //用ABP框架的分页
                    var res = list.PageResult(dto.PageIndex, dto.PageSize);
                    //构建分页结果对象
                    var pageInfo = new PageInfoCount<CustomerContactDto>
                    {
                        TotalCount = res.RowCount,
                        PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                        Data = res.Queryable.ToList()
                    };
                    return pageInfo;
                }, () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(5)     //设置缓存过期时间为5分钟
                });

                return ApiResult<PageInfoCount<CustomerContactDto>>.Success(ResultCode.Success, redislist);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

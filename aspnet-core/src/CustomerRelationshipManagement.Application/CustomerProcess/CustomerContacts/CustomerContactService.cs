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
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomerContacts;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                string cacheKey = $"GetCustomerContact:{dto.StartTime}:{dto.EndTime}:{dto.UserId}:{dto.CreatorId}:{dto.CustomerId}:{(dto.ContactName ?? "null")}:{dto.ContactRelationId}:{dto.RoleId}:{dto.Salutation}:{dto.Position}:{dto.Mobile}:{dto.QQ }:{dto.Email}:{dto.Wechat}:{dto.Keyword}:{dto.PageIndex}:{dto.PageSize}";
                //使用Redis缓存获取或添加数据
                var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
                {
                    var cutomercontactlist = await repository.GetQueryableAsync();
                    var userlist = await userrepository.GetQueryableAsync();
                    var customerlist = await customerRepository.GetQueryableAsync();
                    var relationlist = await contactrelationrepository.GetQueryableAsync();
                    var rolelist = await rolerepository.GetQueryableAsync();
                    var contactlist = await repository.GetQueryableAsync();
                    var list = from cc in cutomercontactlist
                               join customer in customerlist on cc.CustomerId equals customer.Id into ccc
                               from customer in ccc.DefaultIfEmpty()
                               join relation in relationlist on cc.ContactRelationId equals relation.Id into ccr
                               from relation in ccr.DefaultIfEmpty()
                               join c in userlist on customer.UserId equals c.Id into rc
                               from c in rc.DefaultIfEmpty()
                               join r in rolelist on cc.RoleId equals r.Id into rcc
                               from r in rcc.DefaultIfEmpty()
                               join creator in userlist on cc.CreatorId equals creator.Id into creatorJoin
                               from creator in creatorJoin.DefaultIfEmpty()
                               select new CustomerContactDto
                               {
                                   Id = cc.Id,
                                   CreatorId = cc.CreatorId,
                                   CreatorName = creator.RealName,
                                   CreationTime = cc.CreationTime,
                                   CustomerId = cc.CustomerId,
                                   CustomerName = customer.CustomerName,
                                   UserId = customer.UserId,
                                   UserName = c.RealName, // 通过UserId获取的负责人名称
                                   ContactName = cc.ContactName,
                                   ContactRelationId = cc.ContactRelationId,
                                   ContactRelationName = relation.ContactRelationName,
                                   RoleId = cc.RoleId,
                                   RoleName = r.RoleName,
                                   Salutation = cc.Salutation,
                                   Position = cc.Position,
                                   Mobile = cc.Mobile,
                                   QQ = cc.QQ,
                                   Email = cc.Email,
                                   Wechat = cc.Wechat,
                                   Remark = cc.Remark,
                                   IsPrimary = cc.IsPrimary,
                               };
                    // 这里可以加上你的where条件，对p.xxx和r.xxx都可以筛选
                    list = list.WhereIf(!string.IsNullOrEmpty(dto.ContactName), x => x.ContactName.Contains(dto.ContactName))
                        .WhereIf(dto.StartTime.HasValue, x => x.CreationTime >= dto.StartTime.Value)
                        .WhereIf(dto.EndTime.HasValue, x => x.CreationTime <= dto.EndTime.Value.AddDays(1))
                        .WhereIf(dto.UserId.HasValue, x => x.UserId == dto.UserId.Value)
                        .WhereIf(dto.CustomerId.HasValue, x => x.CustomerId == dto.CustomerId.Value)
                        .WhereIf(dto.ContactRelationId.HasValue, x => x.ContactRelationId == dto.ContactRelationId.Value)
                        .WhereIf(dto.RoleId.HasValue, x => x.RoleId == dto.RoleId.Value)
                        .WhereIf(dto.CreatorId.HasValue, x => x.CreatorId == dto.CreatorId.Value)
                        .WhereIf(dto.Salutation.HasValue, x => x.Salutation == dto.Salutation.Value)
                        .WhereIf(!string.IsNullOrEmpty(dto.Position), x => x.Position.Contains(dto.Position))
                        .WhereIf(!string.IsNullOrEmpty(dto.Mobile), x => x.Mobile.Contains(dto.Mobile))
                        .WhereIf(!string.IsNullOrEmpty(dto.QQ), x => x.QQ.Contains(dto.QQ))
                        .WhereIf(!string.IsNullOrEmpty(dto.Email), x => x.Email.Contains(dto.Email))
                        .WhereIf(!string.IsNullOrEmpty(dto.Wechat), x => x.Wechat.Contains(dto.Wechat))
                        .WhereIf(!string.IsNullOrEmpty(dto.Keyword), x => x.ContactName.Contains(dto.Keyword)|| x.Mobile.Contains(dto.Keyword)|| x.Email.Contains(dto.Keyword)|| x.Wechat.Contains(dto.Keyword));

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
                    SlidingExpiration = TimeSpan.FromMinutes(5)     //设置缓存过期时间为5分钟
                });

                return ApiResult<PageInfoCount<CustomerContactDto>>.Success(ResultCode.Success, redislist);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改客户联系人是否为主要联系人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        public async Task<ApiResult<CreateUpdateCustomerContactDto>> UpdateIsPrimaryCustomerContact(Guid id,bool isPrimary)
        {
            try
            {
                var updCustomerContact = await repository.GetAsync(x => x.Id == id);
                if (updCustomerContact == null)
                {
                    return ApiResult<CreateUpdateCustomerContactDto>.Fail("客户联系人不存在", ResultCode.Fail);
                }
                updCustomerContact.IsPrimary = isPrimary;
                await repository.UpdateAsync(updCustomerContact);
                return ApiResult<CreateUpdateCustomerContactDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerContact, CreateUpdateCustomerContactDto>(updCustomerContact));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys.Helps;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CustomerProcess.Prioritys;
using CustomerRelationshipManagement.CustomerProcess.SalesProgresses;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Prioritys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.SalesProgresses;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IBusinessOpportunitys;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.ProductCategory.Products;
using CustomerRelationshipManagement.RBAC.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys
{
    /// <summary>
    /// 商机业务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class BusinessOpportunityService : ApplicationService, IBusinessOpportunityService
    {
        private readonly IRepository<BusinessOpportunity> businessopportunityrepository;
        private readonly IRepository<Customer> customerrepository;
        private readonly IRepository<Priority> priorityrepository;
        private readonly IRepository<SalesProgress> salesprogressrepository;
        private readonly IRepository<Product> productrepository;
        private readonly IRepository<Clue> cluerepository;
        private readonly IRepository<UserInfo> userrepository;
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly IDistributedCache cache;
        private readonly ILogger<BusinessOpportunityService> logger;
        public BusinessOpportunityService(IRepository<BusinessOpportunity> businessopportunityrepository, ILogger<BusinessOpportunityService> logger, IRepository<Customer> customerrepository, IRepository<Priority> priorityrepository, IRepository<SalesProgress> salesprogressrepository, IRepository<Product> productrepository, IRepository<Clue> cluerepository, IRepository<UserInfo> userrepository, IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
        {
            this.businessopportunityrepository = businessopportunityrepository;
            this.logger = logger;
            this.customerrepository = customerrepository;
            this.priorityrepository = priorityrepository;
            this.salesprogressrepository = salesprogressrepository;
            this.productrepository = productrepository;
            this.cluerepository = cluerepository;
            this.userrepository = userrepository;
            this.cache = cache;
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
        /// 添加商机
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<BusinessOpportunityDto>> AddBusinessOpportunity(CreateUpdateBusinessOpportunityDto dto)
        {
            try
            {
                var businessopportunity = ObjectMapper.Map<CreateUpdateBusinessOpportunityDto, BusinessOpportunity>(dto);
                // 自动生成客户编号 C-年月日时分-四位纯数字或大写字母数字混合
                var now = DateTime.Now; // 获取当前时间
                var random = new Random(); // 创建随机数生成器
                string randomStr;
                if (random.Next(2) == 0) // 50%概率生成纯数字
                {
                    // 生成1000~9999的四位纯数字
                    randomStr = random.Next(1000, 10000).ToString("D4");
                }
                else // 50%概率生成大写字母和数字混合
                {
                    // 生成1000~FFFF的四位十六进制字符串（大写，含字母和数字）
                    randomStr = random.Next(0x1000, 0x10000).ToString("X4");
                }
                // 拼接客户编号，格式如C-202506240038-3B7C或C-202506240038-1540
                businessopportunity.BusinessOpportunityCode = $"S-{now:yyyyMMddHHmm}-{randomStr}";
                var list = await businessopportunityrepository.InsertAsync(businessopportunity);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<BusinessOpportunityDto>.Success(ResultCode.Success, ObjectMapper.Map<BusinessOpportunity, BusinessOpportunityDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "添加商机失败");
                return ApiResult<BusinessOpportunityDto>.Fail("添加商机失败", ResultCode.Fail);
            }
        }


        /// <summary>
        /// 获取客户下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<CustomerSimpleDto>>> GetCustomerSelectList()
        {
            try
            {
                var customerList = await customerrepository.GetQueryableAsync();
                var result = customerList
                    .Select(u => new CustomerSimpleDto
                    {
                        Id = u.Id,
                        CustomerCode = u.CustomerCode,
                        CustomerName = u.CustomerName,
                        CustomerPhone = u.CustomerPhone,
                        CreationTime = u.CreationTime
                    })
                    .ToList();
                return ApiResult<List<CustomerSimpleDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取商机优先级下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<PriorityDto>>> GetPrioritySelectList()
        {
            try
            {
                var priorityList = await priorityrepository.GetListAsync();
                var result = priorityList
                    .Select(u => new PriorityDto
                    {
                        Id = u.Id,
                        PriorityName = u.PriorityName
                    })
                    .ToList();
                return ApiResult<List<PriorityDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取商机销售进度下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<SalesProgressDto>>> GetSalesProgressSelectList()
        {
            try
            {
                var salesProgressList = await salesprogressrepository.GetListAsync();
                var result = salesProgressList
                    .Select(u => new SalesProgressDto
                    {
                        Id = u.Id,
                        SalesProgressName = u.SalesProgressName
                    })
                    .ToList();
                return ApiResult<List<SalesProgressDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取产品下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<ProductDtos>>> GetProductSelectList()
        {
            try
            {
                // 从仓储获取所有产品列表
                var productList = (await productrepository.GetListAsync())
                    .Where(u => Guid.TryParse(u.Id.ToString(), out _)) // 只保留合法GUID
                    .ToList();

                // 过滤掉Id不是合法Guid的产品，并映射为ProductDtos
                var result = productList
                    .Where(u => u.Id != Guid.Empty) // 过滤掉空Guid
                    .Select(u => new ProductDtos
                    {
                        Id = u.Id,
                        CategoryId = u.CategoryId,
                        ProductImageUrl = u.ProductImageUrl,
                        ProductBrand = u.ProductBrand,
                        ProductCode = u.ProductCode,
                        DealPrice = u.DealPrice
                    })
                    .ToList();

                return ApiResult<List<ProductDtos>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取产品下拉列表失败");
                return ApiResult<List<ProductDtos>>.Fail("获取产品下拉列表失败", ResultCode.Fail);
            }
        }

        /// <summary>
        /// 显示商机列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<BusinessOpportunityDto>>> ShowBusinessOpportunity([FromQuery] SearchBusinessOpportunityDto dto)
        {
            try
            {
                string cacheKey = BusinessOpportunityCacheKeyHelper.BuildReadableKey(dto);
                // 1. 读取缓存
                var bytes = await cache.GetAsync(cacheKey);
                PageInfoCount<BusinessOpportunityDto> redislist = null;
                if (bytes != null)
                {
                    // 2. 反序列化
                    var json = Encoding.UTF8.GetString(bytes);
                    redislist = JsonConvert.DeserializeObject<PageInfoCount<BusinessOpportunityDto>>(json);
                }

                if (redislist == null)
                {
                    // 3. 查询数据库
                    redislist = await GetBusinessOpportunityList(dto);

                    // 4. 序列化并写入缓存
                    var jsonStr = JsonConvert.SerializeObject(redislist);
                    var bytesToCache = Encoding.UTF8.GetBytes(jsonStr);
                    await cache.SetAsync(cacheKey, bytesToCache, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    });
                }
                return ApiResult<PageInfoCount<BusinessOpportunityDto>>.Success(ResultCode.Success, redislist);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // 私有方法，封装原有LINQ查询和数据处理逻辑
        private async Task<PageInfoCount<BusinessOpportunityDto>> GetBusinessOpportunityList(SearchBusinessOpportunityDto dto)
        {
            var userlist = await userrepository.GetQueryableAsync();
            var cluelist = await cluerepository.GetQueryableAsync();
            var customerlist = await customerrepository.GetQueryableAsync();
            var productlist = await productrepository.GetQueryableAsync();
            var businessopportunitylist = await businessopportunityrepository.GetQueryableAsync();
            var prioritylist = await priorityrepository.GetQueryableAsync();
            var salesprogresslist = await salesprogressrepository.GetQueryableAsync();
            var list = from bus in businessopportunitylist
                       join cus in customerlist on bus.CustomerId equals cus.Id
                       join clu in cluelist on cus.ClueId equals clu.Id into clueGroup
                       from clu in clueGroup.DefaultIfEmpty()
                       join user in userlist on cus.UserId equals user.Id into userGroup
                       from user in userGroup.DefaultIfEmpty()
                       join creator in userlist on bus.CreatorId equals creator.Id into creatorGroup
                       from creator in creatorGroup.DefaultIfEmpty()
                       join priority in prioritylist on bus.PriorityId equals priority.Id into priorityGroup
                       from priority in priorityGroup.DefaultIfEmpty()
                       join sale in salesprogresslist on bus.SalesProgressId equals sale.Id into saleGroup
                       from sale in saleGroup.DefaultIfEmpty()
                       join product in productlist on bus.ProductId equals product.Id into productGroup
                       from product in productGroup.DefaultIfEmpty()
                       select new BusinessOpportunityDto
                       {
                           Id = bus.Id,
                           PriorityId = bus.PriorityId,
                           PriorityName = priority != null ? priority.PriorityName : null,
                           BusinessOpportunityName = bus.BusinessOpportunityName,
                           CustomerId = bus.CustomerId,
                           CustomerName = cus.CustomerName,
                           SalesProgressId = bus.SalesProgressId,
                           SalesProgressName = sale != null ? sale.SalesProgressName : null,
                           LastFollowTime = clu != null ? clu.LastFollowTime : null,
                           NextContactTime = clu != null ? clu.NextContactTime : null,
                           CreationTime = bus.CreationTime,
                           CreatorId = bus.CreatorId,
                           CreateName = creator == null ? "" : creator.UserName,
                           UserId = cus.UserId,
                           UserName = user == null ? "" : user.UserName,
                           BusinessOpportunityCode = bus.BusinessOpportunityCode,
                           Budget = bus.Budget,
                           ExpectedDate = bus.ExpectedDate,
                           Remark = bus.Remark,
                           ProductId = bus.ProductId,
                           ProductBrand = product != null ? product.ProductBrand : null
                       };
            // 查看范围过滤
            if (dto.type == 1 && dto.AssignedTo.HasValue) // 我负责的
            {
                list = list.Where(x => x.UserId == dto.AssignedTo.Value);
            }
            else if (dto.type == 2 && dto.AssignedTo.HasValue) // 我创建的
            {
                list = list.Where(x => x.CreatorId == dto.AssignedTo.Value);
            }
            // 查询条件
            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                list = list.Where(x => x.BusinessOpportunityCode.Contains(dto.Keyword)
                               || x.BusinessOpportunityName.Contains(dto.Keyword));
            }
            // 根据销售进度查询
            if (dto.SalesProgressList != null && dto.SalesProgressList.Any())
            {
                list = list.Where(x => dto.SalesProgressList.Contains(x.SalesProgressId));
            }
            // 时间筛选
            if (dto.StartTime.HasValue && dto.EndTime.HasValue && dto.TimeType.HasValue)
            {
                list = dto.TimeType switch
                {
                    TimeField.CreateTime => list.Where(x => x.CreationTime >= dto.StartTime && x.CreationTime <= dto.EndTime),
                    TimeField.NextContact => list.Where(x => x.NextContactTime >= dto.StartTime && x.NextContactTime <= dto.EndTime),
                    TimeField.LastFollow => list.Where(x => x.LastFollowTime >= dto.StartTime && x.LastFollowTime <= dto.EndTime),
                    _ => list
                };
            }

            // 高级筛选字段处理
            if (dto.MatchMode == 0) // 全部满足(AND)
            {
                if (dto.UserIds != null && dto.UserIds.Count > 0)
                    list = list.Where(x => dto.UserIds.Contains(x.UserId));
                if (dto.CreatedByIds != null && dto.CreatedByIds.Count > 0)
                    list = list.Where(x => x.CreatorId.HasValue && dto.CreatedByIds.Contains(x.CreatorId.Value));
                if (dto.CustomerId != null && dto.CustomerId != Guid.Empty)
                    list = list.Where(x => x.CustomerId == dto.CustomerId);
                if (!string.IsNullOrEmpty(dto.BusinessOpportunityCode))
                    list = list.Where(x => x.BusinessOpportunityCode != null && x.BusinessOpportunityCode.Contains(dto.BusinessOpportunityCode));
                if (dto.PriorityId != null && dto.PriorityId != Guid.Empty)
                    list = list.Where(x => x.PriorityId == dto.PriorityId);
                if (!string.IsNullOrEmpty(dto.BusinessOpportunityName))
                    list = list.Where(x => x.BusinessOpportunityName != null && x.BusinessOpportunityName.Contains(dto.BusinessOpportunityName));
                if (dto.SalesProgressId != null && dto.SalesProgressId != Guid.Empty)
                    list = list.Where(x => x.SalesProgressId == dto.SalesProgressId);
                if (dto.Budget > 0)
                    list = list.Where(x => x.Budget <= dto.Budget);
                if (dto.ExpectedDate != default)
                    list = list.Where(x => x.ExpectedDate <= dto.ExpectedDate);
            }
            else // 部分满足(OR)
            {
                list = list.Where(x =>
                    (dto.UserIds != null && dto.UserIds.Count > 0 && dto.UserIds.Contains(x.UserId)) ||
                    (dto.CreatedByIds != null && dto.CreatedByIds.Count > 0 && x.CreatorId.HasValue && dto.CreatedByIds.Contains(x.CreatorId.Value)) ||
                    (dto.CustomerId != null && dto.CustomerId != Guid.Empty && x.CustomerId == dto.CustomerId) ||
                    (dto.PriorityId != null && dto.PriorityId != Guid.Empty && x.PriorityId == dto.PriorityId) ||
                    (!string.IsNullOrEmpty(dto.BusinessOpportunityCode) && x.BusinessOpportunityCode != null && x.BusinessOpportunityCode.Contains(dto.BusinessOpportunityCode)) ||
                    (!string.IsNullOrEmpty(dto.BusinessOpportunityName) && x.BusinessOpportunityName != null && x.BusinessOpportunityName.Contains(dto.BusinessOpportunityName)) || 
                     ((dto.ExpectedDate != default) && x.ExpectedDate <= dto.ExpectedDate) ||
                            (dto.Budget > 0 && x.Budget <= dto.Budget) ||
                    (dto.SalesProgressId != null && dto.SalesProgressId != Guid.Empty && x.SalesProgressId == dto.SalesProgressId));
            }

            // 排序
            if (dto.OrderBy.HasValue)
            {
                list = (dto.OrderBy.Value, dto.OrderDesc) switch
                {
                    (TimeField.CreateTime, true) => list.OrderByDescending(x => x.CreationTime),
                    (TimeField.CreateTime, false) => list.OrderBy(x => x.CreationTime),
                    (TimeField.NextContact, true) => list.OrderByDescending(x => x.NextContactTime),
                    (TimeField.NextContact, false) => list.OrderBy(x => x.NextContactTime),
                    (TimeField.LastFollow, true) => list.OrderByDescending(x => x.LastFollowTime),
                    (TimeField.LastFollow, false) => list.OrderBy(x => x.LastFollowTime),
                    _ => list.OrderByDescending(x => x.LastFollowTime)
                };
            }
            //用ABP框架的分页
            var res = list.PageResult(dto.PageIndex, dto.PageSize);
            //构建分页结果对象
            return new PageInfoCount<BusinessOpportunityDto>
            {
                TotalCount = res.RowCount,
                PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                Data = res.Queryable.ToList()
            };
        }


        /// <summary>
        /// 删除商机列表
        /// </summary>
        /// <typeparam name="BusinessOpportunity"></typeparam>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult<BusinessOpportunityDto>> DelBusinessOpportunity(Guid id)
        {
            try
            {
                var bus = await businessopportunityrepository.GetAsync(x => x.Id == id);
                if (bus == null)
                {
                    return ApiResult<BusinessOpportunityDto>.Fail("您要删除的商机信息不存在", ResultCode.NotFound);
                }
                bus.IsDeleted = true;
                await businessopportunityrepository.UpdateAsync(bus);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<BusinessOpportunityDto>.Success(ResultCode.Success, ObjectMapper.Map<BusinessOpportunity, BusinessOpportunityDto>(bus));
            }
            catch (Exception ex)
            {
                logger.LogError("删除商机信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取商机详情信息
        /// </summary>
        /// <param name="id">要查询的商机ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<BusinessOpportunityDto>> GetBusinessOpportunityById(Guid id)
        {
            try
            {
                var businessopportunity = await businessopportunityrepository.GetAsync(x => x.Id == id);
                if (businessopportunity == null)
                {
                    return ApiResult<BusinessOpportunityDto>.Fail("商机信息不存在", ResultCode.NotFound);
                }
                return ApiResult<BusinessOpportunityDto>.Success(ResultCode.Success, ObjectMapper.Map<BusinessOpportunity, BusinessOpportunityDto>(businessopportunity));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 修改商机信息
        /// </summary>
        /// <param name="id">要修改的商机ID</param>
        /// <param name="dto">商机信息</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<CreateUpdateBusinessOpportunityDto>> UpdCustomer(Guid id, CreateUpdateBusinessOpportunityDto dto)
        {
            try
            {
                var businessopportunity = await businessopportunityrepository.GetAsync(x => x.Id == id);
                if (businessopportunity == null)
                {
                    return ApiResult<CreateUpdateBusinessOpportunityDto>.Fail("未找到要修改的商机", ResultCode.NotFound);
                }
                var businessopportunityDto = ObjectMapper.Map(dto, businessopportunity);
                await businessopportunityrepository.UpdateAsync(businessopportunity);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<CreateUpdateBusinessOpportunityDto>.Success(ResultCode.Success, ObjectMapper.Map<BusinessOpportunity, CreateUpdateBusinessOpportunityDto>(businessopportunity));
            }
            catch (Exception ex)
            {
                logger.LogError("修改线索信息出错！" + ex.Message);
                throw;
            }
        }
    }
}

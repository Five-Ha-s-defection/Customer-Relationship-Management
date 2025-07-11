﻿using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.CustomerProcess.Cars;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.CustomerProcess.ClueSources;
using CustomerRelationshipManagement.CustomerProcess.CustomerContacts;
using CustomerRelationshipManagement.CustomerProcess.CustomerLevels;
using CustomerRelationshipManagement.CustomerProcess.CustomerRegions;
using CustomerRelationshipManagement.CustomerProcess.Customers.Helpers;
using CustomerRelationshipManagement.CustomerProcess.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Cars;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Levels;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomers;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.Customers
{
    /// <summary>
    /// 客户服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class CustomerService:ApplicationService,ICustomerService
    {
        private readonly IRepository<Customer> repository;
        private readonly IRepository<Clue> clueRepository;
        private readonly IRepository<UserInfo> userRepository;
        private readonly IRepository<CarFrameNumber> carRepository;
        private readonly IRepository<CustomerLevel> levelRepository;
        private readonly IRepository<ClueSource> sourceRepository;
        private readonly IRepository<CustomerRegion> regionRepository;
        private readonly IRepository<CustomerType> typeRepository;
        private readonly IRepository<CustomerContact> contactRepository; 
        private readonly ILogger<CustomerService> logger;
        private readonly IDistributedCache<PageInfoCount<CustomerDto>> cache;
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerService(IRepository<Customer> repository, ILogger<CustomerService> logger, IDistributedCache<PageInfoCount<CustomerDto>> cache, IRepository<Clue> clueRepository, IRepository<UserInfo> userRepository, IRepository<CarFrameNumber> carRepository, IRepository<CustomerLevel> levelRepository, IRepository<ClueSource> sourceRepository, IRepository<CustomerRegion> regionRepository, IConnectionMultiplexer connectionMultiplexer, IRepository<CustomerType> typeRepository, IRepository<CustomerContact> contactRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.logger = logger;
            this.cache = cache;
            this.clueRepository = clueRepository;
            this.userRepository = userRepository;
            this.carRepository = carRepository;
            this.levelRepository = levelRepository;
            this.sourceRepository = sourceRepository;
            this.regionRepository = regionRepository;
            this.connectionMultiplexer = connectionMultiplexer;
            this.typeRepository = typeRepository;
            this.contactRepository = contactRepository;
            _httpContextAccessor = httpContextAccessor;
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
        /// 添加客户信息
        /// </summary>
        /// <param name="dto">要添加的客户信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CustomerDto>> AddCustomer(CreateUpdateCustomerDto dto)
        {
            try
            {
                // 将前端传来的DTO映射为数据库实体Customer
                var customer = ObjectMapper.Map<CreateUpdateCustomerDto, Customer>(dto);
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
                customer.CustomerCode = $"C-{now:yyyyMMddHHmm}-{randomStr}";
                // 插入客户数据到数据库
                var list = await repository.InsertAsync(customer);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                // 返回插入后的客户信息（DTO）
                return ApiResult<CustomerDto>.Success(ResultCode.Success, ObjectMapper.Map<Customer, CustomerDto>(list));
            }
            catch (Exception ex)
            {
                // 记录异常日志
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
                //缓存key(动态分页查询不同的key)
                string cacheKey = CustomerCacheKeyHelper.BuildReadableKey(dto);
                //使用Redis缓存获取或添加数据
                var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
                {
                    var customerlist = await repository.GetQueryableAsync();
                    var cluelist = await clueRepository.GetQueryableAsync();
                    var userlist = await userRepository.GetQueryableAsync();
                    var carlist = await carRepository.GetQueryableAsync();
                    var levelist = await levelRepository.GetQueryableAsync();
                    var regionlist = await regionRepository.GetQueryableAsync();
                    var typelist = await typeRepository.GetQueryableAsync();
                    var sourceList = await sourceRepository.GetQueryableAsync();
                    // var contactlist = await contactRepository.GetQueryableAsync(); // 联系人相关，已注释
                    var list = from cus in customerlist
                               join clu in cluelist on cus.ClueId equals clu.Id into clueGroup
                               from clu in clueGroup.DefaultIfEmpty()
                               join user in userlist on cus.UserId equals user.Id into userGroup
                               from user in userGroup.DefaultIfEmpty()
                               join car in carlist on cus.CarFrameNumberId equals car.Id into carGroup
                               from car in carGroup.DefaultIfEmpty()
                               join level in levelist on cus.CustomerLevelId equals level.Id into levelGroup
                               from level in levelGroup.DefaultIfEmpty()
                               join region in regionlist on cus.CustomerRegionId equals region.Id into regionGroup
                               from region in regionGroup.DefaultIfEmpty()
                               join type in typelist on cus.CustomerTypeId equals type.Id into typeGroup
                               from type in typeGroup.DefaultIfEmpty()
                               join source in sourceList on cus.CustomerSourceId equals source.Id into sourceGroup
                               from source in sourceGroup.DefaultIfEmpty()
                               join creator in userlist on cus.CreatorId equals creator.Id into creatorGroup
                               from creator in creatorGroup.DefaultIfEmpty()
                                   // join contact in contactlist on cus.Id equals contact.CustomerId into contactGroup
                                   // from contact in contactGroup.DefaultIfEmpty()
                               select new CustomerDto
                               {
                                   Id = cus.Id,
                                   UserId = cus.UserId,
                                   UserName = user != null ? user.UserName : null,
                                   CustomerName = cus.CustomerName,
                                   CheckAmount = cus.CheckAmount,
                                   CustomerPhone = cus.CustomerPhone,
                                   CustomerSourceId = cus.CustomerSourceId,
                                   ClueSourceName = source != null ? source.ClueSourceName : null,
                                   ClueId = cus.ClueId,
                                   LastFollowTime = clu != null ? clu.LastFollowTime : null,
                                   NextContactTime = clu != null ? clu.NextContactTime : null,
                                   CreationTime = cus.CreationTime,
                                   CreatorId = creator != null ? creator.Id : Guid.Empty,
                                   CreateName = creator != null ? creator.RealName : null,
                                   ClueWechat = clu != null ? clu.ClueWechat : null,
                                   CustomerEmail = cus.CustomerEmail,
                                   CustomerCode = cus.CustomerCode,
                                   CustomerExpireTime = cus.CustomerExpireTime,
                                   CarFrameNumberId = cus.CarFrameNumberId,
                                   CarFrameNumberName = car != null ? car.CarFrameNumberName : null,
                                   CustomerRegionId = cus.CustomerRegionId,
                                   CustomerRegionName = region != null ? region.CustomerRegionName : null,
                                   CustomerTypeId = cus.CustomerTypeId,
                                   CustomerTypeName = type != null ? type.CustomerTypeName : null,
                                   CustomerLevelId = cus.CustomerLevelId,
                                   CustomerLevelName = level != null ? level.CustomerLevelName : null,
                                   CustomerAddress = cus.CustomerAddress,
                                   CustomerRemark = cus.CustomerRemark,
                                   // ContactName= contact != null ? contact.ContactName : null, // 联系人相关，已注释
                                   // Mobile= contact != null ? contact.Mobile : null, // 联系人相关，已注释
                                   // Email= contact != null ? contact.Email : null, // 联系人相关，已注释
                               };
                    // type: 0=全部，1=我负责的，2=我创建的
                    if (dto.type == 1 && dto.AssignedTo.HasValue)
                    {
                        list = list.Where(x => x.UserId == dto.AssignedTo);
                    }
                    else if (dto.type == 2 && dto.CreatedBy.HasValue)
                    {
                        list = list.Where(x => x.CreatorId == dto.CreatedBy);
                    }


                    ////区分客户池和客户
                    //list = list.WhereIf(dto.type == 1 && dto.AssignedTo.HasValue, x => x.UserId == dto.AssignedTo);

                    //查询条件
                    //根据客户姓名、（联系人）、电话、邮箱、（微信号）模糊查询
                    if (!string.IsNullOrEmpty(dto.Keyword))
                    {
                        list = list.Where(x => x.CustomerName.Contains(dto.Keyword)
                                               || x.CustomerPhone.Contains(dto.Keyword)
                                               || x.ClueWechat.Contains(dto.Keyword)
                                               || x.CustomerEmail.Contains(dto.Keyword));
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
                        if (!string.IsNullOrEmpty(dto.CustomerCode))
                            list = list.Where(x => x.CustomerCode.Contains(dto.CustomerCode));
                        if (!string.IsNullOrEmpty(dto.CustomerName))
                            list = list.Where(x => x.CustomerName.Contains(dto.CustomerName));
                        if (dto.CustomerExpireTime != default)
                            list = list.Where(x => x.CustomerExpireTime <= dto.CustomerExpireTime);
                        if (dto.CheckAmount > 0)
                            list = list.Where(x => x.CheckAmount <= dto.CheckAmount);
                        if (dto.CarFrameNumberId != Guid.Empty)
                            list = list.Where(x => x.CarFrameNumberId == dto.CarFrameNumberId);
                        if (dto.CustomerLevelId != Guid.Empty)
                            list = list.Where(x => x.CustomerLevelId == dto.CustomerLevelId);
                        if (!string.IsNullOrEmpty(dto.CustomerPhone))
                            list = list.Where(x => x.CustomerPhone.Contains(dto.CustomerPhone));
                        if (!string.IsNullOrEmpty(dto.CustomerEmail))
                            list = list.Where(x => x.CustomerEmail.Contains(dto.CustomerEmail));
                        if (dto.CustomerTypeId != Guid.Empty)
                            list = list.Where(x => x.CustomerTypeId == dto.CustomerTypeId);
                        if (dto.CustomerSourceId != Guid.Empty)
                            list = list.Where(x => x.CustomerSourceId == dto.CustomerSourceId);
                        if (dto.CustomerRegionId != Guid.Empty)
                            list = list.Where(x => x.CustomerRegionId == dto.CustomerRegionId);
                        if (!string.IsNullOrEmpty(dto.CustomerAddress))
                            list = list.Where(x => x.CustomerAddress.Contains(dto.CustomerAddress));
                        // if (!string.IsNullOrEmpty(dto.ContactName))
                        //     list = list.Where(x => x.ContactName.Contains(dto.ContactName)); // 联系人相关，已注释
                        // if (!string.IsNullOrEmpty(dto.Mobile))
                        //     list = list.Where(x => x.Mobile.Contains(dto.Mobile)); // 联系人相关，已注释
                        // if (!string.IsNullOrEmpty(dto.Email))
                        //     list = list.Where(x => x.Email.Contains(dto.Email)); // 联系人相关，已注释
                    }
                    else // 部分满足(OR)
                    {
                        list = list.Where(x =>
                            (dto.UserIds != null && dto.UserIds.Count > 0 && dto.UserIds.Contains(x.UserId)) ||
                            (dto.CreatedByIds != null && dto.CreatedByIds.Count > 0 && dto.CreatedByIds.Contains(x.CreatorId.Value)) ||
                            (dto.CarFrameNumberId != Guid.Empty && x.CarFrameNumberId == dto.CarFrameNumberId) ||
                            (dto.CustomerLevelId != Guid.Empty && x.CustomerLevelId == dto.CustomerLevelId) ||
                            (!string.IsNullOrEmpty(dto.CustomerCode) && x.CustomerCode.Contains(dto.CustomerCode)) ||
                             (!string.IsNullOrEmpty(dto.CustomerName) && x.CustomerName.Contains(dto.CustomerName)) ||
                            ((dto.CustomerExpireTime != default) && x.CustomerExpireTime <= dto.CustomerExpireTime) ||
                            (dto.CheckAmount > 0 && x.CheckAmount <= dto.CheckAmount) ||
                            (!string.IsNullOrEmpty(dto.CustomerPhone) && x.CustomerPhone.Contains(dto.CustomerPhone)) ||
                            (!string.IsNullOrEmpty(dto.CustomerEmail) && x.CustomerEmail.Contains(dto.CustomerEmail)) ||
                            (dto.CustomerTypeId != Guid.Empty && x.CustomerTypeId == dto.CustomerTypeId) ||
                            (dto.CustomerSourceId != Guid.Empty && x.CustomerSourceId == dto.CustomerSourceId) ||
                            (dto.CustomerRegionId != Guid.Empty && x.CustomerRegionId == dto.CustomerRegionId) ||
                            (!string.IsNullOrEmpty(dto.CustomerAddress) && x.CustomerAddress.Contains(dto.CustomerAddress))
                         // (!string.IsNullOrEmpty(dto.ContactName) && x.ContactName.Contains(dto.ContactName)) || // 联系人相关，已注释
                         // (!string.IsNullOrEmpty(dto.Mobile) && x.Mobile.Contains(dto.Mobile)) || // 联系人相关，已注释
                         // (!string.IsNullOrEmpty(dto.Email) && x.Email.Contains(dto.Email)) || // 联系人相关，已注释
                         );
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
                    //数据为空时不缓存
                    if (res.RowCount == 0)
                    {
                        // 不缓存空数据
                        return new PageInfoCount<CustomerDto>
                        {
                            TotalCount = 0,
                            PageCount = 0,
                            Data = new List<CustomerDto>()
                        };
                    }
                    //构建分页结果对象
                    return new PageInfoCount<CustomerDto>
                    {
                        TotalCount = res.RowCount,
                        PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                        Data = res.Queryable.ToList()
                    };
                }, () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(1)     //设置缓存过期时间为5分钟
                });
                return ApiResult<PageInfoCount<CustomerDto>>.Success(ResultCode.Success, redislist);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取客户详情信息
        /// </summary>
        /// <param name="id">要查询的客户ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<CustomerDto>> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await repository.GetAsync(x => x.Id == id);
                if (customer == null)
                {
                    return ApiResult<CustomerDto>.Fail("客户信息不存在", ResultCode.NotFound);
                }
                return ApiResult<CustomerDto>.Success(ResultCode.Success, ObjectMapper.Map<Customer, CustomerDto>(customer));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="id">要删除的客户ID</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult<CustomerDto>> DelCustomer(Guid id)
        {
            try
            {
                var customer = await repository.GetAsync(x => x.Id == id);
                if (customer == null)
                {
                    return ApiResult<CustomerDto>.Fail("您要删除的客户信息不存在", ResultCode.NotFound);
                }
                customer.IsDeleted = true;
                await repository.UpdateAsync(customer);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<CustomerDto>.Success(ResultCode.Success, ObjectMapper.Map<Customer, CustomerDto>(customer));
            }
            catch (Exception ex)
            {
                logger.LogError("删除客户信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 修改客户信息
        /// </summary>
        /// <param name="id">要修改的客户ID</param>
        /// <param name="dto">客户信息</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<CreateUpdateCustomerDto>> UpdCustomer(Guid id, CreateUpdateCustomerDto dto)
        {
            try
            {
                var customer = await repository.GetAsync(x => x.Id == id);
                if (customer == null)
                {
                    return ApiResult<CreateUpdateCustomerDto>.Fail("未找到要修改的客户", ResultCode.NotFound);
                }
                var customerDto = ObjectMapper.Map(dto, customer);
                await repository.UpdateAsync(customerDto);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                return ApiResult<CreateUpdateCustomerDto>.Success(ResultCode.Success, ObjectMapper.Map<Customer, CreateUpdateCustomerDto>(customerDto));
            }
            catch (Exception ex)
            {
                logger.LogError("修改线索信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取用户下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<UserInfoDto>>> GetUserSelectList()
        {
            try
            {
                var userList = await userRepository.GetQueryableAsync();
                var result = userList
                    .Where(u => u.IsActive) // 只取有效用户，如有需要
                    .Select(u => new UserInfoDto
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                    })
                    .ToList();

                return ApiResult<List<UserInfoDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取用户下拉框数据出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取车架号下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<CarDto>>> GetCarSelectList()
        {
            try
            {
                var carlist = await carRepository.GetQueryableAsync();

                var result = carlist
                    .Select(u => new
                    {
                        IdString = u.Id.ToString(), // 转为 string
                        u.CarFrameNumberName
                    })
                    .AsEnumerable() // 从数据库取出后处理 Guid
                    .Where(u => Guid.TryParse(u.IdString, out _)) // 过滤非法 Guid
                    .Select(u => new CarDto
                    {
                        Id = Guid.Parse(u.IdString),
                        CarFrameNumberName = u.CarFrameNumberName
                    })
                    .ToList();

                return ApiResult<List<CarDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取客户级别下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<LevelDto>>> GetLevelSelectList()
        {
            try
            {
                var levelList = await levelRepository.GetQueryableAsync();
                var result = levelList
                    .Select(u => new 
                    {
                        IdString = u.Id.ToString(), // 转为 string
                        u.CustomerLevelName
                    })
                      .AsEnumerable() // 从数据库取出后处理 Guid
                    .Where(u => Guid.TryParse(u.IdString, out _)) // 过滤非法 Guid
                    .Select(u => new LevelDto
                    {
                        Id = Guid.Parse(u.IdString),
                        CustomerLevelName = u.CustomerLevelName
                    })
                    .ToList();  
                return ApiResult<List<LevelDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取客户级别下拉框数据出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取来源下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<SourceDto>>> GetSourceSelectList()
        {
            try
            {
                var sourceList = await sourceRepository.GetQueryableAsync();
                var result = sourceList
                    .Select(u => new
                    {
                        IdString=u.Id.ToString(),
                        u.ClueSourceName,
                        u.ClueSourceStatus,
                        u.ClueSourceContent,
                        u.CreateTime
                    })
                    .AsEnumerable()
                    .Where(u=>Guid.TryParse(u.IdString,out _))
                    .Select(u=>new SourceDto
                    {
                        Id=Guid.Parse(u.IdString),
                        ClueSourceName=u.ClueSourceName,
                        ClueSourceStatus=u.ClueSourceStatus,
                        ClueSourceContent=u.ClueSourceContent,
                        CreateTime=u.CreateTime
                    })
                    .ToList();
                return ApiResult<List<SourceDto>>.Success(ResultCode.Success,result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取客户地区下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<RegionDto>>> GetRegionCascadeList()
        {
            try
            {
                var allRegions = await regionRepository.GetListAsync();
                // 递归构建树
                List<RegionDto> BuildTree(Guid parentId)
                {
                    return allRegions
                        .Where(r => r.ParentId == parentId)
                        .Select(r => new RegionDto
                        {
                            Id = r.Id,
                            CustomerRegionName = r.CustomerRegionName,
                            ParentId = r.ParentId,
                            Children = BuildTree(r.Id)
                        })
                        .ToList();
                }
                // 假设根节点ParentId为Guid.Empty
                var tree = BuildTree(Guid.Empty);
                return ApiResult<List<RegionDto>>.Success(ResultCode.Success, tree);
            }
            catch (Exception ex)
            {
                logger.LogError("获取客户地区级联数据出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取客户类别下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<CustomerTypeDto>>> GetCustomerType()
        {
            try
            {
                var typeList = await typeRepository.GetQueryableAsync();
                var result = typeList
                    .Select(u => new
                    {
                        IdString = u.Id.ToString(), // 转为 string
                        u.CustomerTypeName,
                    })
                      .AsEnumerable() // 从数据库取出后处理 Guid
                    .Where(u => Guid.TryParse(u.IdString, out _)) // 过滤非法 Guid
                    .Select(u => new CustomerTypeDto
                    {
                        Id = Guid.Parse(u.IdString),
                        CustomerTypeName = u.CustomerTypeName
                    })
                    .ToList();
                return ApiResult<List<CustomerTypeDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取客户级别下拉框数据出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 上传图片（附文本使用）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<EditorUploadResult> UploadImageForEditAsync(IFormFile file)
        {
            var result = new EditorUploadResult();
            try
            {
                var str = Guid.NewGuid().ToString();
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "editors");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fileName = str + "_" + file.FileName;
                var savePath = Path.Combine(folder, fileName);

                using (var fs = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                // 通过 IHttpContextAccessor 获取请求信息
                var request = _httpContextAccessor.HttpContext?.Request;
                var scheme = request?.Scheme ?? "http";
                var host = request?.Host.Value ?? "localhost";
                var url = $"{scheme}://{host}/editors/{fileName}";

                result.errno = 0;
                result.data = new { url = url };
                return result;
            }
            catch (Exception ex)
            {
                result.errno = 1;
                result.data = new { message = ex.Message };
                return result;
            }
        }
    }
}

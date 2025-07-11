using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.CustomerProcess.Clues.Helpers;
using CustomerRelationshipManagement.CustomerProcess.ClueSources;
using CustomerRelationshipManagement.CustomerProcess.Industrys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Industrys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IClues;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.UserRoles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;




namespace CustomerRelationshipManagement.CustomerProcess.Clues
{
    /// <summary>
    /// 线索服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class ClueService : ApplicationService, IClueService
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private readonly IRepository<Clue> repository;
        private readonly IRepository<ClueSource> sourceRepository;
        private readonly IRepository<UserInfo> userRepository;
        private readonly IRepository<RoleInfo> roleRepository;
        private readonly IRepository<UserRoleInfo> userRoleRepository;
        private readonly IRepository<Industry> industryRepository;
        private readonly ILogger<ClueService> logger;
        private readonly IDistributedCache<PageInfoCount<ClueDto>> cache;
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly ICurrentUser _currentUser;


        public ClueService(IRepository<Clue> repository, ILogger<ClueService> logger, IDistributedCache<PageInfoCount<ClueDto>> cache, IRepository<ClueSource> sourceRepository, IRepository<UserInfo> userRepository, IRepository<Industry> industryRepository, IConnectionMultiplexer connectionMultiplexer, ICurrentUser currentUser, IRepository<RoleInfo> roleRepository, IRepository<UserRoleInfo> userRoleRepository)
        {
            this.repository = repository;
            this.logger = logger;
            this.cache = cache;
            this.sourceRepository = sourceRepository;
            this.userRepository = userRepository;
            this.industryRepository = industryRepository;
            this.connectionMultiplexer = connectionMultiplexer;
            _currentUser = currentUser;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 清楚关于c:PageInfo,k的所有信息
        /// </summary>
        /// <returns></returns>
        public async Task ClearAbpCacheAsync()
        {
            var endpoints=connectionMultiplexer.GetEndPoints();
            foreach(var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern:"c:PageInfo,k:*");//填写自己的缓存前缀
                foreach(var key in keys)
                {
                    await connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }

        /// <summary>
        /// 添加线索
        /// </summary>
        /// <param name="dto">要添加的线索信息</param>
        /// <returns>返回受影响行数</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResult<ClueDto>> AddClue(CreateUpdateClueDto dto)
        {
            try
            {
                //将接收到的对象转换成实体
                var clue = ObjectMapper.Map<CreateUpdateClueDto, Clue>(dto);
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
                // 拼接客户编号，格式如C-202506240038-3B7C或X-202506240038-1540
                clue.ClueCode = $"X-{now:yyyyMMddHHmm}-{randomStr}";
                //保存到数据库
                var list=await repository.InsertAsync(clue);
                //清除缓存，确保数据一致性
                await ClearAbpCacheAsync();
                //将数据库操作成功后的CLue实体转换为CLueDto对象
                return ApiResult<ClueDto>.Success(ResultCode.Success, ObjectMapper.Map<Clue, ClueDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError("添加线索信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 显示查询线索信息
        /// </summary>
        /// <param name="pagingInfo">要查询的条件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<ClueDto>>> ShowClue([FromQuery] SearchClueDto dto)
        {
            try
            {
                //构建缓存键名
                string cacheKey = ClueCacheKeyHelper.BuildReadableKey(dto);
                //使用Redis缓存获取或添加数据
                var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
                {
                    var cluelist = await repository.GetQueryableAsync();
                    var sourcelist=await sourceRepository.GetQueryableAsync();
                    var userlist=await userRepository.GetQueryableAsync();  
                    var industrylist=await industryRepository.GetQueryableAsync();
                    var list = from clu in cluelist
                               join source in sourcelist on clu.ClueSourceId equals source.Id into sourceGroup
                               from source in sourceGroup.DefaultIfEmpty()
                               join user in userlist on clu.UserId equals user.Id into userGroup
                               from user in userGroup.DefaultIfEmpty()
                               join industry in industrylist on clu.IndustryId equals industry.Id into industryGroup
                               from industry in industryGroup.DefaultIfEmpty()
                               join creator in userlist on clu.CreatorId equals creator.Id  into creatorGroup
                               from creator in creatorGroup.DefaultIfEmpty()
                               where (
                                   dto.CluePoolStatus == null ||
                                   (dto.CluePoolStatus == 1 && clu.CluePoolStatus == 1) ||
                                   ((dto.CluePoolStatus == 0 || dto.CluePoolStatus == 2) && (clu.CluePoolStatus == 0 || clu.CluePoolStatus == 2))
                               )
                               select new ClueDto
                               {
                                   Id = clu.Id,
                                   UserId = clu.UserId,
                                   UserName=user.UserName,
                                   ClueName = clu.ClueName,
                                   CluePhone = clu.CluePhone,
                                   ClueSourceId = clu.ClueSourceId,
                                   ClueSourceName= source.ClueSourceName,
                                   ClueEmail = clu.ClueEmail,
                                   ClueWechat = clu.ClueWechat,
                                   ClueQQ = clu.ClueQQ,
                                   CompanyName = clu.CompanyName,
                                   IndustryId = clu.IndustryId,
                                   IndustryName = industry.IndustryName,
                                   Address = clu.Address,
                                   Remark = clu.Remark,
                                   Status = clu.Status,
                                   LastFollowTime = clu.LastFollowTime,
                                   NextContactTime = clu.NextContactTime,
                                   CreatorId = clu.CreatorId,
                                   CreateName = creator.RealName,
                                   CreationTime = clu.CreationTime,
                                   ClueCode=clu.ClueCode,
                               };
                    //// 根据 CluePoolStatus 查询
                    //if (dto.CluePoolStatus != null)
                    //{
                    //    list = list.Where(x => x.CluePoolStatus == dto.CluePoolStatus);
                    //}
                    // type: 0=全部，1=我负责的，2=我创建的
                    if (dto.type == 1 && dto.AssignedTo.HasValue)
                    {
                        list = list.Where(x => x.UserId == dto.AssignedTo);
                    }
                    else if (dto.type == 2 && dto.CreatedBy.HasValue)
                    {
                        list = list.Where(x => x.CreatorId == dto.CreatedBy);
                    }
                    //查询条件
                    if (!string.IsNullOrEmpty(dto.Keyword))
                    {
                        list = list.Where(x => x.ClueName.Contains(dto.Keyword)
                                               || x.ClueEmail.Contains(dto.Keyword)
                                               || x.CluePhone.Contains(dto.Keyword)
                                               || x.CompanyName.Contains(dto.Keyword));
                    }
                    //根据状态查询
                    if (dto.Status != null && dto.Status.Count > 0)
                    {
                        list = list.Where(x => dto.Status.Contains(x.Status));
                    }
                    //根据创建人查询
                    if (dto.CreatedBy.HasValue)
                    {
                        list = list.Where(x => x.CreatorId == dto.CreatedBy);
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

                    // 高级筛选字段处理（负责人、创建人多选，线索来源、行业单选）
                    if (dto.MatchMode == 0) // 全部满足(AND)
                    {
                        if (dto.UserIds != null && dto.UserIds.Count > 0)
                            list = list.Where(x => dto.UserIds.Contains(x.UserId));
                        if (dto.CreatedByIds != null && dto.CreatedByIds.Count > 0)
                            list = list.Where(x => x.CreatorId.HasValue && dto.CreatedByIds.Contains(x.CreatorId.Value));
                        if (dto.ClueSourceId != Guid.Empty)
                            list = list.Where(x => x.ClueSourceId == dto.ClueSourceId);
                        if (dto.IndustryId != Guid.Empty)
                            list = list.Where(x => x.IndustryId == dto.IndustryId);
                        if (!string.IsNullOrEmpty(dto.ClueCode))
                            list = list.Where(x => x.ClueCode.Contains(dto.ClueCode));
                        if (!string.IsNullOrEmpty(dto.ClueName))
                            list = list.Where(x => x.ClueName.Contains(dto.ClueName));
                        if (!string.IsNullOrEmpty(dto.CluePhone))
                            list = list.Where(x => x.CluePhone.Contains(dto.CluePhone));
                        if (!string.IsNullOrEmpty(dto.ClueEmail))
                            list = list.Where(x => x.ClueEmail.Contains(dto.ClueEmail));
                        if (!string.IsNullOrEmpty(dto.ClueWechat))
                            list = list.Where(x => x.ClueWechat.Contains(dto.ClueWechat));
                        if (!string.IsNullOrEmpty(dto.ClueQQ))
                            list = list.Where(x => x.ClueQQ.Contains(dto.ClueQQ));
                        if (!string.IsNullOrEmpty(dto.CompanyName))
                            list = list.Where(x => x.CompanyName.Contains(dto.CompanyName));
                        if (!string.IsNullOrEmpty(dto.Address))
                            list = list.Where(x => x.Address.Contains(dto.Address));
                    }
                    else // 部分满足(OR)
                    {
                        list = list.Where(x =>
                            (dto.UserIds != null && dto.UserIds.Count > 0 && dto.UserIds.Contains(x.UserId)) ||
                            (dto.CreatedByIds != null && dto.CreatedByIds.Count > 0 && dto.CreatedByIds.Contains(x.CreatorId.Value)) ||
                            (dto.ClueSourceId != Guid.Empty && x.ClueSourceId == dto.ClueSourceId) ||
                            (dto.IndustryId != Guid.Empty && x.IndustryId == dto.IndustryId) ||
                            (!string.IsNullOrEmpty(dto.ClueCode) && x.ClueCode.Contains(dto.ClueCode)) ||
                            (!string.IsNullOrEmpty(dto.ClueName) && x.ClueName.Contains(dto.ClueName)) ||
                            (!string.IsNullOrEmpty(dto.CluePhone) && x.CluePhone.Contains(dto.CluePhone)) ||
                            (!string.IsNullOrEmpty(dto.ClueEmail) && x.ClueEmail.Contains(dto.ClueEmail)) ||
                            (!string.IsNullOrEmpty(dto.ClueWechat) && x.ClueWechat.Contains(dto.ClueWechat)) ||
                            (!string.IsNullOrEmpty(dto.ClueQQ) && x.ClueQQ.Contains(dto.ClueQQ)) ||
                            (!string.IsNullOrEmpty(dto.CompanyName) && x.CompanyName.Contains(dto.CompanyName)) ||
                            (!string.IsNullOrEmpty(dto.Address) && x.Address.Contains(dto.Address))
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
                    //构建分页结果对象
                    var pageInfo = new PageInfoCount<ClueDto>
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

                return ApiResult<PageInfoCount<ClueDto>>.Success(ResultCode.Success,redislist);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取线索表详情
        /// </summary>
        /// <param name="id">要查看的线索ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<ClueDto>> GetClueById(Guid id)
        {
            try
            {
                var clue = await repository.GetAsync(x => x.Id == id);
                if (clue == null)
                {
                    return ApiResult<ClueDto>.Fail("线索信息不存在", ResultCode.NotFound);
                }
                return ApiResult<ClueDto>.Success(ResultCode.Success, ObjectMapper.Map<Clue, ClueDto>(clue));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 删除线索信息
        /// </summary>
        /// <param name="id">要删除的线索ID</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpDelete]
        public async Task<ApiResult<ClueDto>> DelClue(Guid id)
        {
            try
            {
                var clue=await repository.GetAsync(x=>x.Id==id);
                if(clue==null)
                {
                    return ApiResult<ClueDto>.Fail("未找到删除的线索", ResultCode.NotFound);
                }
                clue.IsDeleted = true; // 设置为已删除状态
                await repository.UpdateAsync(clue);
                return ApiResult<ClueDto>.Success(ResultCode.Success, ObjectMapper.Map<Clue, ClueDto>(clue));
            }
            catch (Exception ex)
            {
                logger.LogError("删除线索信息出错！" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 修改线索信息
        /// </summary>
        /// <param name="dto">要修改的线索信息</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<CreateUpdateClueDto>> UpdClue(Guid id,CreateUpdateClueDto dto)
        {
            try
            {
                var clue=await repository.GetAsync(x=>x.Id==id);
                if (clue == null)
                {
                   return ApiResult<CreateUpdateClueDto>.Fail("未找到要修改的线索", ResultCode.NotFound);
                }
                var clueDto=ObjectMapper.Map(dto,clue);
                await repository.UpdateAsync(clueDto);
                return ApiResult<CreateUpdateClueDto>.Success(ResultCode.Success, ObjectMapper.Map<Clue, CreateUpdateClueDto>(clueDto));
            }
            catch (Exception ex)
            {
                logger.LogError("修改线索信息出错！" + ex.Message);
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
                        IdString = u.Id.ToString(),
                        u.ClueSourceName,
                        u.ClueSourceStatus,
                        u.ClueSourceContent,
                        u.CreateTime
                    })
                    .AsEnumerable()
                    .Where(u => Guid.TryParse(u.IdString, out _))
                    .Select(u => new SourceDto
                    {
                        Id = Guid.Parse(u.IdString),
                        ClueSourceName = u.ClueSourceName,
                        ClueSourceStatus = u.ClueSourceStatus,
                        ClueSourceContent = u.ClueSourceContent,
                        CreateTime = u.CreateTime
                    })
                    .ToList();
                return ApiResult<List<SourceDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception)
            {

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
        /// 获取行业下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<IndustryDto>>> GetIndustrySelectList()
        {
            try
            {

                var industryList = await industryRepository.GetQueryableAsync();
                var result = industryList
                    .Select(u => new
                    {
                        IdString = u.Id.ToString(),
                        u.IndustryName
                    })
                    .AsEnumerable()
                    .Where(u => Guid.TryParse(u.IdString, out _))
                    .Select(u => new IndustryDto
                    {
                        Id = Guid.Parse(u.IdString),
                        IndustryName = u.IndustryName,
                    })
                    .ToList();
                return ApiResult<List<IndustryDto>>.Success(ResultCode.Success, result);
            }
            catch (Exception ex)
            {
                logger.LogError("获取用户下拉框数据出错！" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 处理线索的分配、领取、放弃操作，并返回更新后的线索详情
        /// </summary>
        /// <param name="dto">包含操作类型、线索ID和目标用户ID（可选）的请求参数</param>
        /// <returns>返回更新后的 ClueDto 对象</returns>
        [HttpPut]
        public async Task<ApiResult<CreateUpdateClueDto>> HandleClueActionAsync(ClueActionDto dto)
        {
            // 根据线索 ID 查询线索实体
            var clue = await repository.FirstOrDefaultAsync(x => x.Id == dto.ClueId);
            if (clue == null)
            {
                throw new BusinessException("Clue.NotFound", "未找到对应的线索");
            }

            // 获取当前登录用户的 ID
            // _currentUser 由 ABP 框架自动提供，代表当前已登录用户的信息，常用于应用服务中判断用户身份、ID、租户等。
            var currentUserId = _currentUser.Id;
            if (!currentUserId.HasValue)
            {
                throw new BusinessException("User.NotLoggedIn", "当前用户未登录");
            }

            // 根据操作类型处理不同的逻辑
            switch (dto.ActionType.ToLower())
            {
                case "assign":  // 分配线索给其他人
                    if (clue.CluePoolStatus ==1 ) // 仅公海线索可分配
                        throw new BusinessException("Clue.InvalidStatus", "仅可分配公海线索");

                    if (!dto.TargetUserId.HasValue || dto.TargetUserId == currentUserId) // 校验目标用户
                        throw new BusinessException("Clue.InvalidTargetUser", "目标用户无效，不能是自己");

                    clue.UserId = dto.TargetUserId.Value;   // 设置为目标用户
                    clue.CluePoolStatus = 1;          // 标记为已分配
                    break;

                case "receive":  // 当前用户领取线索
                    if (clue.CluePoolStatus ==1 ) // 仅公海线索可领取
                        throw new BusinessException("Clue.InvalidStatus", "仅可领取公海线索");

                    clue.UserId = currentUserId.Value;      // 设置为当前用户
                    clue.CluePoolStatus = 1;          // 标记为已领取
                    break;

                case "abandon":  // 放弃线索
                    if (clue.CluePoolStatus != 1) // 仅已分配线索可放弃
                        throw new BusinessException("Clue.InvalidStatus", "仅可放弃已分配线索");

                    if (clue.UserId != currentUserId) // 校验是否为本人负责
                        throw new BusinessException("Clue.PermissionDenied", "只能放弃自己负责的线索");

                    clue.UserId = Guid.Empty;               // 去除负责人
                    clue.CluePoolStatus = 2;          // 标记为已放弃
                    break;

                default:
                    throw new BusinessException("Clue.InvalidAction", "不支持的操作类型");
            }

            // 更新线索数据到数据库
            await repository.UpdateAsync(clue);

            // 再次查询，含导航属性（例如：User、ClueSource、Industry 等）
            // WithDetailsAsync() 是 ABP 框架中 IRepository 提供的一个扩展方法。它返回的是一个 IQueryable<Clue>，并且会自动 Include 线索实体的关联导航属性，比如 User（负责人）、ClueSource（线索来源）、Industry（行业）等。
            // 自己理解：准备查询，包含关联的其他表
            var query = await repository.WithDetailsAsync(); // 需要实体配置 WithDetails()
            // 正式查询
            var updatedClue = await query.FirstOrDefaultAsync(x => x.Id == clue.Id);

            // 映射为 DTO 并返回给前端
            var resultDto = ObjectMapper.Map<Clue, CreateUpdateClueDto>(updatedClue);

            return ApiResult<CreateUpdateClueDto>.Success(ResultCode.Success, resultDto);
        }

        /// <summary>
        /// 显示用户列表（用来分配线索）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [HttpGet]
        public async Task<ApiResult<PageInfoCount<GetUserRoleDto>>> ShowUserListAsync([FromQuery] SearchUserDto dto)
        {
            try
            {
                var userQuery = await userRepository.GetQueryableAsync();
                var roleQuery = await roleRepository.GetQueryableAsync();
                var userRoleQuery = await userRoleRepository.GetQueryableAsync();

                var query = from user in userQuery
                            join ur in userRoleQuery on user.Id equals ur.UserId into urGroup
                            from ur in urGroup.DefaultIfEmpty()
                            join role in roleQuery on ur.RoleId equals role.Id into roleGroup
                            from role in roleGroup.DefaultIfEmpty()
                            select new GetUserRoleDto
                            {
                                UserRoleId = ur.Id,
                                UserId = user.Id,
                                RealName = user.RealName,
                                Email = user.Email,
                                PhoneInfo = user.PhoneInfo,
                                RoleId = role != null ? role.Id : Guid.Empty,
                                RoleName = role != null ? role.RoleName : "--"
                            };

                // 关键词搜索（按姓名或手机号）
                if (!string.IsNullOrWhiteSpace(dto.Keyword))
                {
                    query = query.Where(x =>
                        x.RealName.Contains(dto.Keyword) ||
                        x.PhoneInfo.Contains(dto.Keyword));
                }

                // 排序（按创建时间，假如你有时间字段，可以加上）
                query = query.OrderByDescending(x => x.RealName); // 示例按名字排序

                // 分页处理
                var totalCount = query.Count();
                var pagedList = query.Skip((dto.PageIndex - 1) * dto.PageSize).Take(dto.PageSize).ToList();

                var pageInfo = new PageInfoCount<GetUserRoleDto>
                {
                    TotalCount = totalCount,
                    PageCount = (int)Math.Ceiling(totalCount * 1.0 / dto.PageSize),
                    Data = pagedList
                };

                return ApiResult<PageInfoCount<GetUserRoleDto>>.Success(ResultCode.Success, pageInfo);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("用户列表获取失败：" + ex.Message);
            }
        }

    }
}

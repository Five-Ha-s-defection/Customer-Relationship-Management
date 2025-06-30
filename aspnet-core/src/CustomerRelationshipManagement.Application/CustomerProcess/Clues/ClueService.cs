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
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
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
        private readonly IRepository<Industry> industryRepository;
        private readonly ILogger<ClueService> logger;
        private readonly IDistributedCache<PageInfoCount<ClueDto>> cache;

        public ClueService(IRepository<Clue> repository, ILogger<ClueService> logger, IDistributedCache<PageInfoCount<ClueDto>> cache, IRepository<ClueSource> sourceRepository, IRepository<UserInfo> userRepository, IRepository<Industry> industryRepository)
        {
            this.repository = repository;
            this.logger = logger;
            this.cache = cache;
            this.sourceRepository = sourceRepository;
            this.userRepository = userRepository;
            this.industryRepository = industryRepository;
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
                // 设置UserId为随机 GUID
                clue.UserId = Guid.NewGuid();
                //保存到数据库
                var list=await repository.InsertAsync(clue);
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
                               join source in sourcelist
                               on clu.ClueSourceId equals source.Id
                               join user in userlist
                               on clu.UserId equals user.Id
                               join industry in industrylist
                               on clu.IndustryId equals industry.Id
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
                                   NextContactTime = clu.NextContactTime
                               };
                    // 只在type==1且AssignedTo有值时加UserId过滤条件
                    list = list.WhereIf(dto.type == 1 && dto.AssignedTo.HasValue, x => x.UserId == dto.AssignedTo);
                    //查询条件
                    if (!string.IsNullOrEmpty(dto.Keyword))
                    {
                        list = list.Where(x => x.ClueName.Contains(dto.Keyword)
                                               || x.ClueEmail.Contains(dto.Keyword)
                                               || x.CluePhone.Contains(dto.Keyword)
                                               || x.CompanyName.Contains(dto.Keyword));
                    }
                    //根据状态查询
                    if (dto.Status.HasValue)
                    {
                        list = list.Where(x => x.Status == dto.Status.Value);
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
                    SlidingExpiration = TimeSpan.FromMinutes(5)     //设置缓存过期时间为5分钟
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
    }
}

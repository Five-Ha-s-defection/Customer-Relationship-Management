using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using Volo.Abp.Caching;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IClues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;



namespace CustomerRelationshipManagement.CustomerProcess.Clues
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ClueService : ApplicationService, IClueService
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private readonly IRepository<Clue> repository;
        private readonly ILogger<ClueService> logger;
        private readonly IDistributedCache<PageInfoCount<ClueDto>> cache;

        public ClueService(IRepository<Clue> repository, ILogger<ClueService> logger, IDistributedCache<PageInfoCount<ClueDto>> cache)
        {
            this.repository = repository;
            this.logger = logger;
      this.cache = cache;
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
                string cacheKey = "ClueRedis";
                //使用Redis缓存获取或添加数据
                var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
                {
                    var cluelist = await repository.GetQueryableAsync();
                    //查询条件
                    //根据姓名、手机、邮箱、公司名称模糊查询
                    if (!string.IsNullOrEmpty(dto.Keyword))
                    {
                        cluelist = cluelist.Where(x => x.ClueName.Contains(dto.Keyword)
                                               || x.ClueEmail.Contains(dto.Keyword)
                                               || x.CluePhone.Contains(dto.Keyword)
                                               || x.CompanyName.Contains(dto.Keyword));
                    }
                    //根据状态查询
                    if (dto.Status.HasValue)
                    {
                        cluelist = cluelist.Where(x => x.Status == dto.Status.Value);
                    }
                    //根据创建人查询
                    if (dto.CreatedBy.HasValue)
                    {
                        cluelist = cluelist.Where(x => x.CreatorId == dto.CreatedBy);
                    }
                    //根据负责人查询
                    if (dto.AssignedTo.HasValue)
                    {
                        cluelist = cluelist.Where(x => x.UserId == dto.AssignedTo);
                    }
                    // 时间筛选
                    if (dto.StartTime.HasValue && dto.EndTime.HasValue && dto.TimeType.HasValue)
                    {
                        cluelist = dto.TimeType switch
                        {
                            TimeField.CreateTime => cluelist.Where(x => x.CreationTime >= dto.StartTime && x.CreationTime <= dto.EndTime),
                            TimeField.NextContact => cluelist.Where(x => x.NextContactTime >= dto.StartTime && x.NextContactTime <= dto.EndTime),
                            TimeField.LastFollow => cluelist.Where(x => x.LastFollowTime >= dto.StartTime && x.LastFollowTime <= dto.EndTime),
                            _ => cluelist
                        };
                    }

                    // 排序
                    if (dto.OrderBy.HasValue)
                    {
                        cluelist = (dto.OrderBy.Value, dto.OrderDesc) switch
                        {
                            (TimeField.CreateTime, true) => cluelist.OrderByDescending(x => x.CreationTime),
                            (TimeField.CreateTime, false) => cluelist.OrderBy(x => x.CreationTime),

                            (TimeField.NextContact, true) => cluelist.OrderByDescending(x => x.NextContactTime),
                            (TimeField.NextContact, false) => cluelist.OrderBy(x => x.NextContactTime),

                            (TimeField.LastFollow, true) => cluelist.OrderByDescending(x => x.LastFollowTime),
                            (TimeField.LastFollow, false) => cluelist.OrderBy(x => x.LastFollowTime),

                            _ => cluelist.OrderByDescending(x => x.LastFollowTime)
                        };
                    }
                    //用ABP框架的分页
                    var res = cluelist.PageResult(dto.PageIndex, dto.PageSize);
                    //实体列表转换成DTO列表
                    var clueDtos = ObjectMapper.Map<List<Clue>, List<ClueDto>>(res.Queryable.ToList());
                    //构建分页结果对象
                    var pageInfo = new PageInfoCount<ClueDto>
                    {
                        TotalCount = res.RowCount,
                        PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / dto.PageSize),
                        Data = clueDtos
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
    }
}

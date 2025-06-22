using CustomerRelationshipManagement.ApiResult;
using CustomerRelationshipManagement.Dtos.Clues;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;

namespace CustomerRelationshipManagement.Clues
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ClueService : ApplicationService, IClueService
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private readonly IRepository<Clue> repository;
        private readonly ILogger<ClueService> logger;

        public ClueService(IRepository<Clue> repository, ILogger<ClueService> logger)
        {
            this.repository = repository;
            this.logger = logger;
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
        public async Task<ApiResult<PageInfoCount<ClueDto>>> ShowClue([FromQuery] PagingInfo pagingInfo)
        {
            try
            {
                var cluelist = await repository.GetQueryableAsync();
                //查询条件
                //根据姓名、手机、邮箱、公司名称模糊查询
                if (!string.IsNullOrEmpty(pagingInfo.Keyword))
                {
                    cluelist = cluelist.Where(x => x.ClueName.Contains(pagingInfo.Keyword) 
                                           || x.ClueEmail.Contains(pagingInfo.Keyword)
                                           || x.CluePhone.Contains(pagingInfo.Keyword)
                                           || x.CompanyName.Contains(pagingInfo.Keyword));
                }
                //根据状态查询
                if (pagingInfo.Status.HasValue)
                {
                    cluelist = cluelist.Where(x => x.Status == pagingInfo.Status.Value);
                }
                //根据创建人查询
                if (pagingInfo.CreatedBy.HasValue)
                {
                    cluelist = cluelist.Where(x => x.CreatorId == pagingInfo.CreatedBy);
                }
                //根据负责人查询
                if (pagingInfo.AssignedTo.HasValue)
                {
                    cluelist = cluelist.Where(x => x.UserId == pagingInfo.AssignedTo);
                }
                //根据时间查询
                if (pagingInfo.StartTime.HasValue && pagingInfo.EndTime.HasValue)
                {
                    cluelist = pagingInfo.TimeType switch
                    {
                        "CreateTime" => cluelist.Where(x => x.CreationTime >= pagingInfo.StartTime && x. CreationTime <=    pagingInfo.EndTime),
                        "NextContact" => cluelist.Where(x => x.NextContactTime >= pagingInfo.StartTime &&      x.NextContactTime <= pagingInfo.EndTime),
                        _ => cluelist.Where(x => x.LastFollowTime >= pagingInfo.StartTime && x.LastFollowTime <=       pagingInfo.EndTime)
                    };
                }
                //分页
                var totalCount = cluelist.Count();
                var totalPage = (int)Math.Ceiling((double)cluelist.Count() / pagingInfo.PageSize);
                var clues = cluelist.OrderBy(x => x.Id).Skip((pagingInfo.PageIndex - 1) * pagingInfo.PageSize).Take(pagingInfo.PageSize).ToList();
                return ApiResult<PageInfoCount<ClueDto>>.Success(ResultCode.Success, new PageInfoCount<ClueDto>
                {
                    Data = clues.Select(x => ObjectMapper.Map<Clue, ClueDto>(x)).ToList(),
                    PageCount = totalPage,
                    TotalCount = totalCount
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

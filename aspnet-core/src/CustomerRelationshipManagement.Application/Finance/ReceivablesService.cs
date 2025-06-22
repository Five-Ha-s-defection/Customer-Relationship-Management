using CustomerRelationshipManagement.ApiResult;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.Redis;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Finance
{
    /// <summary>
    /// 应收款服务类
    /// 提供应收款的增删改查功能，并集成Redis缓存以提高查询性能
    /// </summary>
    public class ReceivablesService : ApplicationService, IReceivablesService
    {
        /// <summary>
        /// 应收款数据仓储接口
        /// </summary>
        private readonly IRepository<Receivables, Guid> repository;
        
        /// <summary>
        /// Redis缓存服务接口
        /// </summary>
        private readonly IRedisCacheService redisCacheService;

        /// <summary>
        /// 构造函数，注入依赖服务
        /// </summary>
        /// <param name="repository">应收款数据仓储</param>
        /// <param name="redisCacheService">Redis缓存服务</param>
        public ReceivablesService(IRepository<Receivables, Guid> repository,IRedisCacheService redisCacheService)
        {
            this.repository = repository;
            this.redisCacheService = redisCacheService;
        }

        /// <summary>
        /// 添加应收款信息
        /// </summary>
        /// <param name="createUpdateReceibablesDto">应收款创建/更新数据传输对象</param>
        /// <returns>操作结果，包含创建的应收款信息</returns>
        public async Task<ApiResult<ReceivablesDTO>> InsertAsync(CreateUpdateReceibablesDto createUpdateReceibablesDto)
        {
            // 将DTO映射为实体对象
            var receivables = ObjectMapper.Map<CreateUpdateReceibablesDto, Receivables>(createUpdateReceibablesDto);

            // 如果用户没有提供编号，则自动生成；如果提供了，则使用用户提供的
            if (string.IsNullOrEmpty(receivables.ReceivableCode))
            {
                // 自动生成应收款编号：时间-随机四位数字
                var random = new Random();
                var randomNumber = random.Next(1000, 10000); // 生成1000-9999之间的随机数
                var currentTime = DateTime.Now.ToString("yyyyMMdd"); // 格式：20241201
                receivables.ReceivableCode = $"M{currentTime}-{randomNumber}"; // 格式：M20241201-1234
            }
            // 插入数据到数据库
            receivables =await repository.InsertAsync(receivables);

            // 返回成功结果，包含创建的应收款信息
            return ApiResult<ReceivablesDTO>.Success(ResultCode.Success, ObjectMapper.Map<Receivables, ReceivablesDTO>(receivables));
        }

        /// <summary>
        /// 获取应收款分页列表
        /// 使用Redis缓存提高查询性能，支持多种查询条件过滤
        /// </summary>
        /// <param name="receivablesSearchDto">应收款搜索条件</param> 
        /// <returns>分页查询结果</returns>
        public async Task<ApiResult<PageInfoCount<ReceivablesDTO>>> GetPageAsync([FromQuery] ReceivablesSearchDto receivablesSearchDto)
        {
            // 构建缓存键名
            string cacheKey ="Getreceivables";

            // 使用Redis缓存获取或添加数据
            var redislist = await redisCacheService.GetOrAddAsync<PageInfoCount<ReceivablesDTO>>(cacheKey, async () =>
            {
                // 获取查询对象
                var list = await repository.GetQueryableAsync();
                
                // 应收款编号过滤（模糊查询）
                list = list.WhereIf(!string.IsNullOrEmpty(receivablesSearchDto.ReceivableCode), x => x.ReceivableCode.Contains(receivablesSearchDto.ReceivableCode));
                
                // 时间范围过滤 - 开始时间
                list = list.WhereIf(receivablesSearchDto.StartTime.HasValue, x => x.ReceivableDate >= receivablesSearchDto.StartTime);
                
                // 时间范围过滤 - 结束时间
                list = list.WhereIf(receivablesSearchDto.EndTime.HasValue, x => x.ReceivableDate <= receivablesSearchDto.EndTime);

                // 负责人过滤
                list = list.WhereIf(receivablesSearchDto.UserId.HasValue, x => x.UserId == receivablesSearchDto.UserId);

                // 创建人过滤（已注释）
                //list = list.WhereIf(receivablesSearchDto.CreateId.HasValue, x => x.CreateId == receivablesSearchDto.CreateId);

                // 所属客户过滤
                list = list.WhereIf(receivablesSearchDto.CustomerId.HasValue, x => x.CustomerId == receivablesSearchDto.CustomerId);

                // 关联合同过滤
                list = list.WhereIf(receivablesSearchDto.ContractId.HasValue, x => x.ContractId == receivablesSearchDto.ContractId);

                // 应收款金额过滤
                list = list.WhereIf(receivablesSearchDto.ReceivablePay.HasValue, x => x.ReceivablePay == receivablesSearchDto.ReceivablePay);

                // 使用ABP框架的分页方法进行分页查询
                var res = list.PageResult(receivablesSearchDto.PageIndex, receivablesSearchDto.PageSize);

                // 将实体列表转换为DTO列表
                var itemDtos = ObjectMapper.Map<List<Receivables>, List<ReceivablesDTO>>(res.Queryable.ToList());

                // 构建分页结果对象
                var pageInfo = new PageInfoCount<ReceivablesDTO>
                {
                    TotalCount = res.RowCount, // 总记录数
                    PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / receivablesSearchDto.PageSize), // 总页数
                    Data = itemDtos // 当前页数据
                };

                // 返回分页结果（这个return语句很重要，不能缺失）
                return pageInfo;
            }, expiration: TimeSpan.FromMinutes(5)); // 设置缓存过期时间为5分钟
            
            // 返回成功结果
            return ApiResult<PageInfoCount<ReceivablesDTO>>.Success(ResultCode.Success, redislist);
        }
    }
}

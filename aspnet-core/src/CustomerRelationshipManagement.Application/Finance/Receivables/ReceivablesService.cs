using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.FinanceInfo.Finance;
using CustomerRelationshipManagement.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Finance.Receivableses
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
        private readonly IDistributedCache<PageInfoCount<ReceivablesDTO>> _cache;

        private readonly IDistributedCache<ReceivablesDTO> _cacheById;

        /// <summary>
        /// 构造函数，注入依赖服务
        /// </summary>
        /// <param name="repository">应收款数据仓储</param>
        /// <param name="redisCacheService">Redis缓存服务</param>
        public ReceivablesService(IRepository<Receivables, Guid> repository, IDistributedCache<PageInfoCount<ReceivablesDTO>> cache, IDistributedCache<ReceivablesDTO> cacheById)
        {
            this.repository = repository;
            _cache = cache;
            _cacheById = cacheById;
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
            else
            {
                receivables.ReceivableCode = $"M{receivables.ReceivableCode}";
            }
            // 插入数据到数据库
            receivables =await repository.InsertAsync(receivables);

            // 返回成功结果，包含创建的应收款信息
            return  ApiResult<ReceivablesDTO>.Success(ResultCode.Success, ObjectMapper.Map<Receivables, ReceivablesDTO>(receivables));
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
            var redislist = await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                // 获取查询对象
                var list = await repository.GetQueryableAsync();

                // 应收款编号过滤（模糊查询）
                list = list.WhereIf(!string.IsNullOrEmpty(receivablesSearchDto.ReceivableCode), x => x.ReceivableCode.Contains(receivablesSearchDto.ReceivableCode))
                .WhereIf(receivablesSearchDto.StartTime.HasValue, x => x.ReceivableDate >= receivablesSearchDto.StartTime)
                .WhereIf(receivablesSearchDto.EndTime.HasValue, x => x.ReceivableDate <= receivablesSearchDto.EndTime)
                .WhereIf(receivablesSearchDto.UserId.HasValue, x => x.UserId == receivablesSearchDto.UserId)
                .WhereIf(receivablesSearchDto.CustomerId.HasValue, x => x.CustomerId == receivablesSearchDto.CustomerId)
                .WhereIf(receivablesSearchDto.ContractId.HasValue, x => x.ContractId == receivablesSearchDto.ContractId)
                .WhereIf(receivablesSearchDto.ReceivablePay.HasValue, x => x.ReceivablePay == receivablesSearchDto.ReceivablePay);

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
                return pageInfo;
            }, () => new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
            }); // 设置缓存过期时间为5分钟
            
            // 返回成功结果
            return ApiResult<PageInfoCount<ReceivablesDTO>>.Success(ResultCode.Success, redislist);
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<ReceivablesDTO>> GetByIdAsync(Guid id)
        {
            // 构建缓存键名
            string cacheKey = $"receivable:{id}";
            
            var receivablesDto = await _cacheById.GetOrAddAsync(
                cacheKey,
                async () => {
                    var query = await repository.GetAsync(id);
                    return ObjectMapper.Map<Receivables, ReceivablesDTO>(query);
                },
                () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                }
            );

            if (receivablesDto == null)
            {
                return ApiResult<ReceivablesDTO>.Fail("未找到该数据", ResultCode.NotFound);
            }

            return ApiResult<ReceivablesDTO>.Success(ResultCode.Success, receivablesDto);
        }
        /// <summary>
        /// 修改应收款的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUpdateReceibablesDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<ReceivablesDTO>> UpdateAsync(Guid id, CreateUpdateReceibablesDto createUpdateReceibablesDto)
        {
            // 1. 获取数据库中最新的、带有正确ConcurrencyStamp的实体
            var receivables = await repository.GetAsync(id);
            if (receivables == null)
            {
                return ApiResult<ReceivablesDTO>.Fail("未找到该数据", ResultCode.NotFound);
            }
            
            // 2. 将传入的DTO中的属性值，更新到从数据库查出的实体上
            ObjectMapper.Map(createUpdateReceibablesDto, receivables);

            // 3. 更新实体，此时Id和ConcurrencyStamp都是正确的
            await repository.UpdateAsync(receivables);

            // 4. 清除相关缓存
            //await ClearReceivablesCacheAsync();
            await _cacheById.RemoveAsync($"receivable:{id}");
            
            // 5. 返回更新后的数据
            return ApiResult<ReceivablesDTO>.Success(ResultCode.Success, ObjectMapper.Map<Receivables, ReceivablesDTO>(receivables));
        }
        /// <summary>
        /// 删除应收款的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<ReceivablesDTO>> DeleteAsync(Guid id)
        {
            try
            {
                await repository.DeleteAsync(id);
                
                await _cacheById.RemoveAsync($"receivable:{id}");

                return ApiResult<ReceivablesDTO>.Success(ResultCode.Success, null);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.crmcontracts;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.DTOS.Finance.Receibableses;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Helper;
using CustomerRelationshipManagement.Interfaces.IFinance.Receivableses;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Finance.Receivableses
{
    [ApiExplorerSettings(GroupName = "v1")]
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
        private readonly IRepository<Payment, Guid> paymentrepository;
        private readonly IRepository<UserInfo, Guid> userinforeceivables;
        private readonly IRepository<Customer, Guid> customerrepository;
        private readonly IRepository<CrmContract, Guid> crmcontractreceivables;

        private readonly IConnectionMultiplexer connectionMultiplexer;

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
        public ReceivablesService(IRepository<Receivables, Guid> repository, IDistributedCache<PageInfoCount<ReceivablesDTO>> cache, IDistributedCache<ReceivablesDTO> cacheById, IRepository<Payment, Guid> paymentrepository, IRepository<UserInfo, Guid> userinforeceivables, IRepository<Customer, Guid> customerrepository, IRepository<CrmContract, Guid> crmcontractreceivables, IConnectionMultiplexer connectionMultiplexer)
        {
            this.repository = repository;
            _cache = cache;
            _cacheById = cacheById;
            this.paymentrepository = paymentrepository;
            this.userinforeceivables = userinforeceivables;
            this.customerrepository = customerrepository;
            this.crmcontractreceivables = crmcontractreceivables;
            this.connectionMultiplexer = connectionMultiplexer;
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
            await ClearAbpCacheAsync();
            // 构建缓存键名
            string cacheKey ="Getreceivables";

            // 使用Redis缓存获取或添加数据
            var redislist = await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                // 获取查询对象
                var payments = await paymentrepository.GetQueryableAsync();
                var receivables = await repository.GetQueryableAsync();
                var userinfo = await userinforeceivables.GetQueryableAsync();
                var customer = await customerrepository.GetQueryableAsync();
                var crmcontract = await crmcontractreceivables.GetQueryableAsync();

                // 联合查询
                var query = from r in receivables
                            join p in payments on r.Id equals p.ReceivableId into pr
                            from p in pr.DefaultIfEmpty() // left join，如果要inner join去掉DefaultIfEmpty
                            join c in userinfo on r.UserId equals c.Id into pc
                            from c in pc.DefaultIfEmpty()
                            join d in customer on r.CustomerId equals d.Id into cd
                            from d in cd.DefaultIfEmpty()
                            join e in crmcontract on r.ContractId equals e.Id into de
                            from e in de.DefaultIfEmpty()
                            join creator in userinfo on r.CreatorId equals creator.Id into creatorJoin
                            from creator in creatorJoin.DefaultIfEmpty()
                            select new ReceivablesDTO
                            {
                                Id = r.Id, // 用应收款Id
                                ReceivableCode = r.ReceivableCode,
                                ReceivablePay = r.ReceivablePay,
                                ReceivableDate = r.ReceivableDate,
                                CustomerId = r.CustomerId,
                                CustomerName = d != null ? d.CustomerName : string.Empty,
                                ContractId = r.ContractId,
                                ContractName = e != null ? e.ContractName : string.Empty,
                                CreationTime = r.CreationTime,
                                UserId = r.UserId,
                                RealName = c != null ? c.RealName : string.Empty,
                                PaymentId = r.PaymentId,
                                Amount = p != null ? p.Amount : 0m,
                                CreatorId = r.CreatorId,
                                CreatorRealName = creator.UserName,

                            };

                // 过滤条件
                query = query.WhereIf(!string.IsNullOrEmpty(receivablesSearchDto.ReceivableCode), x => x.ReceivableCode.Contains(receivablesSearchDto.ReceivableCode))
                    .WhereIf(receivablesSearchDto.StartTime.HasValue, x => x.ReceivableDate >= receivablesSearchDto.StartTime.Value)
                    .WhereIf(receivablesSearchDto.EndTime.HasValue, x => x.ReceivableDate <= receivablesSearchDto.EndTime.Value.AddDays(1))
                    .WhereIf(receivablesSearchDto.UserId.HasValue, x => x.UserId == receivablesSearchDto.UserId.Value)
                    .WhereIf(receivablesSearchDto.CustomerId.HasValue, x => x.CustomerId == receivablesSearchDto.CustomerId.Value)
                    .WhereIf(receivablesSearchDto.ContractId.HasValue, x => x.ContractId == receivablesSearchDto.ContractId.Value)
                    .WhereIf(receivablesSearchDto.CreatorId.HasValue, x => x.CreatorId == receivablesSearchDto.CreatorId.Value)
                    .WhereIf(receivablesSearchDto.ReceivableDate.HasValue, x =>x.ReceivableDate >= receivablesSearchDto.ReceivableDate.Value &&x.ReceivableDate < receivablesSearchDto.ReceivableDate.Value.AddDays(1))
                    .WhereIf(!string.IsNullOrEmpty(receivablesSearchDto.ReceivableCode), x => x.ReceivableCode.Contains(receivablesSearchDto.ReceivableCode));

                // 使用ABP框架的分页方法进行分页查询
                var res = query.PageResult(receivablesSearchDto.PageIndex, receivablesSearchDto.PageSize);

                // 构建分页结果对象
                var pageInfo = new PageInfoCount<ReceivablesDTO>
                {
                    TotalCount = res.RowCount, // 总记录数
                    PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / receivablesSearchDto.PageSize), // 总页数
                    Data = res.Queryable.ToList() // 当前页数据
                };
                return pageInfo;
            }, () => new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
            }); // 设置缓存过期时间为10分钟
            
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
            await ClearAbpCacheAsync();

            // 5. 返回更新后的数据
            return ApiResult<ReceivablesDTO>.Success(ResultCode.Success, ObjectMapper.Map<Receivables, ReceivablesDTO>(receivables));
        }
        /// <summary>
        /// 批量删除应收款信息
        /// </summary>
        /// <param name="ids">要删除的应收款ID数组</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult<ReceivablesDTO>> DeleteAsync([FromBody]Guid[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    return ApiResult<ReceivablesDTO>.Fail("请选择要删除的数据", ResultCode.Fail);
                }

                // 批量删除数据库记录
                foreach (var id in ids)
                {
                    await repository.DeleteAsync(id);
                    // 清除对应的缓存
                    await ClearAbpCacheAsync();
                }

                return ApiResult<ReceivablesDTO>.Success(ResultCode.Success, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult<ReceivablesDTO>> DeleteGetIdAsync(Guid id)
        {
            try
            {
                await repository.DeleteAsync(id);
                // 清除对应的缓存
                await ClearAbpCacheAsync();

                return ApiResult<ReceivablesDTO>.Success(ResultCode.Success, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 导出应收款信息
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetExportReceivablesAsyncExcel()
        {
            // 从数据库查询所有产品
            // 获取查询对象
            var payments = await paymentrepository.GetQueryableAsync();
            var receivables = await repository.GetQueryableAsync();
            var userinfo = await userinforeceivables.GetQueryableAsync();
            var customer = await customerrepository.GetQueryableAsync();
            var crmcontract = await crmcontractreceivables.GetQueryableAsync();

            // 联合查询
            var query = from r in receivables
                        join p in payments on r.Id equals p.ReceivableId into pr
                        from p in pr.DefaultIfEmpty() // left join，如果要inner join去掉DefaultIfEmpty
                        join c in userinfo on r.UserId equals c.Id into pc
                        from c in pc.DefaultIfEmpty()
                        join d in customer on r.CustomerId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        join e in crmcontract on r.ContractId equals e.Id into de
                        from e in de.DefaultIfEmpty()
                        join creator in userinfo on r.CreatorId equals creator.Id into creatorJoin
                        from creator in creatorJoin.DefaultIfEmpty()
                        select new ReceivablesDTO
                        {
                            Id = r.Id, // 用应收款Id
                            ReceivableCode = r.ReceivableCode,
                            ReceivablePay = r.ReceivablePay,
                            ReceivableDate = r.ReceivableDate,
                            CustomerId = r.CustomerId,
                            CustomerName = d.CustomerName,
                            ContractId = r.ContractId,
                            ContractName = e != null ? e.ContractName : string.Empty,
                            UserId = r.UserId,
                            RealName = c != null ? c.RealName : string.Empty,
                            CreationTime = r.CreationTime,
                            CreatorRealName = creator.UserName,
                        };
            var data = query.ToList();

            var headers = new List<string>
            {
                "应收款编号",
                "应收款金额",
                "应收款时间",
                "所属客户",
                "关联合同",
                "负责人"
            };
            // 定义每列对应的属性选择器（可自定义格式）
            var selectors = new List<Func<ReceivablesDTO, object>>
            {
                p => p.ReceivableCode,
                p => p.ReceivablePay,
                p => p.ReceivableDate.ToString("yyyy-MM-dd"),
                p => p.CustomerName,
                p => p.ContractName,
                p => p.RealName
            };

            // 调用通用导出方法，生成Excel字节流
            var fileBytes = ExcelExportHelper.ExportToExcel(data, headers, selectors, "应收款信息");

            // 返回文件流，浏览器会自动下载
            return new FileContentResult(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "应收款信息.xlsx"
            };
        }

        /// <summary>
        /// 清除关于c:PageInfo,k的所有信息
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
    }
}

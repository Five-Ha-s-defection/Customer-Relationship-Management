using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivableses;
using CustomerRelationshipManagement.Interfaces.IFinance.Payments;
using CustomerRelationshipManagement.Paging;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Finance.Payments
{
    public class PaymentService:ApplicationService, IPaymentService
    {
        private readonly IRepository<Payment, Guid> repository;
        private readonly IRepository<Receivables, Guid> receivablesRepository;
        private readonly IDistributedCache<PageInfoCount<PaymentDTO>> cache;

        public PaymentService(IRepository<Payment, Guid> repository, IDistributedCache<PageInfoCount<PaymentDTO>> cache, IRepository<Receivables, Guid> receivablesRepository)
        {
            this.repository = repository;
            this.cache = cache;
            this.receivablesRepository = receivablesRepository;
        }
        /// <summary>
        /// 新增收款
        /// </summary>
        /// <param name="createUpdatePaymentDTO"></param>
        /// <returns></returns>
        public async Task<ApiResult<PaymentDTO>> InsertPayment(CreateUpdatePaymentDTO createUpdatePaymentDTO)
        {
            var payment = ObjectMapper.Map<CreateUpdatePaymentDTO, Payment>(createUpdatePaymentDTO);

            if (string.IsNullOrEmpty(payment.PaymentCode))
            {
                Random random = new Random();
                payment.PaymentCode = $"R{DateTime.Now.ToString("yyyyMMdd")}-{random.Next(1000, 10000)}";
            }
            else
            {
                payment.PaymentCode = $"R{payment.PaymentCode}";
            }
            await repository.InsertAsync(payment);
            
            return ApiResult<PaymentDTO>.Success(ResultCode.Success, ObjectMapper.Map<Payment, PaymentDTO>(payment));
        }
        /// <summary>
        /// 显示分页查询收款列表
        /// </summary>
        /// <param name="searchDTO"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageInfoCount<PaymentDTO>>> GetPayment(PaymentSearchDTO searchDTO)
        {
            string cacheKey = $"GetPayment";
            var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var payments = await repository.GetQueryableAsync();
                var receivables = await receivablesRepository.GetQueryableAsync();

                // 联合查询
                var query = from p in payments
                            join r in receivables on p.ReceivableId equals r.Id into pr
                            from r in pr.DefaultIfEmpty() // left join，如果要inner join去掉DefaultIfEmpty
                            select new PaymentDTO
                            {
                                Id = p.Id,
                                PaymentCode = p.PaymentCode,
                                Amount = p.Amount,
                                PaymentMethod = p.PaymentMethod,
                                PaymentDate = p.PaymentDate,
                                ApproverId = p.ApproverId,
                                PaymentStatus = p.PaymentStatus,
                                Remark = p.Remark,
                                UserId = p.UserId,
                                CustomerId = p.CustomerId,
                                ContractId = p.ContractId,
                                ReceivableId = p.ReceivableId,
                                ReceivablePay = r.ReceivablePay,
                            };
                // 这里可以加上你的where条件，对p.xxx和r.xxx都可以筛选
                query = query
                    .WhereIf(!string.IsNullOrEmpty(searchDTO.PaymentCode), x => x.PaymentCode.Contains(searchDTO.PaymentCode))
                    .WhereIf(searchDTO.PaymentStatus != 0, x => x.PaymentStatus == searchDTO.PaymentStatus)
                    .WhereIf(searchDTO.PaymentMethod.HasValue, x => x.PaymentMethod == searchDTO.PaymentMethod)
                    .WhereIf(searchDTO.PaymentDate != null, x => x.PaymentDate >= searchDTO.StartTime && x.PaymentDate <= searchDTO.EndTime)
                    .WhereIf(searchDTO.UserId.HasValue, x => x.UserId == searchDTO.UserId)
                    .WhereIf(searchDTO.CustomerId.HasValue, x => x.CustomerId == searchDTO.CustomerId)
                    .WhereIf(searchDTO.ContractId.HasValue, x => x.ContractId == searchDTO.ContractId)
                    .WhereIf(searchDTO.ApproverId.HasValue, x => x.ApproverId == searchDTO.ApproverId);
        
                // 使用ABP框架的分页方法进行分页查询
                        var res = query.PageResult(searchDTO.PageIndex, searchDTO.PageSize);


                // 构建分页结果对象
                var pageInfo = new PageInfoCount<PaymentDTO>
                {
                    TotalCount = res.RowCount, // 总记录数
                    PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / searchDTO.PageSize), // 总页数
                    Data = res.Queryable.ToList() // 当前页数据
                };
                return pageInfo;
            }, () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });
            return ApiResult<PageInfoCount<PaymentDTO>>.Success(ResultCode.Success, redislist);
        }
        /// <summary>
        /// 修改收款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUpdatePaymentDTO"></param>
        /// <returns></returns>
        public async Task<ApiResult<PaymentDTO>> UpdatePayment(Guid id,CreateUpdatePaymentDTO createUpdatePaymentDTO)
        {
            var payment = await repository.GetAsync(id);
            if(payment == null)
            {
                return ApiResult<PaymentDTO>.Fail("未找到该数据",ResultCode.NotFound);
            }
            ObjectMapper.Map(createUpdatePaymentDTO, payment);
            await repository.UpdateAsync(payment);
            return ApiResult<PaymentDTO>.Success(ResultCode.Success, ObjectMapper.Map<Payment, PaymentDTO>(payment));
        }

        /// <summary>
        /// 删除收款的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<PaymentDTO>> DeleteAsync(Guid id)
        {
            try
            {
                await repository.DeleteAsync(id);

                return ApiResult<PaymentDTO>.Success(ResultCode.Success, null);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 通过Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<PaymentDTO>> GetByIdAsync(Guid id)
        {
            var query = await repository.GetAsync(id);
            if(query == null)
            {
                return ApiResult<PaymentDTO>.Fail( "未找到该数据",ResultCode.NotFound);
            }
            return ApiResult<PaymentDTO>.Success(ResultCode.Success, ObjectMapper.Map<Payment, PaymentDTO>(query));
        }
    }
}

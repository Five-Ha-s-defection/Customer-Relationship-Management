using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.FinanceInfo.Payments;
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

namespace CustomerRelationshipManagement.Payments
{
    public class PaymentService:ApplicationService, IPaymentService
    {
        private readonly IRepository<Payment, Guid> repository;
        private readonly IDistributedCache<PageInfoCount<PaymentDTO>> cache;

        public PaymentService(IRepository<Payment, Guid> repository, IDistributedCache<PageInfoCount<PaymentDTO>> cache)
        {
            this.repository = repository;
            this.cache = cache;
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
        
        public async Task<ApiResult<PageInfoCount<PaymentDTO>>> GetPayment(PaymentSearchDTO searchDTO)
        {
            string cacheKey = $"GetPayment";
            var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var query = await repository.GetQueryableAsync();
                query = query.WhereIf(!string.IsNullOrEmpty(searchDTO.PaymentCode), x => x.PaymentCode.Contains(searchDTO.PaymentCode))
                    .WhereIf(searchDTO.PaymentStatus != 0, x => x.PaymentStatus == searchDTO.PaymentStatus)
                    .WhereIf(searchDTO.PaymentMethod.HasValue, x => x.PaymentMethod == searchDTO.PaymentMethod)
                    .WhereIf(searchDTO.PaymentDate != null, x => x.PaymentDate >= searchDTO.StartTime && x.PaymentDate <= searchDTO.EndTime)
                     .WhereIf(searchDTO.UserId.HasValue, x => x.UserId == searchDTO.UserId)
                    .WhereIf(searchDTO.CustomerId.HasValue, x => x.CustomerId == searchDTO.CustomerId)
                    .WhereIf(searchDTO.ContractId.HasValue, x => x.ContractId == searchDTO.ContractId)
                    .WhereIf(searchDTO.ApproverId.HasValue, x => x.ApproverId == searchDTO.ApproverId);

                // 使用ABP框架的分页方法进行分页查询
                var res = query.PageResult(searchDTO.PageIndex, searchDTO.PageSize);

                // 将实体列表转换为DTO列表
                var itemDtos = ObjectMapper.Map<List<Payment>, List<PaymentDTO>>(res.Queryable.ToList());

                // 构建分页结果对象
                var pageInfo = new PageInfoCount<PaymentDTO>
                {
                    TotalCount = res.RowCount, // 总记录数
                    PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / searchDTO.PageSize), // 总页数
                    Data = itemDtos // 当前页数据
                };
                return pageInfo;
            }, () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });
            return ApiResult<PageInfoCount<PaymentDTO>>.Success(ResultCode.Success, redislist);
        }
    }
}

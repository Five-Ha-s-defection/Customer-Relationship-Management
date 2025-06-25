using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.Finance.Incoices;
using CustomerRelationshipManagement.Interfaces.IFinance.Invoices;
using CustomerRelationshipManagement.Paging;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Finance.Invoices
{
    public class InvoiceService : ApplicationService, IInvoiceService
    {
        private readonly IRepository<Invoice, Guid> repository;
        private readonly IDistributedCache<PageInfoCount<InvoiceDTO>> cache;

        public InvoiceService(IRepository<Invoice,Guid> repository,IDistributedCache<PageInfoCount<InvoiceDTO>> cache)
        {
            this.repository = repository;
            this.cache = cache;
        }
        /// <summary>
        /// 新增发票
        /// </summary>
        /// <param name="createUpdateInvoiceDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<InvoiceDTO>> InvoiceAsync(CreateUpdateInvoiceDto createUpdateInvoiceDto)
        {
            var invoice = ObjectMapper.Map<CreateUpdateInvoiceDto, Invoice>(createUpdateInvoiceDto);
            if (string.IsNullOrEmpty(invoice.InvoiceNumberCode))
            {
                Random random = new Random();
                invoice.InvoiceNumberCode = $"NO{DateTime.Now.ToString("yyyyMMdd")}-{random.Next(1000, 10000)}";
            }
            else
            {
                invoice.InvoiceNumberCode = $"NO{invoice.InvoiceNumberCode}";
            }
            await repository.InsertAsync(invoice);
            return ApiResult<InvoiceDTO>.Success(ResultCode.Success, ObjectMapper.Map<Invoice, InvoiceDTO>(invoice));
        }
        

        public async Task<ApiResult<PageInfoCount<InvoiceDTO>>> GetInvoiceListAsync(InvoiceSearchDto invoiceSearchDto)
        {
            var cacheKey = $"InvoiceList";
            var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var invoice = await repository.GetQueryableAsync();
                invoice = invoice.WhereIf(!string.IsNullOrEmpty(invoiceSearchDto.InvoiceNumberCode), x => x.InvoiceNumberCode.Contains(invoiceSearchDto.InvoiceNumberCode))
                    .WhereIf(invoiceSearchDto.InvoiceStatus != null, x => x.InvoiceStatus == invoiceSearchDto.InvoiceStatus)
                    .WhereIf(invoiceSearchDto.InvoiceType != null, x => x.InvoiceType == invoiceSearchDto.InvoiceType)
                    .WhereIf(invoiceSearchDto.InvoiceDate != null, x => x.InvoiceDate >= invoiceSearchDto.StartTime && x.InvoiceDate <= invoiceSearchDto.EndTime)
                    .WhereIf(invoiceSearchDto.CustomerId.HasValue, x => x.CustomerId == invoiceSearchDto.CustomerId)
                    .WhereIf(invoiceSearchDto.ContractId.HasValue, x => x.ContractId == invoiceSearchDto.ContractId)
                    .WhereIf(invoiceSearchDto.ApproverId.HasValue, x => x.ApproverId == invoiceSearchDto.ApproverId);

                var res = invoice.PageResult(invoiceSearchDto.PageIndex, invoiceSearchDto.PageSize);

                var itemDtos = ObjectMapper.Map<List<Invoice>, List<InvoiceDTO>>(res.Queryable.ToList());
                var pageInfo = new PageInfoCount<InvoiceDTO>
                {
                    TotalCount = res.RowCount,
                    PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / invoiceSearchDto.PageSize),
                    Data = itemDtos
                };
                return pageInfo;
            }, () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            });

            return ApiResult<PageInfoCount<InvoiceDTO>>.Success(ResultCode.Success, redislist);
        }

        public async Task<ApiResult<InvoiceDTO>> UpdateInvoiceAsync(Guid id, CreateUpdateInvoiceDto createUpdateInvoiceDto)
        {
            var invoicce = await repository.GetAsync(id);
            if (invoicce == null)
            {
                return ApiResult<InvoiceDTO>.Fail("未找到该数据", ResultCode.NotFound);
            }
            ObjectMapper.Map(createUpdateInvoiceDto, invoicce);
            await repository.UpdateAsync(invoicce);
            return ApiResult<InvoiceDTO>.Success(ResultCode.Success, ObjectMapper.Map<Invoice, InvoiceDTO>(invoicce));
        }

        public async Task<ApiResult<InvoiceDTO>> DeleteInvoiceAsync(Guid id)
        {
            var invoice =await repository.GetAsync(id);
            if (invoice == null)
            {
                return ApiResult<InvoiceDTO>.Fail("未找到该数据", ResultCode.NotFound);
            }
            repository.DeleteAsync(invoice);
            return ApiResult<InvoiceDTO>.Success(ResultCode.Success, ObjectMapper.Map<Invoice, InvoiceDTO>(invoice));
        }

        public async Task<ApiResult<InvoiceDTO>> GetInvoiceByIdAsync(Guid id)
        {
            var invoice =await repository.GetAsync(id);
            if (invoice == null)
            {
                return ApiResult<InvoiceDTO>.Fail("未找到该数据", ResultCode.NotFound);
            }
            return ApiResult<InvoiceDTO>.Success(ResultCode.Success, ObjectMapper.Map<Invoice, InvoiceDTO>(invoice));
        }
    }
}

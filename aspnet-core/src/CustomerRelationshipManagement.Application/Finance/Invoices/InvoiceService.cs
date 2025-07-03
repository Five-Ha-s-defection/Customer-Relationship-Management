using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.crmcontracts;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.DTOS.Finance.Incoices;
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.Finance.PaymentMethods;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivableses;
using CustomerRelationshipManagement.Interfaces.IFinance.Invoices;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Users;
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

namespace CustomerRelationshipManagement.Finance.Invoices
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class InvoiceService : ApplicationService, IInvoiceService
    {
        private readonly IRepository<Invoice, Guid> repository;
        private readonly IRepository<UserInfo, Guid> userinforeceivables;
        private readonly IRepository<Customer, Guid> customerrepository;
        private readonly IRepository<CrmContract, Guid> crmcontractreceivables;
        private readonly IDistributedCache<PageInfoCount<InvoiceDTO>> cache;

        public InvoiceService(IRepository<Invoice, Guid> repository, IDistributedCache<PageInfoCount<InvoiceDTO>> cache, IRepository<CrmContract, Guid> crmcontractreceivables, IRepository<UserInfo, Guid> userinforeceivables, IRepository<Customer, Guid> customerrepository)
        {
            this.repository = repository;
            this.cache = cache;
            this.crmcontractreceivables = crmcontractreceivables;
            this.userinforeceivables = userinforeceivables;
            this.customerrepository = customerrepository;
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
        
        /// <summary>
        /// 查询发票列表
        /// </summary>
        /// <param name="invoiceSearchDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageInfoCount<InvoiceDTO>>> GetInvoiceListAsync(InvoiceSearchDto invoiceSearchDto)
        {
            var cacheKey = $"InvoiceList";
            var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var invoice = await repository.GetQueryableAsync();
                var userinfo = await userinforeceivables.GetQueryableAsync();
                var customer = await customerrepository.GetQueryableAsync();
                var crmcontract = await crmcontractreceivables.GetQueryableAsync();// 联合查询
                var query = from i in invoice
                            join c in userinfo on i.UserId equals c.Id into rc
                            from c in rc.DefaultIfEmpty()// left join，如果要inner join去掉DefaultIfEmpty
                            join d in customer on i.CustomerId equals d.Id into id
                            from d in id.DefaultIfEmpty()
                            join e in crmcontract on i.ContractId equals e.Id into re
                            from e in re.DefaultIfEmpty()
                            join creator in userinfo on i.CreatorId equals creator.Id into creatorJoin
                            from creator in creatorJoin.DefaultIfEmpty()
                            join io in invoice on i.InvoiceInformationId equals io.Id into ie
                            from io in ie.DefaultIfEmpty()
                            select new InvoiceDTO
                            {
                                Id = i.Id,
                                CustomerId = i.CustomerId,
                                ContractId = i.ContractId,
                                UserId = i.UserId,
                                RealName = c.RealName,
                                InvoiceNumberCode = i.InvoiceNumberCode,
                                Amount = i.Amount,
                                InvoiceDate = i.InvoiceDate,
                                InvoiceType = i.InvoiceType,
                                ApproverIds = i.ApproverIds,
                                CurrentStep = i.CurrentStep,
                                ApproveComments = i.ApproveComments,
                                ApproveTimes = i.ApproveTimes,
                                InvoiceStatus = i.InvoiceStatus,
                                CustomerName = d.CustomerName,
                                ContractName = e.ContractName,
                                CreatorRealName = creator.RealName,
                                Title = i.Title,
                                TaxNumber = i.TaxNumber,
                                Bank = i.Bank,
                                BillingAddress = i.BillingAddress,
                                BankAccount = i.BankAccount,
                                BillingPhone = i.BillingPhone,
                                InvoiceImg = i.InvoiceImg,
                                Remark = i.Remark,
                                InoviceTitle = io.Title
                                
                            };
                query = query.WhereIf(!string.IsNullOrEmpty(invoiceSearchDto.InvoiceNumberCode), x => x.InvoiceNumberCode.Contains(invoiceSearchDto.InvoiceNumberCode))
                    .WhereIf(invoiceSearchDto.InvoiceStatus != null, x => x.InvoiceStatus == invoiceSearchDto.InvoiceStatus)
                    .WhereIf(invoiceSearchDto.InvoiceType != null, x => x.InvoiceType == invoiceSearchDto.InvoiceType)
                    .WhereIf(invoiceSearchDto.InvoiceDate != null, x => x.InvoiceDate >= invoiceSearchDto.StartTime && x.InvoiceDate <= invoiceSearchDto.EndTime)
                    .WhereIf(invoiceSearchDto.CustomerId.HasValue, x => x.CustomerId == invoiceSearchDto.CustomerId)
                    .WhereIf(invoiceSearchDto.ContractId.HasValue, x => x.ContractId == invoiceSearchDto.ContractId);

                var res = query.PageResult(invoiceSearchDto.PageIndex, invoiceSearchDto.PageSize);

                // 先ToList，后处理AuditorNames
                var userList = userinfo.ToList();
                var dataList = res.Queryable.ToList();
                foreach (var item in dataList)
                {
                    if (item.ApproverIds != null && item.ApproverIds.Count > 0)
                    {
                        item.AuditorNames = string.Join(",", userList.Where(u => item.ApproverIds.Contains(u.Id)).Select(u => u.RealName));
                    }
                    else
                    {
                        item.AuditorNames = string.Empty;
                    }
                }

                var pageInfo = new PageInfoCount<InvoiceDTO>
                {
                    TotalCount = res.RowCount,
                    PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / invoiceSearchDto.PageSize),
                    Data = dataList
                };
                return pageInfo;
            }, () => new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });

            return ApiResult<PageInfoCount<InvoiceDTO>>.Success(ResultCode.Success, redislist);
        }



        public async Task<ApiResult> Approve(Guid id, Guid approverId, bool isPass, string comment)
        {
            // 获取收款记录
            var invoice = await repository.GetAsync(id);
            if (invoice == null)
                return ApiResult.Fail("未找到该收款记录", ResultCode.NotFound);

            // 判断审批是否已结束（2=全部通过，3=拒绝）
            if (invoice.InvoiceStatus == 2 || invoice.InvoiceStatus == 3)
                return ApiResult.Fail("审批已结束", ResultCode.NotFound);

            // 判断是否有审批人或审批流程是否已结束
            if (invoice.ApproverIds.Count == 0 || invoice.CurrentStep >= invoice.ApproverIds.Count)
                return ApiResult.Fail("无审批人或审批流程已结束", ResultCode.NotFound);

            // 获取当前应审批人
            var currentApprover = invoice.ApproverIds[invoice.CurrentStep];
            if (currentApprover != approverId)
                return ApiResult.Fail("当前不是你的审批环节", ResultCode.NotFound);

            // 记录审批意见和时间
            invoice.ApproveComments.Add(comment);
            invoice.ApproveTimes.Add(DateTime.Now);

            if (!isPass)
            {
                // 审批拒绝
                invoice.InvoiceStatus = 3;
            }
            else
            {
                // 审批通过，进入下一个审批环节
                invoice.CurrentStep++;
                if (invoice.CurrentStep >= invoice.ApproverIds.Count)
                {
                    // 所有审批人已通过
                    invoice.InvoiceStatus = 2;
                }
                else
                {
                    // 仍处于审核中
                    invoice.InvoiceStatus = 1;
                }
            }

            // 更新收款记录
            await repository.UpdateAsync(invoice);
            return ApiResult.Success(ResultCode.Success);
        }

        /// <summary>
        /// 修改发票信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="createUpdateInvoiceDto"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 删除发票信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        // 获取发票通过id
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

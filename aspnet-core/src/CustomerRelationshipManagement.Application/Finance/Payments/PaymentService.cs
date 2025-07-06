using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.crmcontracts;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.DTOS.Finance.Receibableses;
using CustomerRelationshipManagement.Finance.PaymentMethods;
using CustomerRelationshipManagement.Finance.Receivableses;
using CustomerRelationshipManagement.Interfaces.IFinance.Payments;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NPOI.POIFS.Properties;
using Org.BouncyCastle.Crypto;
using StackExchange.Redis;
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
    [ApiExplorerSettings(GroupName = "v1")]
    public class PaymentService:ApplicationService, IPaymentService
    {
        private readonly IRepository<Payment, Guid> repository;
        private readonly IRepository<PaymentMethod, Guid> paymentmethodeceivables;
        private readonly IRepository<Receivables, Guid> receivablesRepository;
        private readonly IRepository<UserInfo, Guid> userinforeceivables;
        private readonly IRepository<Customer, Guid> customerrepository;
        private readonly IRepository<CrmContract, Guid> crmcontractreceivables;
        private readonly IDistributedCache<PageInfoCount<PaymentDTO>> cache;


        private readonly IConnectionMultiplexer connectionMultiplexer;

        public PaymentService(IRepository<Payment, Guid> repository, IDistributedCache<PageInfoCount<PaymentDTO>> cache, IRepository<Receivables, Guid> receivablesRepository, IRepository<UserInfo, Guid> userinforeceivables, IRepository<PaymentMethod, Guid> paymentmethodeceivables, IRepository<Customer, Guid> customerrepository, IRepository<CrmContract, Guid> crmcontractreceivables, IConnectionMultiplexer connectionMultiplexer)
        {
            this.repository = repository;
            this.cache = cache;
            this.receivablesRepository = receivablesRepository;
            this.userinforeceivables = userinforeceivables;
            this.paymentmethodeceivables = paymentmethodeceivables;
            this.customerrepository = customerrepository;
            this.crmcontractreceivables = crmcontractreceivables;
            this.connectionMultiplexer = connectionMultiplexer;
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
            // 清除对应的缓存
            await ClearAbpCacheAsync();

            //更新应收
            if (createUpdatePaymentDTO.ReceivableId != Guid.Empty)
            {
                var receivables = await receivablesRepository.GetAsync(createUpdatePaymentDTO.ReceivableId);
                if(receivables != null)
                {
                    receivables.PaymentId = payment.Id;
                    await receivablesRepository.UpdateAsync(receivables);
                    // 清除对应的缓存
                    await ClearAbpCacheAsync();
                }
            }
            return ApiResult<PaymentDTO>.Success(ResultCode.Success, ObjectMapper.Map<Payment, PaymentDTO>(payment));
        }
        
        /// <summary>
        /// 添加付款方式
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        public async Task<ApiResult<PaymentMethod>> InsertPaymentMethod(PaymentMethod paymentMethod)
        {
            await paymentmethodeceivables.InsertAsync(paymentMethod);
            return ApiResult<PaymentMethod>.Success(ResultCode.Success, paymentMethod);
        }
        /// <summary>
        /// 获取付款方式
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<PaymentMethod>>> GetPaymentMethod()
        {
            var paymentMethodInfo =await paymentmethodeceivables.GetListAsync();
            if(paymentMethodInfo == null)
            {
                return ApiResult<List<PaymentMethod>>.Fail("未找到数据",ResultCode.NotFound);
            }
            return ApiResult<List<PaymentMethod>>.Success(ResultCode.Success, paymentMethodInfo);
        }
        /// <summary>
        /// 处理收款记录的审批操作
        /// </summary>
        /// <param name="paymentId">待审批的收款记录唯一标识</param>
        /// <param name="approverId">执行审批操作的用户唯一标识</param>
        /// <param name="isPass">审批结果（true=通过，false=拒绝）</param>
        /// <param name="comment">审批意见备注</param>
        /// <returns>包含操作结果的ApiResult对象</returns>
        public async Task<ApiResult> Approve(Guid id, Guid approverId, bool isPass, string? comment)
        {
            // 获取收款记录
            var payment = await repository.GetAsync(id);
            if (payment == null)
                return ApiResult.Fail("未找到该收款记录", ResultCode.NotFound);

            // 判断审批是否已结束（2=全部通过，3=拒绝）
            if (payment.PaymentStatus == 2 || payment.PaymentStatus == 3)
                return ApiResult.Fail("审批已结束", ResultCode.NotFound);

            // 判断是否有审批人或审批流程是否已结束
            if (payment.ApproverIds.Count == 0 || payment.CurrentStep >= payment.ApproverIds.Count)
                return ApiResult.Fail("无审批人或审批流程已结束", ResultCode.NotFound);

            // 获取当前应审批人
            var currentApprover = payment.ApproverIds[payment.CurrentStep];
            if (currentApprover != approverId)
                return ApiResult.Fail("当前不是你的审批环节", ResultCode.NotFound);

            // 记录审批意见和时间
            payment.ApproveComments.Add(comment);
            payment.ApproveTimes.Add(DateTime.Now);

            if (isPass==false)
            {
                // 审批拒绝
                payment.PaymentStatus = 3;
            }
            else
            {
                // 审批通过，进入下一个审批环节
                payment.CurrentStep++;
                if (payment.CurrentStep >= payment.ApproverIds.Count)
                {
                    // 所有审批人已通过
                    payment.PaymentStatus = 2;
                }
                else
                {
                    // 仍处于审核中
                    payment.PaymentStatus = 1;
                }
            }

            // 更新收款记录
            await repository.UpdateAsync(payment);
            return ApiResult.Success(ResultCode.Success);
        }

        //["2cd5a286-dbc8-2e6a-b720-3a1abd3eb2f0","61e730fa-53ca-420c-3690-3a1adc0f4e32","a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"]
        //2cd5a286-dbc8-2e6a-b720-3a1abd3eb2f0","61e730fa-53ca-420c-3690-3a1adc0f4e32","a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11


        /// <summary>
        /// 显示分页查询收款列表
        /// </summary>
        /// <param name="searchDTO"></param>
        /// <returns></returns>
        public async Task<ApiResult<PageInfoCount<PaymentDTO>>> GetPayment(PaymentSearchDTO searchDTO)
        {
            // 清除对应的缓存
            await ClearAbpCacheAsync();
            string cacheKey = $"GetPayment";
            var redislist = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var payments = await repository.GetQueryableAsync();
                var receivables = await receivablesRepository.GetQueryableAsync();
                var userinfo = await userinforeceivables.GetQueryableAsync();
                var customer = await customerrepository.GetQueryableAsync();
                var crmcontract = await crmcontractreceivables.GetQueryableAsync();
                var paymentmethod = await paymentmethodeceivables.GetQueryableAsync();

                // 联合查询
                var query = from p in payments
                            join r in receivables on p.ReceivableId equals r.Id into pr
                            from r in pr.DefaultIfEmpty() // left join，如果要inner join去掉DefaultIfEmpty
                            join c in userinfo on p.UserId equals c.Id into rc
                            from c in rc.DefaultIfEmpty()
                            join d in customer on p.CustomerId equals d.Id into rd
                            from d in rd.DefaultIfEmpty()
                            join e in crmcontract on p.ContractId equals e.Id into re
                            from e in re.DefaultIfEmpty()
                            join f in paymentmethod on p.PaymentMethod equals f.Id into pf
                            from f in pf.DefaultIfEmpty()
                            join creator in userinfo on r.CreatorId equals creator.Id into creatorJoin
                            from creator in creatorJoin.DefaultIfEmpty()
                            select new PaymentDTO
                            {
                                Id = p.Id,
                                PaymentCode = p.PaymentCode,
                                Amount = p.Amount,
                                PaymentMethod = p.PaymentMethod,
                                PaymentMethodName = f.PaymentMethodName,
                                PaymentDate = p.PaymentDate,
                                ApproverIds = p.ApproverIds,
                                CurrentStep = p.CurrentStep,
                                ApproveComments = p.ApproveComments,
                                ApproveTimes = p.ApproveTimes,
                                PaymentStatus = p.PaymentStatus,
                                Remark = p.Remark,
                                UserId = p.UserId,
                                RealName = c.RealName,
                                CustomerId = p.CustomerId,
                                ContractId = p.ContractId,
                                ReceivableId = p.ReceivableId,
                                ReceivablePay = r.ReceivablePay,
                                ContractName = e.ContractName,
                                CustomerName = d.CustomerName,
                                CreatorId = p.CreatorId,
                                CreatorRealName = creator.RealName,
                                CreationTime = p.CreationTime,
                                CurrentAuditorName = "",
                            };




                // 这里可以加上你的where条件，对p.xxx和r.xxx都可以筛选
                query = query.WhereIf(!string.IsNullOrEmpty(searchDTO.PaymentCode), x => x.PaymentCode.Contains(searchDTO.PaymentCode))
                 .WhereIf(searchDTO.PaymentStatus != null, x => x.PaymentStatus == searchDTO.PaymentStatus)
                    .WhereIf(searchDTO.StartTime.HasValue, x => x.PaymentDate >= searchDTO.StartTime.Value)
                    .WhereIf(searchDTO.EndTime.HasValue, x => x.PaymentDate <= searchDTO.EndTime.Value.AddDays(1))
                    .WhereIf(searchDTO.UserId.HasValue, x => x.UserId == searchDTO.UserId.Value)
                    .WhereIf(searchDTO.CustomerId.HasValue, x => x.CustomerId == searchDTO.CustomerId.Value)
                    .WhereIf(searchDTO.ContractId.HasValue, x => x.ContractId == searchDTO.ContractId.Value)
                    .WhereIf(searchDTO.CreatorId.HasValue, x => x.CreatorId == searchDTO.CreatorId.Value)
                    .WhereIf(searchDTO.PaymentDate.HasValue, x => x.PaymentDate >= searchDTO.PaymentDate.Value && x.PaymentDate < searchDTO.PaymentDate.Value.AddDays(1))
                    .WhereIf(searchDTO.PaymentMethod.HasValue, x => x.PaymentMethod == searchDTO.PaymentMethod.Value);

                var resultList = query.ToList(); // 数据拉回内存
                if (searchDTO.ApproverIds != null && searchDTO.ApproverIds.Any())
                {
                    resultList = resultList
                        .Where(x => x.ApproverIds != null && x.ApproverIds.Intersect(searchDTO.ApproverIds).Any())
                        .ToList();
                }


                // 先ToList，后处理AuditorNames
                var userList = userinfo.ToList();
                foreach (var item in resultList)
                {
                    if (item.ApproverIds != null && item.ApproverIds.Count > 0)
                    {
                        item.AuditorNames = string.Join(",", userList.Where(u => item.ApproverIds.Contains(u.Id)).Select(u => u.RealName));
                        // 只显示当前审核人
                        if (item.CurrentStep >= 0 && item.CurrentStep < item.ApproverIds.Count)
                        {
                            //通过索引从审批人 ID 列表中获取当前步骤的审批人 ID
                            var currentAuditorId = item.ApproverIds[item.CurrentStep];
                            var currentAuditor = userList.FirstOrDefault(u => u.Id == currentAuditorId);
                            item.CurrentAuditorName = currentAuditor?.RealName ?? "";
                        }
                        else
                        {
                            item.CurrentAuditorName = "";
                        }
                    }
                    else
                    {
                        item.AuditorNames = string.Empty;
                        item.CurrentAuditorName = "";
                    }
                }
                // 使用ABP框架的分页方法进行分页查询
                //var res = query.PageResult(searchDTO.PageIndex, searchDTO.PageSize);
                //// 构建分页结果对象
                //var pageInfo = new PageInfoCount<PaymentDTO>
                //{
                //    TotalCount = res.RowCount, // 总记录数
                //    PageCount = (int)Math.Ceiling(res.RowCount * 1.0 / searchDTO.PageSize), // 总页数
                //    Data = resultList // 当前页数据
                //};

                // 分页
                int totalCount = resultList.Count;
                var pagedData = resultList.Skip((searchDTO.PageIndex - 1) * searchDTO.PageSize).Take(searchDTO.PageSize).ToList();

                var pageInfo = new PageInfoCount<PaymentDTO>
                {
                    TotalCount = totalCount,
                    PageCount = (int)Math.Ceiling(totalCount * 1.0 / searchDTO.PageSize),
                    Data = pagedData
                };
                return pageInfo;

            }, () => new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
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
            // 清除对应的缓存
            await ClearAbpCacheAsync();
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
                // 清除对应的缓存
                await ClearAbpCacheAsync();

                return ApiResult<PaymentDTO>.Success(ResultCode.Success, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 批量删除收款的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<PaymentDTO>> DeleteAllAsync([FromBody]Guid[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    return ApiResult<PaymentDTO>.Fail("请选择要删除的数据", ResultCode.Fail);
                }

                // 批量删除数据库记录
                foreach (var id in ids)
                {
                    await repository.DeleteAsync(id);
                    // 清除对应的缓存
                    await ClearAbpCacheAsync();
                }
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


        /// <summary>
        /// 清除关于c:PageInfo,k的所有信息
        /// </summary>
        /// <returns></returns>
        public async Task ClearAbpCacheAsync()
        {
            var endpoints = connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: "c:PageInfo,k:*");//填写自己的缓存前缀
                foreach (var key in keys)
                {
                    await connectionMultiplexer.GetDatabase().KeyDeleteAsync(key);
                }
            }
        }
    }
}

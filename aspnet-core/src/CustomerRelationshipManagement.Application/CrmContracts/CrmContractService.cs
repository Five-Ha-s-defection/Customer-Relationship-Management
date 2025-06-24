using AutoMapper.Internal.Mappers;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Contracts;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;
using CustomerRelationshipManagement.ICrmContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace CustomerRelationshipManagement.CrmContracts
{
    public class CrmContractService : ApplicationService, ICrmContractService
    {
        private readonly IRepository<CrmContract, Guid> repository;
        private readonly ILogger<CrmContractService> logger;

        public CrmContractService(IRepository<CrmContract, Guid> repository, ILogger<CrmContractService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 查询分页显示合同列表
        /// </summary>
        /// <param name="pageCrmContractDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IList<ShowCrmContractDto>>> ShowCrmContractList([FromQuery] PageCrmContractDto pageCrmContractDto)
        {
            try
            {
                //对合同表预查询
                var query = await repository.GetQueryableAsync();


                //查询条件(1.合同名称模糊查询)
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ContractName) && pageCrmContractDto.CheckType == 0, a => a.ContractName.Contains(pageCrmContractDto.ContractName));
                //创建时间范围查询
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.BeginTime)&&pageCrmContractDto.SearchTimeType==0,a=>a.CreationTime >= DateTime.Parse(pageCrmContractDto.BeginTime));
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.EndTime) && pageCrmContractDto.SearchTimeType == 0, a => a.CreationTime < DateTime.Parse(pageCrmContractDto.BeginTime).AddDays(1));
                //签订时间范围查询
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.BeginTime) && pageCrmContractDto.SearchTimeType == 1, a => a.SignDate >= DateTime.Parse(pageCrmContractDto.BeginTime));
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.EndTime) && pageCrmContractDto.SearchTimeType == 1, a => a.SignDate < DateTime.Parse(pageCrmContractDto.BeginTime).AddDays(1));
                //生效时间范围查询
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.BeginTime) && pageCrmContractDto.SearchTimeType == 2, a => a.CommencementDate >= DateTime.Parse(pageCrmContractDto.BeginTime));
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.EndTime) && pageCrmContractDto.SearchTimeType == 2, a => a.CommencementDate < DateTime.Parse(pageCrmContractDto.BeginTime).AddDays(1));
                //截止时间范围查询
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.BeginTime) && pageCrmContractDto.SearchTimeType == 3, a => a.ExpirationDate >= DateTime.Parse(pageCrmContractDto.BeginTime));
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.EndTime) && pageCrmContractDto.SearchTimeType == 3, a => a.ExpirationDate < DateTime.Parse(pageCrmContractDto.BeginTime).AddDays(1));

                //高级搜索(1.全部满足的情况,合同名称精准查询)
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ContractName) && pageCrmContractDto.CheckType == 1, a => a.ContractName.Equals(pageCrmContractDto.ContractName));
                //负责人查询
                query = query.WhereIf(pageCrmContractDto.UserIds.Count() != 0 && pageCrmContractDto.CheckType == 1, a => pageCrmContractDto.UserIds.Contains(a.UserId));
                //创建人查询
                query = query.WhereIf(pageCrmContractDto.CreateUserIds.Count() != 0 && pageCrmContractDto.CheckType == 1, a => pageCrmContractDto.CreateUserIds.Contains((Guid)a.CreatorId));
                //所属客户查询
                query = query.WhereIf(pageCrmContractDto.CustomerId != Guid.Empty && pageCrmContractDto.CheckType == 1, a => a.UserId.Equals(pageCrmContractDto.CustomerId));
                //签订日期
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.SignDate) && pageCrmContractDto.CheckType == 1, a=>a.SignDate >= DateTime.Parse(pageCrmContractDto.SignDate) && a.SignDate < DateTime.Parse(pageCrmContractDto.SignDate).AddDays(1));
                //生效日期
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.CommencementDate) && pageCrmContractDto.CheckType == 1, a => a.CommencementDate >= DateTime.Parse(pageCrmContractDto.CommencementDate) && a.CommencementDate < DateTime.Parse(pageCrmContractDto.CommencementDate).AddDays(1));
                //截止日期
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ExpirationDate) && pageCrmContractDto.CheckType == 1, a => a.ExpirationDate >= DateTime.Parse(pageCrmContractDto.ExpirationDate) && a.ExpirationDate < DateTime.Parse(pageCrmContractDto.ExpirationDate).AddDays(1));
                //经销商
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ContractName) && pageCrmContractDto.CheckType == 1, a => a.ContractName.Equals(pageCrmContractDto.ContractName));

                //高级搜索(2.部分满足的情况,合同名称精准查询)
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ContractName) && pageCrmContractDto.CheckType == 2, a => a.ContractName.Contains(pageCrmContractDto.ContractName));
                //负责人查询
                query = query.WhereIf(pageCrmContractDto.UserIds.Count() != 0 && pageCrmContractDto.CheckType == 2, a => pageCrmContractDto.UserIds.Contains(a.UserId));
                //创建人查询
                query = query.WhereIf(pageCrmContractDto.CreateUserIds.Count() != 0 && pageCrmContractDto.CheckType == 2, a => pageCrmContractDto.CreateUserIds.Contains((Guid)a.CreatorId));
                //所属客户查询
                query = query.WhereIf(pageCrmContractDto.CustomerId != Guid.Empty && pageCrmContractDto.CheckType == 2, a => a.UserId.Equals(pageCrmContractDto.CustomerId));
                //签订日期
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.SignDate) && pageCrmContractDto.CheckType == 2, a => a.SignDate >= DateTime.Parse(pageCrmContractDto.SignDate) && a.SignDate < DateTime.Parse(pageCrmContractDto.SignDate).AddDays(1));
                //生效日期
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.CommencementDate) && pageCrmContractDto.CheckType == 2, a => a.CommencementDate >= DateTime.Parse(pageCrmContractDto.CommencementDate) && a.CommencementDate < DateTime.Parse(pageCrmContractDto.CommencementDate).AddDays(1));
                //截止日期
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ExpirationDate) && pageCrmContractDto.CheckType == 2, a => a.ExpirationDate >= DateTime.Parse(pageCrmContractDto.ExpirationDate) && a.ExpirationDate < DateTime.Parse(pageCrmContractDto.ExpirationDate).AddDays(1));
                //经销商
                query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ContractName) && pageCrmContractDto.CheckType == 2, a => a.ContractName.Contains(pageCrmContractDto.ContractName));


                //分页
                var querypaging = query.OrderByDescending(a => a.Id).Skip(pageCrmContractDto.PageIndex).Take(pageCrmContractDto.PageSize);

                //将数据通过映射转换
                var crmcontractdto = ObjectMapper.Map<IList<CrmContract>, IList<ShowCrmContractDto>>(querypaging.ToList());

                //返回apiresult
                return ApiResult<IList<ShowCrmContractDto>>.Success(ResultCode.Success, crmcontractdto);
            }
            catch (Exception ex)
            {
                logger.LogError("合同分页显示或查询出错 " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 添加的方法
        /// </summary>
        /// <param name="addUpdateCrmContractDto"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddCrmContract(AddUpdateCrmContractDto addUpdateCrmContractDto)
        {
            var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using (scope)
            {
                try
                {
                    //转换要添加的合同表数据
                    var crmcontract = ObjectMapper.Map<AddUpdateCrmContractDto, CrmContract>(addUpdateCrmContractDto);

                    //执行插入的数据的操作
                    var result = await repository.InsertAsync(crmcontract);

                    //提交事务
                    scope.Complete();

                    //返回统一返回值
                    return ApiResult.Success(ResultCode.Success);
                }
                catch (Exception ex)
                {
                    logger.LogError("添加合同事务出错 " + ex.Message);
                    throw;
                }
                finally
                {
                    scope.Dispose();
                }
            }
        }




    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.crmcontracts;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;
using CustomerRelationshipManagement.DTOS.Finance.Receibableses;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.Finance.Receivableses;
using CustomerRelationshipManagement.Interfaces.ICrmContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Tls.Crypto.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CrmContracts
{
    /// <summary>
    /// 合同服务(模版)
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class CrmContractService : ApplicationService, ICrmContractService
    {
        private readonly IRepository<CrmContract, Guid> repository;
        private readonly IRepository<Receivables, Guid> receivablesrepository;
        private readonly IRepository<CrmContractandProduct, Guid> crmContractandProductrepository;
        private readonly ILogger<CrmContractService> logger;

        public CrmContractService(IRepository<CrmContract, Guid> repository, IRepository<Receivables, Guid> receivablesrepository, IRepository<CrmContractandProduct, Guid> crmContractandProductrepository, ILogger<CrmContractService> logger)
        {
            this.repository = repository;
            this.receivablesrepository = receivablesrepository;
            this.crmContractandProductrepository = crmContractandProductrepository;
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
            //对合同表预查询
            var query = await repository.GetQueryableAsync();

            #region 查询条件
            //查询条件(1.合同名称模糊查询)
            query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ContractName) && pageCrmContractDto.CheckType == 0, a => a.ContractName.Contains(pageCrmContractDto.ContractName));
            //创建时间范围查询
            query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.BeginTime) && pageCrmContractDto.SearchTimeType == 0, a => a.CreationTime >= DateTime.Parse(pageCrmContractDto.BeginTime));
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
            query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.SignDate) && pageCrmContractDto.CheckType == 1, a => a.SignDate >= DateTime.Parse(pageCrmContractDto.SignDate) && a.SignDate < DateTime.Parse(pageCrmContractDto.SignDate).AddDays(1));
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
            #endregion

            //分页
            var querypaging = query.OrderByDescending(a => a.Id).Skip(pageCrmContractDto.PageIndex).Take(pageCrmContractDto.PageSize);

            //将数据通过映射转换
            var crmcontractdto = ObjectMapper.Map<IList<CrmContract>, IList<ShowCrmContractDto>>(querypaging.ToList());

            //返回apiresult
            return ApiResult<IList<ShowCrmContractDto>>.Success(ResultCode.Success, crmcontractdto);
        }

        /// <summary>
        ///  添加合同的方法(事务实现),思路:先将产品信息预存在前端的页面中,然后写出UI样后,根据UI调试后端的需求
        /// </summary>
        /// <param name="addCrmContractDto"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddCrmContract(AddCrmContractDto addCrmContractDto)
        {
            var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using (scope)
            {
                try
                {
                    //合同表操作
                    //转换要添加的合同表数据
                    var crmcontract = ObjectMapper.Map<AddCrmContractDto, CrmContract>(addCrmContractDto);

                    //执行插入合同表的数据的操作
                    var crmcontractresult = await repository.InsertAsync(crmcontract);


                    //合同产品关系表操作
                    //创建要添加的关系表数据集合
                    List<CrmContractandProduct> crmcontractproductlist = new List<CrmContractandProduct>();
                    //遍历添加产品id到集合中
                    foreach (var item in addCrmContractDto.AddCrmcontractandProductDto)
                    {
                        crmcontractproductlist.Add(new CrmContractandProduct
                        {
                            CrmContractId = crmcontract.Id,
                            ProductId = item.ProductId,
                            BuyProductNum = item.BuyProductNum,
                            SellPrice = item.SellPrice,
                            SumPrice = item.SumPrice,
                        });
                    }

                    //批量添加到关系表中
                    await crmContractandProductrepository.InsertManyAsync(crmcontractproductlist);


                    //应收款表操作
                    //转换要添加的应收款数据
                    var receivables = ObjectMapper.Map<CreateUpdateReceibablesDto, Receivables>(addCrmContractDto.CreateUpdateReceibablesDto);
                    receivables.CustomerId = crmcontractresult.CustomerId;
                    receivables.ContractId = crmcontractresult.Id;
                    receivables.UserId = crmcontractresult.UserId;

                    //执行插入应收款表的操作
                    var receivablesresult = await receivablesrepository.InsertAsync(receivables);

                    //提交事务
                    scope.Complete();

                    //返回统一返回值
                    return ApiResult.Success(ResultCode.Success);
                }
                finally
                {
                    scope.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取合同表的详情
        /// </summary>
        /// <param name="GetId"></param>
        /// <returns></returns>
        public async Task<ApiResult<GetCrmContractDto>> GetCrmContract(Guid GetId)
        {
            //根据合同id获取合同表数据
            var crmcontract = await repository.FindAsync(GetId);
            var crmcontractdto = ObjectMapper.Map<CrmContract, ShowCrmContractDto>(crmcontract);

            //根据合同表id获取所有产品关系表
            var crmcontractproduct = await crmContractandProductrepository.GetListAsync(a => a.CrmContractId == GetId);

            //实例化dto
            var getCrmContractDto = new GetCrmContractDto();
            getCrmContractDto.showCrmContractDto = crmcontractdto;
            getCrmContractDto.crmContractandProducts = crmcontractproduct;

            return ApiResult<GetCrmContractDto>.Success(ResultCode.Success, getCrmContractDto);
        }

        /// <summary>
        /// 修改合同的方法
        /// </summary>
        /// <param name="UpdateCrmContractDto"></param>
        /// <returns></returns>
        public async Task<ApiResult> UpdateCrmContract(Guid id, UpdateCrmContractDto updateCrmContractDto)
        {
            var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using (scope)
            {
                try
                {
                    //合同表操作
                    //获取对应id的合同表数据
                    var crmcontractmodel = await repository.FindAsync(a => a.Id == id);

                    if (crmcontractmodel == null)
                    {
                        return ApiResult.Fail("该合同的数据不存在", ResultCode.Fail);
                    }

                    //将修改的合同dto转换为合同实体
                    var crmcontract = ObjectMapper.Map(updateCrmContractDto, crmcontractmodel);

                    //执行合同修改的方法
                    await repository.UpdateAsync(crmcontract);


                    //产品表操作
                    //根据id在关系表中找出对应的信息
                    var product = await crmContractandProductrepository.GetListAsync(a => a.CrmContractId == id);

                    //仅创建新的记录集合
                    List<CrmContractandProduct> newcrmContractandProducts = new List<CrmContractandProduct>();
                    newcrmContractandProducts.AddRange(product);

                    //删除查到的数据
                    await crmContractandProductrepository.HardDeleteAsync(product, true);

                    //添加新的关系表信息
                    foreach (var item in updateCrmContractDto.AddCrmcontractandProductDto)
                    {
                        newcrmContractandProducts.Add(new CrmContractandProduct
                        {
                            CrmContractId = crmcontract.Id,
                            ProductId = item.ProductId,
                            BuyProductNum = item.BuyProductNum,
                            SellPrice = item.SellPrice,
                            SumPrice = item.SumPrice,
                        });
                    }

                    //插入新记录
                    await crmContractandProductrepository.InsertManyAsync(newcrmContractandProducts);

                    scope.Complete();

                    return ApiResult.Success(ResultCode.Success);
                }
                finally
                {
                    scope.Dispose();
                }
            }
        }

        /// <summary>
        /// 删除合同方法
        /// </summary>
        /// <param name="DeleteId"></param>
        /// <returns></returns>
        public async Task<ApiResult> DeleteCrmContract(Guid DeleteId)
        {
            try
            {
                await repository.DeleteAsync(DeleteId);

                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail("删除失败", ResultCode.Fail);
                throw;
            }
        }

        /// <summary>
        /// 批量删除合同方法
        /// </summary>
        /// <param name="DeleteIds"></param>
        /// <returns></returns>
        public async Task<ApiResult> DeleteCrmContract(IList<Guid> DeleteIds)
        {
            try
            {
                await repository.DeleteManyAsync(DeleteIds);

                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail("删除失败", ResultCode.Fail);
                throw;
            }
        }

    }
}

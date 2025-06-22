using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Contracts;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;
using CustomerRelationshipManagement.ICrmContracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CrmContracts
{
    public class ShowCrmContract: ApplicationService, ICrmContractService
    {
        private readonly IRepository<CrmContract, Guid> repository;

        public ShowCrmContract(IRepository<CrmContract,Guid> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 显示合同列表
        /// </summary>
        /// <param name="pageCrmContractDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<IList<ShowCrmContractDto>>> ShowCrmContractList([FromQuery]PageCrmContractDto pageCrmContractDto)
        {
            //对合同表预查询
            var query = await repository.GetQueryableAsync();

            //查询条件
            query = query.WhereIf(!string.IsNullOrEmpty(pageCrmContractDto.ContractName), a => a.ContractName.Contains(pageCrmContractDto.ContractName));

            //分页
            var querypaging = query.OrderByDescending(a => a.Id).Skip(pageCrmContractDto.PageIndex).Take(pageCrmContractDto.PageSize);

            //将数据通过映射转换
            var crmcontractdto = ObjectMapper.Map<IList<CrmContract>,IList<ShowCrmContractDto>>(querypaging.ToList());

            //返回apiresult
            return ApiResult<IList<ShowCrmContractDto>>.Success(ResultCode.Success, crmcontractdto);
        }


    }
}

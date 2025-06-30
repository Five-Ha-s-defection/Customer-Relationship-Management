using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Industrys;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IIndustrys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.Industrys
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class IndustryService:ApplicationService, IIndustryService
    {
        private readonly IRepository<Industry> repository;
        private readonly ILogger<IndustryService> logger;
        public IndustryService(IRepository<Industry> repository, ILogger<IndustryService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 新增行业
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<IndustryDto>> AddIndustry(CreateUpdateIndustryDto dto)
        {
            try
            {
                var entity=ObjectMapper.Map<CreateUpdateIndustryDto,Industry>(dto);
                entity=await repository.InsertAsync(entity);    
                return ApiResult<IndustryDto>.Success(ResultCode.Success,ObjectMapper.Map<Industry,IndustryDto>(entity));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

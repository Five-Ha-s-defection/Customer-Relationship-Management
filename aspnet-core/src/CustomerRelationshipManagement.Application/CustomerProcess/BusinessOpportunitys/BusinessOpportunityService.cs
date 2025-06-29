using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IBusinessOpportunitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class BusinessOpportunityService : ApplicationService, IBusinessOpportunityService
    {
        private readonly IRepository<BusinessOpportunity> businessopportunityrepository;
        private readonly ILogger<BusinessOpportunityService> logger;
        public BusinessOpportunityService(IRepository<BusinessOpportunity> businessopportunityrepository, ILogger<BusinessOpportunityService> logger)
        {
            this.businessopportunityrepository = businessopportunityrepository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加商机
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<BusinessOpportunityDto>> AddBusinessOpportunity(CreateUpdateBusinessOpportunityDto dto)
        {
            try
            {
                var businessopportunity = ObjectMapper.Map<CreateUpdateBusinessOpportunityDto, BusinessOpportunity>(dto);
                var list = await businessopportunityrepository.InsertAsync(businessopportunity);
                return ApiResult<BusinessOpportunityDto>.Success(ResultCode.Success, ObjectMapper.Map<BusinessOpportunity, BusinessOpportunityDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "添加商机失败");
                return ApiResult<BusinessOpportunityDto>.Fail("添加商机失败", ResultCode.Fail);
            }
        }
    }
}

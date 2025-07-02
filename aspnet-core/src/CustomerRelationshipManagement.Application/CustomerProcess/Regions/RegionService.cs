using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IRegions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.CustomerRegions
{
    /// <summary>
    /// 客户地区服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class RegionService:ApplicationService, IRegionService
    {
        private readonly IRepository<CustomerRegion> repository;
        private readonly ILogger<RegionService> logger;
        public RegionService(IRepository<CustomerRegion> repository, ILogger<RegionService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加客户地区
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<RegionDto>> AddRegion(CreateUpdateRegionDto dto)
        {
            try
            {
                var entity = ObjectMapper.Map<CreateUpdateRegionDto, CustomerRegion>(dto);
                entity = await repository.InsertAsync(entity);
                return ApiResult<RegionDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerRegion, RegionDto>(entity));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

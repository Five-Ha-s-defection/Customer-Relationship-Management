using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Prioritys;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IPrioritys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.Prioritys
{
    /// <summary>
    /// 商机优先级服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class PriorityService:ApplicationService,IPriorityService
    {
        private readonly IRepository<Priority> priorityRepository;
        private readonly ILogger<PriorityService> logger;
        public PriorityService(IRepository<Priority> priorityRepository, ILogger<PriorityService> logger)
        {
            this.priorityRepository = priorityRepository;
            this.logger = logger;
        }
    
        /// <summary>
        /// 创建商机的优先级
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<PriorityDto>> AddPriority(CreateUpdatePriorityDto dto)
        {
            try
            {
                var priority = ObjectMapper.Map<CreateUpdatePriorityDto, Priority>(dto);
                var list = await priorityRepository.InsertAsync(priority);
                return ApiResult<PriorityDto>.Success(ResultCode.Success, ObjectMapper.Map<Priority, PriorityDto>(list));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "添加优先级失败");
                return ApiResult<PriorityDto>.Fail("添加优先级失败", ResultCode.Fail);
            }
        }
    }
}

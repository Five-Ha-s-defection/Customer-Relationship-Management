using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerLevels;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Levels;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ILevels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.Levels
{
    /// <summary>
    /// 客户等级服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class LevelService:ApplicationService,ILevelService
    {
        private readonly IRepository<CustomerLevel> repository;
        private readonly ILogger<LevelService> logger;  
        public LevelService(IRepository<CustomerLevel> repository, ILogger<LevelService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// /添加客户等级
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<LevelDto>> AddLevel(CreateUpdateLevelDto dto)
        {
            var entity = ObjectMapper.Map<CreateUpdateLevelDto,CustomerLevel>(dto);
            entity = await repository.InsertAsync(entity);
            return ApiResult<LevelDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerLevel, LevelDto>(entity));
        }
    }
}

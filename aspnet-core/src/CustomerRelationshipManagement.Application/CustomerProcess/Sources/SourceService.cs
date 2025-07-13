using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.ClueSources;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ISources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.Sources
{
    /// <summary>
    /// 客户源服务/线索源服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class SourceService:ApplicationService,ISourceService
    {
        private readonly IRepository<ClueSource> repository;
        private readonly ILogger<SourceService> logger;
        public SourceService(IRepository<ClueSource> repository, ILogger<SourceService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加线索/客户来源
        /// </summary>
        /// <param name="sourceDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<SourceDto>> AddSource(CreateUpdateSourceDto sourceDto)
        {
            try
            {
                var entity=ObjectMapper.Map<CreateUpdateSourceDto,ClueSource>(sourceDto);
                entity=await repository.InsertAsync(entity);
                return ApiResult<SourceDto>.Success(ResultCode.Success, ObjectMapper.Map<ClueSource, SourceDto>(entity));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

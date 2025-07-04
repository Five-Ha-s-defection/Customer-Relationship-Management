using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CommunicationTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICommunicationTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.CommunicationTypes
{
    /// <summary>
    /// 沟通类型服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class CommunicationTypeService:ApplicationService,ICommunicationTypeService
    {
        private readonly IRepository<CommunicationType> repository;
        private readonly ILogger<CommunicationTypeService> logger;
        public CommunicationTypeService(IRepository<CommunicationType> repository, ILogger<CommunicationTypeService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加沟通类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CommunicationTypeDto>> AddCustomerType(CreateUpdateCommunicationTypeDto dto)
        {
            try
            {
                var entity = ObjectMapper.Map<CreateUpdateCommunicationTypeDto, CommunicationType>(dto);
                entity = await repository.InsertAsync(entity);
                return ApiResult<CommunicationTypeDto>.Success(ResultCode.Success, ObjectMapper.Map<CommunicationType, CommunicationTypeDto>(entity));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

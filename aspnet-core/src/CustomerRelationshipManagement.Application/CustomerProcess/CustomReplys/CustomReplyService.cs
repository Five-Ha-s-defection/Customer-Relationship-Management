using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomReplys;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomReplys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.CustomReplys
{
    /// <summary>
    /// 自定义回复服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class CustomReplyService:ApplicationService,ICustomReplyService
    {
        private readonly IRepository<CustomReply> repository;
        private readonly ILogger<CustomReplyService> logger;
        public CustomReplyService(IRepository<CustomReply> repository, ILogger<CustomReplyService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加自定义回复
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CustomReplyDto>> AddCustomerType(CreateUpdateCustomReplyDto dto)
        {
            try
            {
                var entity = ObjectMapper.Map<CreateUpdateCustomReplyDto, CustomReply>(dto);
                entity = await repository.InsertAsync(entity);
                return ApiResult<CustomReplyDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomReply, CustomReplyDto>(entity));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

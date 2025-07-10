using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.ContactRelations;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.IContactRelations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace CustomerRelationshipManagement.CustomerProcess.ContactRelations
{
    /// <summary>
    /// 联系人关系服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class ContactRelationService:ApplicationService,IContactRelationService
    {
        private readonly IRepository<ContactRelation> repository;
        private readonly ILogger<ContactRelationService> logger;
        public ContactRelationService(IRepository<ContactRelation> repository, ILogger<ContactRelationService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加联系人关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<ContactRelationDto>> AddContactRelation(CreateUpdateContactRelationsDto dto)
        {
            try
            {
                var entity = ObjectMapper.Map<CreateUpdateContactRelationsDto, ContactRelation>(dto);
                entity = await repository.InsertAsync(entity);
                return ApiResult<ContactRelationDto>.Success(ResultCode.Success, ObjectMapper.Map<ContactRelation, ContactRelationDto>(entity));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

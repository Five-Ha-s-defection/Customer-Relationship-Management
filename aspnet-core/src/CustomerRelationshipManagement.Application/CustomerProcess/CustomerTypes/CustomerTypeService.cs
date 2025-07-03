using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.CustomerLevels;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerTypes;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Levels;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ICustomerTypes;
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

namespace CustomerRelationshipManagement.CustomerProcess.CustomerTypes
{
    /// <summary>
    /// 客户类型服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class CustomerTypeService:ApplicationService,ICustomerTypeService
    {
        private readonly IRepository<CustomerType> repository;
        private readonly ILogger<CustomerTypeService> logger;   
        public CustomerTypeService(IRepository<CustomerType> repository, ILogger<CustomerTypeService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加客户类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CustomerTypeDto>> AddCustomerType(CreateUpdateCustomerTypeDto dto)
        {
            try
            {
                var entity = ObjectMapper.Map<CreateUpdateCustomerTypeDto, CustomerType>(dto);
                entity = await repository.InsertAsync(entity);
                return ApiResult<CustomerTypeDto>.Success(ResultCode.Success, ObjectMapper.Map<CustomerType, CustomerTypeDto>(entity));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.CustomerProcess.Cars;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Cars;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.SalesProgresses;
using CustomerRelationshipManagement.Interfaces.ICustomerProcess.ISalesProgresses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CustomerProcess.SalesProgresses
{
    /// <summary>
    /// 商机的销售进度
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class SalesProgressService:ApplicationService, ISalesProgressService
    {
        private readonly IRepository<SalesProgress> salesprogressrepository;
        private readonly ILogger<SalesProgressService> logger;
        public SalesProgressService(IRepository<SalesProgress> salesprogressrepository, ILogger<SalesProgressService> logger)
        {
            this.salesprogressrepository = salesprogressrepository;
            this.logger = logger;
        }

        /// <summary>
        /// 添加销售进度
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<SalesProgressDto>> AddSalesProgress(CreateUpdateSalesProgressDto dto)
        {
            var salesprogress = ObjectMapper.Map<CreateUpdateSalesProgressDto, SalesProgress>(dto);
            var list = await salesprogressrepository.InsertAsync(salesprogress);
            return ApiResult<SalesProgressDto>.Success(ResultCode.Success, ObjectMapper.Map<SalesProgress, SalesProgressDto>(list));
        }
    }
}

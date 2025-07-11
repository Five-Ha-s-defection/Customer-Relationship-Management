using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Interfaces.Record;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RecordDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;

namespace CustomerRelationshipManagement.Record
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]

    public class OperationLogService:ApplicationService, IOperationLogService
    {
        private readonly IRepository<OperationLog, Guid> repository;
        private readonly IRepository<UserInfo, Guid> userrepository;

        public OperationLogService(IRepository<OperationLog, Guid> repository, IRepository<UserInfo, Guid> userrepository)
        {
            this.repository = repository;
            this.userrepository = userrepository;
        }
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="bizType"></param>
        /// <param name="bizId"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<OperationLogDto>>> GetLogs(string bizType, Guid bizId)
        {
            var operationLog = await repository.GetQueryableAsync();
            var user = await userrepository.GetQueryableAsync();
            
            var query = from o in operationLog
                        join creator in user on o.CreatorId equals creator.Id into creatorJoin
                        from creator in creatorJoin.DefaultIfEmpty()
                        select new OperationLogDto
                        {
                            Id = o.Id,
                            BizType = o.BizType,
                            BizId = o.BizId,
                            Action = o.Action,
                            CreatorId = o.CreatorId,
                            CreationTime = o.CreationTime,
                            CreatorName = creator.RealName
                        };
            query = query.Where(x => x.BizType == bizType && x.BizId == bizId);
            return ApiResult<List<OperationLogDto>>.Success(ResultCode.Success,query.ToList());
        }

    }
}

using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.RecordDto;
using CustomerRelationshipManagement.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Interfaces.Record
{
    public interface IOperationLogService: IApplicationService
    {
        Task<ApiResult<List<OperationLogDto>>> GetLogs(string bizType, Guid bizId);
    }
}

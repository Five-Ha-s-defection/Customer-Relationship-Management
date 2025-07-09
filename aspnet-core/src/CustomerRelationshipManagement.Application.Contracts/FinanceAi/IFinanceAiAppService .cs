using CustomerRelationshipManagement.DTOS.Finance.FinanceAi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.FinanceAi
{
    public interface IFinanceAiAppService:IApplicationService
    {
        /// <summary>
        /// 获取AI结果
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<string> AskAsync(AiRequestDto dto);
    }
}

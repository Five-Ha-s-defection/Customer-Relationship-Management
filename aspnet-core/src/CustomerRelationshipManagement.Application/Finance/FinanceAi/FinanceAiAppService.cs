using CustomerRelationshipManagement.DTOS.Finance.FinanceAi;
using CustomerRelationshipManagement.FinanceAi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Finance.FinanceAi
{
    public class FinanceAiAppService : IApplicationService, IFinanceAiAppService
    {
        public FinanceAiAppService()
        {
        }
        public Task<string> AskAsync(AiRequestDto dto)
        {
            throw new NotImplementedException();
        }
    }
}

using CustomerRelationshipManagement.DTOS.ChatGLMAi;
using CustomerRelationshipManagement.DTOS.Finance.FinanceAi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.ChatGLMAi
{
    /// <summary>
    /// 聊天接口
    /// </summary>
    public interface IChatGLMService: IApplicationService
    {
        /// <summary>
        /// 聊天对话
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        Task<string> ChatAsync(List<ChatMessage> history, string model);
    }
}

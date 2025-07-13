using CustomerRelationshipManagement.DTOS.ChatGLMAi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.ChatGLMAi
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ChatAppService:ApplicationService
    {
        private readonly IChatGLMService chatGLMService;

        public ChatAppService(IChatGLMService chatGLMService)
        {
            this.chatGLMService = chatGLMService;
        }
        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        [HttpPost("/api/app/chat-glm/context")]
        public async Task<string> ChatWithContextAsync([FromBody] ChatRequest request)
        {
            return await chatGLMService.ChatAsync(request.Messages, request.Model);
        }
    }
}

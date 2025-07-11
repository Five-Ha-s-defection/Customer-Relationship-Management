using CustomerRelationshipManagement.DTOS.ChatGLMAi;
using CustomerRelationshipManagement.DTOS.Finance.FinanceAi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Websocket.Client;

namespace CustomerRelationshipManagement.ChatGLMAi
{
    /// <summary>
    /// SparkAi服务
    /// </summary>
    public class ChatGLMService : IChatGLMService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public ChatGLMService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task<string> ChatAsync(List<ChatMessage> history, string model)
        {
            try
            {
                // 从配置文件中读取 API Key
                var apiKey = configuration["ChatGLM:ApiKey"];
                // 创建 HttpClient实例
                var client = httpClientFactory.CreateClient();
                // 设置请求头
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var body = new
                {
                    model = "glm-4-flash-250414",
                    messages = history.Select(m => new { role = m.Role, content = m.Content }).ToArray(),
                    temperature = 0.9,
                    top_p = 0.7,
                    stream = false
                };
                // 发送 POST 请求
                var response = await client.PostAsJsonAsync("https://open.bigmodel.cn/api/paas/v4/chat/completions", body);
                response.EnsureSuccessStatusCode();
                // 处理响应
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                return result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

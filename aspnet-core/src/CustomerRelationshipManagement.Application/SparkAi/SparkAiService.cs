using CustomerRelationshipManagement.DTOS.Finance.FinanceAi;
using CustomerRelationshipManagement.DTOS.SparkAi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Websocket.Client;

namespace CustomerRelationshipManagement.SparkAi
{
    /// <summary>
    /// SparkAi服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class SparkAiService : ApplicationService, ITransientDependency
    {
        private readonly IOptions<SparkAiOptions> options;

        public SparkAiService(IOptions<SparkAiOptions> options)
        {
            this.options = options;
        }
        /// <summary>
        /// 发起AI对话请求
        /// 获取智能回复（星火大模型）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("spark-ai-reply")]
        public async Task<string> GetReplyAsync(AiRequestDto dto)
        {
            string host = "spark-api.xf-yun.com";
            string date = DateTime.UtcNow.ToString("r"); // RFC1123 格式

            // 注意路径必须匹配你要访问的版本！
            string path = "/v1/x1";




            string signatureOrigin = $"host:{host}\ndate:{date}\nGET {path} HTTP/1.1";
            //string signatureSha = HMACSHA256(signatureOrigin, options.Value.ApiSecret);
            var secretBytes = Convert.FromBase64String(options.Value.ApiSecret);
            var signatureSha = Convert.ToBase64String(
    new HMACSHA256(secretBytes).ComputeHash(Encoding.UTF8.GetBytes(signatureOrigin))
);
            // 计算签名和authorization ...
            string authorizationOrigin =
                $"api_key=\"{options.Value.ApiKey}\",algorithm=\"hmac-sha256\", headers=\"host date request-line\", signature=\"{signatureSha}\"";

            string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorizationOrigin));

            // **关键，URL编码authorization**
            string authorizationUrlEncoded = Uri.EscapeDataString(authorization);

            string url = $"wss://spark-api.xf-yun.com/v1/x1" +
    $"?authorization={Uri.EscapeDataString(authorization)}" +
    $"&date={Uri.EscapeDataString(date)}" +
    $"&host=spark-api.xf-yun.com";

            Console.WriteLine("date: " + date);
            Console.WriteLine("signature origin:\n" + signatureOrigin);
            Console.WriteLine("signature base64: " + signatureSha);
            Console.WriteLine("authorization origin:\n" + authorizationOrigin);
            Console.WriteLine("authorization base64: " + authorization);
            Console.WriteLine("最终 URL：" + url);


            //构造请求消息(发送用户问题)
            var message = new
            {
                header = new { app_id = options.Value.AppId, uid = Guid.NewGuid().ToString("N") },
                parameter = new
                {
                    chat = new
                    {
                        domain = "general",
                        temperature = 0.5,
                        max_tokens = 1024
                    }
                },
                payload = new
                {
                    message = new
                    {
                        text = new[]
             {
                new { role = "user", content = dto.Input }
            }
                    }
                }
            };

            Console.WriteLine($"🌐 最终 URL：{url}");
            Console.WriteLine("📨 最终 message：" + JsonConvert.SerializeObject(message));


            // 发送请求
            var result = new StringBuilder();
            // 创建一个CancellationTokenSource对象
            var exitEvent = new ManualResetEvent(false);
            // 创建一个WebsocketClient对象
            using var client = new WebsocketClient(new Uri(url));

            client.MessageReceived.Subscribe(msg =>
            {
                Console.WriteLine("📥 收到消息：" + msg.Text);
                if (msg.Text != null)
                {
                    // 解析JSON
                    var json = JObject.Parse(msg.Text);


                    var code = json["header"]?["code"]?.ToString();
                    var errorMsg = json["header"]?["message"]?.ToString();

                    if (code != "0")
                    {
                        Console.WriteLine($"❌ AI 错误：{errorMsg} (code: {code})");
                        result.AppendLine($"❌ AI 错误：{errorMsg}");
                        exitEvent.Set(); // 立即退出
                        return;
                    }



                    // 获取内容
                    var content = json["payload"]?["choices"]?[0]?["content"]?.ToString();
                    if (!string.IsNullOrEmpty(content))
                    {
                        result.Append(content);
                    }
                    var status = json["header"]?["status"]?.ToString();
                    Console.WriteLine($"当前状态：{status}");
                    if (status == "2")
                    {
                        Console.WriteLine("✅ AI 返回完毕");
                        exitEvent.Set();
                    }
                }
            });

            client.MessageReceived.Subscribe(msg =>
            {
                Console.WriteLine("📥 收到消息：" + msg.Text);
            });

            client.DisconnectionHappened.Subscribe(info =>
            {
                Console.WriteLine($"❌ 断开连接: {info.Type} {info.Exception?.Message}");
            });

            client.ReconnectionHappened.Subscribe(info =>
            {
                Console.WriteLine($"🔄 重新连接: {info.Type}");
            });

            Console.WriteLine("🚀 WebSocket 开始连接...");
            await client.Start();
            Console.WriteLine("✅ WebSocket 已连接，准备发送问题...");
            await client.SendInstant(JsonConvert.SerializeObject(message));
            // 等待事件触发 阻塞直到 AI 返回结束 加载一个超时不能超过20秒
            bool completed = exitEvent.WaitOne(TimeSpan.FromSeconds(30));
            if (!completed)
            {
                return "⚠️ AI响应超时（30秒内未返回结果），请简化输入或稍后重试。";
            }
            return result.ToString();

        }
        /// <summary>
        /// HMAC-SHA256签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="base64Secret"></param>
        /// <returns></returns>
        private static string HMACSHA256(string data, string base64Secret)
        {
            var secretBytes = Convert.FromBase64String(base64Secret); // ✅ 正确用法
            using var hmac = new HMACSHA256(secretBytes);
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashBytes);
        }

    }
}

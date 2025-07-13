using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.ChatGLMAi
{
    public class ChatRequest
    {
        public string Model { get; set; } = "glm-4-flash-250414"; // 选择的模型
        public List<ChatMessage> Messages { get; set; } = new(); // 多轮上下文消息
    }
}

namespace CustomerRelationshipManagement.DTOS.ChatGLMAi
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; } // "user" 或 "assistant"
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}

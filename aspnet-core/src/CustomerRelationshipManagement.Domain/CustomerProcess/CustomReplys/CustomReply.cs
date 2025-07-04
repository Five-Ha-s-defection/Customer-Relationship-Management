using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace CustomerRelationshipManagement.CustomerProcess.CustomReplys
{
    /// <summary>
    /// 自定义回复
    /// </summary>
    public class CustomReply:Entity<Guid>
    {
        /// <summary>
        /// 中文内容
        /// </summary>
        public string CustomReplyName { get; set; }

        /// <summary>
        /// 英文内容
        /// </summary>
        public string CustomReplyEnglishName { get; set; }

        /// <summary>
        /// 创建时间 默认为当前时间
        /// </summary>
        public DateTime CreateTime { get; set; }=DateTime.Now;
    }
}

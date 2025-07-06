using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomReplys
{
    public class CreateUpdateCustomReplyDto
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
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}

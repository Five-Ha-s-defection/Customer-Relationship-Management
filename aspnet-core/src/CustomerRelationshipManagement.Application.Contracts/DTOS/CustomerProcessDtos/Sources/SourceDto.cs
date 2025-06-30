using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources
{
    public class SourceDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 线索来源名称
        /// </summary>
        public string ClueSourceName { get; set; } = string.Empty;

        /// <summary>
        /// 状态
        /// 1 启用
        /// 0 禁用
        /// </summary>
        public int ClueSourceStatus { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ClueSourceContent { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}

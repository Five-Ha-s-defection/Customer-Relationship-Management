using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CommunicationTypes
{
    public class CommunicationTypeDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 中文类型名称
        /// </summary>
        public string CommunicationTypeName { get; set; }

        /// <summary>
        /// 英文类型名称
        /// </summary>
        public string CommunicationTypeEnglishName { get; set; }

        /// <summary>
        /// 启用状态 1禁用 0启用 默认0
        /// </summary>
        public int CommunicationTypeStatus { get; set; } = 0;

        /// <summary>
        /// 创建时间 默认当前时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 自定义回复（外键）
        /// </summary>
        public Guid CustomReplyId { get; set; }
    }
}

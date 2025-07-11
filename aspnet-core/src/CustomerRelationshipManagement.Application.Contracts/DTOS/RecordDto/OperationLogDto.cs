using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.RecordDto
{
    public class OperationLogDto: CreationAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BizType { get; set; } = string.Empty;
        /// <summary>
        /// 业务Id
        /// </summary>
        public Guid BizId { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public string Action { get; set; } = string.Empty;
        /// <summary>
        /// 操作人
        /// </summary>
        public string CreatorName { get; set; } = string.Empty;
    }
}

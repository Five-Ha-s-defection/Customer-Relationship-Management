using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers
{
    /// <summary>
    /// 修改客户分配、领取、放弃状态Dto
    /// </summary>
    public class CustomerActionDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        //用于在模型绑定时验证字符串格式是否符合要求
        [RegularExpression("assign|receive|abandon", ErrorMessage = "操作类型仅支持 assign、receive 或 abandon")]
        public string ActionType { get; set; }

        // 仅在 assign 操作时需要
        public Guid? TargetUserId { get; set; }

        // 仅在 abandon 操作时需要
        public string? AbandonReason { get; set; }  // 放弃原因
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues
{
    /// <summary>
    /// 修改线索分配、领取、放弃状态Dto
    /// </summary>
    public class ClueActionDto
    {
        [Required]
        public Guid ClueId { get; set; }

        [Required]
        //用于在模型绑定时验证字符串格式是否符合要求
        [RegularExpression("assign|receive|abandon", ErrorMessage = "操作类型仅支持 assign、receive 或 abandon")]
        public string ActionType { get; set; }

        // 仅在 assign 操作时需要
        public Guid? TargetUserId { get; set; }
    }
}

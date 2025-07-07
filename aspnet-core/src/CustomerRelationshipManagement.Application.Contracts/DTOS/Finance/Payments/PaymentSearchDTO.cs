using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.Finance.Payments
{
    public class PaymentSearchDTO : PagingInfo
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }


        /// <summary>
        /// 所属客户
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 关联合同
        /// </summary>
        public Guid? ContractId { get; set; }

        /// <summary>
        /// 收款编号
        /// </summary>
        public string? PaymentCode { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public Guid? PaymentMethod { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        public string? PaymentDate { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public int? PaymentStatus { get; set; } = 0;

        /// <summary>
        /// 每一级的审批人 ID
        /// </summary>
        public List<Guid> ApproverIds { get; set; } = new();
        /// <summary>
        /// 当前审批到第几人（从0开始）
        /// </summary>
        public int CurrentStep { get; set; } = 0;
        /// <summary>
        /// 审批意见
        /// </summary>
        public List<string> ApproveComments { get; set; } = new();
        /// <summary>
        /// 审批时间
        /// </summary>
        public List<DateTime> ApproveTimes { get; set; } = new();

    }
}

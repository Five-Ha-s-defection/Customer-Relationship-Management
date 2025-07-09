using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;

namespace CustomerRelationshipManagement.DTOS.Finance.Payments
{
    [CacheName("Payment")]
    public class PaymentDTO: FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 所属客户
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 关联合同
        /// </summary>
        public Guid ContractId { get; set; }

        /// <summary>
        /// 关联应收（可选）
        /// </summary>
        public Guid ReceivableId { get; set; } = Guid.Empty;

        /// <summary>
        /// 负责人
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 收款编号
        /// </summary>
        public string PaymentCode { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public Guid PaymentMethod { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 审批人顺序列表
        /// </summary>
        public List<Guid> ApproverIds { get; set; } = new();

        /// <summary>
        /// 当前审批到第几人（从0开始）
        /// </summary>
        public int CurrentStep { get; set; } = 0;

        /// <summary>
        /// 审批意见（顺序与审批人一致）
        /// </summary>
        public List<string> ApproveComments { get; set; } = new();

        /// <summary>
        /// 审批时间（顺序与审批人一致）
        /// </summary>
        public List<DateTime> ApproveTimes { get; set; } = new();

        /// <summary>
        /// 收款状态：0-待审，1-审核中，2-通过，3-拒绝
        /// </summary>
        public int PaymentStatus { get; set; } = 0;

        /// <summary>
        /// 备注（可选）
        /// </summary>
        public string Remark { get; set; } = string.Empty;




        /// <summary>
        /// 应收款金额
        /// </summary>
        public decimal? ReceivablePay { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; }

        public string RealName { get; set; }

        /// <summary>
        /// 创建者真实姓名
        /// </summary>
        public string CreatorRealName { get; set; }

        /// <summary>
        /// 审核人姓名（逗号分隔）
        /// </summary>
        public string AuditorNames { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public string PaymentMethodName { get; set; }

        /// <summary>
        /// 当前审核人
        /// </summary>
        public string CurrentAuditorName { get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.crmcontracts
{
    /// <summary>
    /// 合同表设计
    /// </summary>
    public class CrmContract : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 所属客户ID
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 选择商机ID
        /// </summary>
        public Guid BusinessOpportunityId { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 签订日期
        /// </summary>
        public DateTime SignDate { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; } = string.Empty;

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime CommencementDate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 经销商
        /// </summary>
        public string Dealer { get; set; } = string.Empty;

        /// <summary>
        /// 合同条款
        /// </summary>
        public string ContractTerms { get; set; } = string.Empty;

        /// <summary>
        /// 审核人ID
        /// </summary>
        public IList<Guid> AuditorId { get; set; } = new List<Guid>();

        /// <summary>
        /// 合同扫描件(图片)
        /// </summary>
        public string ContractScanning { get; set; } = string.Empty;

        /// <summary>
        /// 上传附件
        /// </summary>
        public string Attachment { get; set; } = string.Empty;

        /// <summary>
        /// 合同收款
        /// </summary>
        public decimal ContractProceeds { get; set; } = 0;

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
        /// <summary>
        /// 收款状态
        /// </summary>
        public int PaymentStatus { get; set; } = 0; // 0-待审核，1-审核中，2-已通过，3-已拒绝
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;

namespace CustomerRelationshipManagement.DTOS.Finance.Incoices
{
    /// <summary>
    /// 发票数据传输对象
    /// </summary>
    [CacheName("Invoice")]
    public class InvoiceDTO : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 所属客户ID
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 关联合同ID
        /// </summary>
        public Guid ContractId { get; set; }

        /// <summary>
        /// 关联收款ID
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 发票编号
        /// </summary>
        public string InvoiceNumberCode { get; set; }

        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// 开票时间
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// 开票类型
        /// </summary>
        public int InvoiceType { get; set; }

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

        /// <summary>
        /// 开票状态
        /// </summary>
        public int InvoiceStatus { get; set; }

        /// <summary>
        /// 发票图片
        /// </summary>
        public string InvoiceImg { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;


        /// <summary>
        /// 已有的发票信息
        /// </summary>
        public Guid InvoiceInformationId { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        public string TaxNumber { get; set; }

        public string Bank { get; set; } = string.Empty;

        /// <summary>
        /// 开户地址
        /// </summary>
        public string BillingAddress { get; set; } = string.Empty;

        /// <summary>
        /// 开户账号
        /// </summary>
        public string BankAccount { get; set; } = string.Empty;

        /// <summary>
        /// 开票电话
        /// </summary>
        public string BillingPhone { get; set; } = string.Empty;


        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal? PaymentAmount { get; set;}
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
        /// 已有发票信息名称
        /// </summary>
        public string InoviceTitle { get; set; }


        /// <summary>
        /// 当前审核人
        /// </summary>
        public string CurrentAuditorName { get; set; }
    }
} 
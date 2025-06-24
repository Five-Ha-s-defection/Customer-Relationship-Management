using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.Invoices
{
    /// <summary>
    /// 发票实体类
    /// 继承FullAuditedAggregateRoot以获取审计字段和聚合根功能
    /// </summary>
    public class Invoice : FullAuditedAggregateRoot<Guid>
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
        public Guid PaymentId { get; set; } = Guid.Empty;

        /// <summary>
        /// 负责人ID
        /// </summary>
        public Guid UserId { get; set; }

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
        /// 开票类型（增值税普通发票、增值税专用发票等）
        /// </summary>
        public int InvoiceType { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public Guid ApproverId { get; set; }

        /// <summary>
        /// 开票状态
        /// </summary>
        public int InvoiceStatus { get; set; }= 0;// 0:待审核 1:审核通过 2:审核未通过

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
        public Guid InvoiceInformationId { get; set; } = Guid.Empty;

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



    }
}

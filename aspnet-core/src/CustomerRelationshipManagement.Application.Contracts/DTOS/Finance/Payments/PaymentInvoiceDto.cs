using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.Finance.Payments
{
    public class PaymentInvoiceDto
    {

        /// <summary>
        /// 关联收款ID
        /// </summary>
        public Guid PaymentId { get; set; } = Guid.Empty;
        /// <summary>
        /// 发票编号
        /// </summary>
        public string InvoiceNumberCode { get; set; }


        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 开票时间
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// 开票类型（增值税普通发票、增值税专用发票等）
        /// </summary>
        public int InvoiceType { get; set; }

        /// <summary>
        /// 开票状态
        /// </summary>
        public int InvoiceStatus { get; set; } = 0;// 0:待审核 1:审核通过 2:审核未通过


    }
}

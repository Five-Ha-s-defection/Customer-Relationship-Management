using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Payments
{
    public class CreateUpdatePaymentDTO
    {
        /// <summary>
        /// 所属客户
        /// </summary>
        [Required]
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
        /// 审核人
        /// </summary>
        public Guid ApproverId { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        public int PaymentStatus { get; set; } = 0;

        /// <summary>
        /// 备注（可选）
        /// </summary>
        public string Remark { get; set; } = string.Empty;
    }
}

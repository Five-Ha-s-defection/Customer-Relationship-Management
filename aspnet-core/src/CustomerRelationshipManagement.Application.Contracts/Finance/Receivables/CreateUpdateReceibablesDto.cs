using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.FinanceInfo.Finance
{
    public class CreateUpdateReceibablesDto
    {
        /// <summary>
        /// 所属客户
        /// </summary>
        [Required]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 关联合同
        /// </summary>
        [Required]
        public Guid ContractId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 应收款编号（可选，系统会自动生成）
        /// </summary>
        public string? ReceivableCode { get; set; }

        /// <summary>
        /// 应收款金额
        /// </summary
        [Required]
        public decimal ReceivablePay { get; set; }

        /// <summary>
        /// 应收款时间
        /// </summary>
        [Required]
        public DateTime ReceivableDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        
        public string Remark { get; set; }
    }
}

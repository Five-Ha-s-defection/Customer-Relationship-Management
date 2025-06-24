using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.Finance
{
    public class Receivables:FullAuditedAggregateRoot<Guid>
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
        /// 负责人
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 应收款编号
        /// </summary>
        public string ReceivableCode { get; set; } = string.Empty;

        /// <summary>
        /// 应收款金额
        /// </summary>
        public decimal ReceivablePay { get; set; }

        /// <summary>
        /// 应收款时间
        /// </summary>
        public DateTime ReceivableDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

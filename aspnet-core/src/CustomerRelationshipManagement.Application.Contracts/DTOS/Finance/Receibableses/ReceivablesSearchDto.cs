using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.Finance.Receibableses
{
    public class ReceivablesSearchDto: PagingInfo
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
        /// 应收款编号
        /// </summary>
        public string? ReceivableCode { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreateId { get; set; }

        /// <summary>
        /// 所属客户
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 关联合同
        /// </summary>
        public Guid? ContractId { get; set; }

        /// <summary>
        /// 应收款金额
        /// </summary>
        public decimal? ReceivablePay { get; set; }

        /// <summary>
        /// 应收款时间
        /// </summary>
        public DateTime? ReceivableDate { get; set; }

    }
}

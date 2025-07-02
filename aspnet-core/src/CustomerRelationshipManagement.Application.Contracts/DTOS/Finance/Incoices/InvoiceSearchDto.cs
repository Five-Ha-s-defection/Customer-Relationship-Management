using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.Finance.Incoices
{
    public class InvoiceSearchDto: PagingInfo
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
        /// 负责人ID
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 所属客户ID
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 关联合同ID
        /// </summary>
        public Guid? ContractId { get; set; }



        /// <summary>
        /// 发票编号
        /// </summary>
        public string? InvoiceNumberCode { get; set; }



        /// <summary>
        /// 开票类型
        /// </summary>
        public int? InvoiceType { get; set; }

        /// <summary>
        /// 开票时间
        /// </summary>
        public DateTime? InvoiceDate { get; set; }



        /// <summary>
        /// 开票状态
        /// </summary>
        public int? InvoiceStatus { get; set; }

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

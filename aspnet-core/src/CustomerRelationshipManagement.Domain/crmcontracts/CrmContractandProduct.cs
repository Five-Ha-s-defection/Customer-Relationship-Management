using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Settings;

namespace CustomerRelationshipManagement.crmcontracts
{
    /// <summary>
    /// 合同产品关系表
    /// </summary>
    public class CrmContractandProduct : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 合同表id
        /// </summary>
        public Guid CrmContractId { get; set; } = new Guid();

        /// <summary>
        /// 产品表id
        /// </summary>
        public Guid ProductId { get; set; } = new Guid();

        /// <summary>
        /// 购买数量
        /// </summary>
        public int BuyProductNum { get; set; } = 0;

        /// <summary>
        /// 售价
        /// </summary>
        public decimal SellPrice { get; set; } = 0;

        /// <summary>
        /// 合计
        /// </summary>
        public decimal SumPrice { get; set; } = 0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;

namespace CustomerRelationshipManagement.DTOS.Finance.Receibableses
{
    [CacheName("Receivables")]
    public class ReceivablesDTO:FullAuditedEntityDto<Guid>
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
        public string ReceivableCode { get; set; }

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
        /// <summary>
        /// 关联的收款单
        /// </summary>
        public Guid PaymentId { get; set; }



        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Amount { get; set; }





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


    }
}

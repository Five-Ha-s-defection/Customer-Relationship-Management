using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.Dtos.CrmContractDtos
{
    /// <summary>
    /// 实现合同dto
    /// </summary>
    public class ShowCrmContractDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 所属客户ID
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 选择商机ID
        /// </summary>
        public Guid BusinessOpportunityId { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 签订日期
        /// </summary>
        public DateTime SignDate { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; } = string.Empty;

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime CommencementDate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 经销商
        /// </summary>
        public string Dealer { get; set; } = string.Empty;

        /// <summary>
        /// 合同条款
        /// </summary>
        public string ContractTerms { get; set; } = string.Empty;

        /// <summary>
        /// 审核人ID
        /// </summary>
        public Guid AuditorId { get; set; }

        /// <summary>
        /// 合同扫描件(图片)
        /// </summary>
        public string ContractScanning { get; set; } = string.Empty;

        /// <summary>
        /// 上传附件
        /// </summary>
        public string Attachment { get; set; } = string.Empty;

        /// <summary>
        /// 合同收款
        /// </summary>
        public decimal ContractProceeds { get; set; }



        /// <summary>
        /// 产品ID
        /// </summary>
        public Guid ProductId { get; set; }

    }
}

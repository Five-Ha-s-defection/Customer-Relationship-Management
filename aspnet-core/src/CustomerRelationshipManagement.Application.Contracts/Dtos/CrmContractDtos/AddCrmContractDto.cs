using CustomerRelationshipManagement.Finance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.Dtos.CrmContractDtos
{
    /// <summary>
    /// 添加修改合同Dto
    /// </summary>
    public class AddCrmContractDto: FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 所属客户ID
        /// </summary>
        [Required]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 选择商机ID
        /// </summary>
        public Guid BusinessOpportunityId { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 签订日期
        /// </summary>
        [Required]
        public DateTime SignDate { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        [Required]
        public string ContractName { get; set; } 

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime CommencementDate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        [Required]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 经销商
        /// </summary>
        [Required]
        public string Dealer { get; set; }

        /// <summary>
        /// 合同条款
        /// </summary>
        [Required]
        public string ContractTerms { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        [Required]
        public IList<Guid> AuditorIds { get; set; }

        /// <summary>
        /// 合同扫描件(图片)
        /// </summary>
        public string ContractScanning { get; set; }

        /// <summary>
        /// 上传附件
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 合同收款
        /// </summary>
        public decimal ContractProceeds { get; set; }



        /// <summary>
        /// 产品ID
        /// </summary>
        [Required]
        public IList<Guid> ProductId { get; set; }



        public CreateUpdateReceibablesDto CreateUpdateReceibablesDto { get; set; } = new CreateUpdateReceibablesDto();

    }
}

using CustomerRelationshipManagement.DTOS.Finance.Receibableses;
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
    /// 添加合同Dto
    /// </summary>
    public class AddCrmContractDto
    {
        #region 合同表的字段
        /// <summary>
        /// 所属客户ID
        /// </summary>

        public Guid? CustomerId { get; set; } = new Guid();

        /// <summary>
        /// 选择商机ID
        /// </summary>
        public Guid? BusinessOpportunityId { get; set; } = new Guid();

        /// <summary>
        /// 负责人ID
        /// </summary>

        public Guid? UserId { get; set; } = new Guid();

        /// <summary>
        /// 签订日期
        /// </summary>

        public DateTime? SignDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 合同名称
        /// </summary>

        public string? ContractName { get; set; } = string.Empty;

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime? CommencementDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 截止日期
        /// </summary>

        public DateTime? ExpirationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 经销商
        /// </summary>

        public string? Dealer { get; set; } = string.Empty;

        /// <summary>
        /// 合同条款
        /// </summary>

        public string? ContractTerms { get; set; } = string.Empty;

        /// <summary>
        /// 审核人ID
        /// </summary>

        public IList<Guid>? AuditorIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 合同扫描件(图片)
        /// </summary>
        public string? ContractScanning { get; set; } = string.Empty;

        /// <summary>
        /// 上传附件
        /// </summary>
        public string? Attachment { get; set; } = string.Empty;

        /// <summary>
        /// 合同收款
        /// </summary>
        public decimal? ContractProceeds { get; set; } = 0;

        /// <summary>
        /// 当前审批到第几人（从0开始）
        /// </summary>
        public int? CurrentStep { get; set; } = 0;
        /// <summary>
        /// 审批意见
        /// </summary>
        public List<string>? ApproveComments { get; set; } = new List<string>();
        /// <summary>
        /// 审批时间
        /// </summary>
        public List<DateTime>? ApproveTimes { get; set; } = new List<DateTime>();
        /// <summary>
        /// 收款状态
        /// </summary>
        public int? PaymentStatus { get; set; } = 0; // 0-待审核，1-审核中，2-已通过，3-已拒绝

        #endregion


        #region 产品表的字段

        /// <summary>
        /// 产品信息
        /// </summary>

        public IList<AddCrmcontractandProductDto>? AddCrmcontractandProductDto { get; set; } = new List<AddCrmcontractandProductDto>();

        #endregion


        #region 收款表
        /// <summary>
        /// 添加应收款表信息
        /// </summary>

        public CreateUpdateReceibablesDto? CreateUpdateReceibablesDto { get; set; } = new CreateUpdateReceibablesDto();

        #endregion
    }
}

using CustomerRelationshipManagement.crmcontracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.Dtos.CrmContractDtos
{
    /// <summary>
    /// 获取合同表数据
    /// </summary>
    public class GetCrmContractDto: FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 显示合同表
        /// </summary>
        public ShowCrmContractDto showCrmContractDto {  get; set; } = new ShowCrmContractDto();

        /// <summary>
        /// 合同产品关系表
        /// </summary>
        public IList<CrmContractandProduct> crmContractandProducts { get; set; } = new List<CrmContractandProduct>();
    }
}

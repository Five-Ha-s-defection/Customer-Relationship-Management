using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Dtos.CrmContractDtos
{
    public class PageCrmContractDto: PagingInfo
    {
        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; } = string.Empty;
    }
}

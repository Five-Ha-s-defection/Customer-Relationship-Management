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
        /// 查询的满足条件 0:不使用高级搜索 1:使用全部满足条件搜索 2:部分满足条件搜索
        /// </summary>
        public int CheckType {  get; set; } = 0;

        /// <summary>
        /// 负责人
        /// </summary>
        public IList<Guid> UserIds { get; set; } = new List<Guid>();

        public IList<Guid> 

        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; } = string.Empty;
    }
}

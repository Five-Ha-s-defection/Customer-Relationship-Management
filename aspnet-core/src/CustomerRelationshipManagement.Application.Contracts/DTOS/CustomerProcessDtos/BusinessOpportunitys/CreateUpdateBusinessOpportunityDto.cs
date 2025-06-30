using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys
{
    public class CreateUpdateBusinessOpportunityDto
    {
        /// <summary>
        /// 所属客户
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public Guid PriorityId { get; set; }

        /// <summary>
        /// 商机名称
        /// </summary>
        public string BusinessOpportunityName { get; set; }

        /// <summary>
        /// 销售进度
        /// </summary>
        public Guid SalesProgressId { get; set; }

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// 预计成交日期
        /// </summary>
        public DateTime ExpectedDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid ProductId { get; set; }
    }
}

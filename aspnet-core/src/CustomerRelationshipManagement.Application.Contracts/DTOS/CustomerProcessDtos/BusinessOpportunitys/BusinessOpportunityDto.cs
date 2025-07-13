using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys
{
    public class BusinessOpportunityDto:FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 所属客户
        /// </summary>
        public Guid CustomerId { get; set; }

        //===============================================================================
        /// <summary>
        /// 最后跟进时间(线索外键)
        /// </summary>
        public DateTime? LastFollowTime { get; set; }

        /// <summary>
        /// 下次联系时间(线索外键)
        /// </summary>
        public DateTime? NextContactTime { get; set; }

        /// <summary>
        /// 客户负责人(外键)
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 客户负责人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }


        /// <summary>
        /// 优先级
        /// </summary>
        public Guid PriorityId { get; set; }

        //===============================================================================
        /// <summary>
        /// 优先级名称
        /// </summary>
        public string PriorityName { get; set; }

        /// <summary>
        /// 商机名称
        /// </summary>
        public string BusinessOpportunityName { get; set; }

        /// <summary>
        /// 销售进度
        /// </summary>
        public Guid SalesProgressId { get; set; }

        //===============================================================================
        /// <summary>
        /// 销售进度名称
        /// </summary>
        public string SalesProgressName { get; set; }

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

        /// <summary>
        /// 门幅
        /// </summary>
        public string ProductBrand { get; set; }

        //======================================================================================
        /// <summary>  
        /// 创建人姓名（createId外键 连接 用户表Id）  
        /// </summary>  
        public string CreateName { get; set; }

        /// <summary>
        /// 商机编号
        /// </summary>
        public string BusinessOpportunityCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CrmContractDtos
{
    public class ContractProductDto
    {
        /// <summary>
        /// 合同表id
        /// </summary>
        public Guid CrmContractId { get; set; } = new Guid();

        /// <summary>
        /// 产品编号
        /// </summary>
        public string? ProductCode { get; set; }

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


        /// <summary>
        /// 产品图片
        /// </summary>
        public string? ProductImageUrl { get; set; }

        /// <summary>
        /// 产品门幅
        /// </summary>
        public string ProductBrand { get; set; } = string.Empty;
    }
}

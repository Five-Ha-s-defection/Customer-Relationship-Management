using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues
{
    /// <summary>
    /// 线索转换客户DTO
    /// </summary>
    public class ClueConversionDto
    {
        public Guid ClueId { get; set; }
        public ConversionType ConversionType { get; set; } // 转换类型

        // 以下字段仅用于新建客户
        public Guid CustomerLevelId { get; set; }
        public Guid CustomerRegionId { get; set; }
        public Guid CustomerTypeId { get; set; }
        public Guid CustomerSourceId { get; set; }
        public Guid CarFrameNumberId { get; set; }

        public decimal? CheckAmount { get; set; }
        public DateTime CustomerExpireTime { get; set; }
        public bool CreateContact { get; set; } = false;

        public bool SyncContactRecord { get; set; } // 是否同步联系记录
        public bool CreateNewContact { get; set; } // 是否创建新的联系人

        //新建联系人字段（可选）
        public CreateUpdateCustomerContactDto? ContactInfo { get; set; }
    }

    public enum ConversionType
    {
        CreateNewCustomer, // 创建新客户
        LinkToExistingCustomer, // 关联到现有客户
    }
}

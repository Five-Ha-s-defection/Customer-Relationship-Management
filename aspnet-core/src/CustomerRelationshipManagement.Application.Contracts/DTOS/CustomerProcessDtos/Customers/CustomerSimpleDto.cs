using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers
{
    // 新增：只包含五个字段的简化DTO
    public class CustomerSimpleDto
    {
        public Guid Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

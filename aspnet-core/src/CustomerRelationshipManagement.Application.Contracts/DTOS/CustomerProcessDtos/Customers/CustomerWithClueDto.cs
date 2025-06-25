using CustomerRelationshipManagement.CustomerProcess.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers
{
    public class CustomerWithClueDto
    {
        public Customer Customer { get; set; }
        //======================================================================================
        /// <summary>
        /// 微信号(线索外键)
        /// </summary>
        public string ClueWechat { get; set; }
        /// <summary>
        /// 最后跟进时间(线索外键)
        /// </summary>
        public DateTime LastFollowTime { get; set; }

        /// <summary>
        /// 下次联系时间(线索外键)
        /// </summary>
        public DateTime NextContactTime { get; set; }
    }
}

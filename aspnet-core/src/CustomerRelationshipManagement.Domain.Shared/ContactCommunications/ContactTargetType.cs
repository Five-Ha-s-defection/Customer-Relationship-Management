using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.ContactCommunications
{
    /// <summary>
    /// 联系对象类型
    /// </summary>
    public enum ContactTargetType
    {
        All = 0,      // 全部
        Customer = 1, // 客户
        CustomerPool = 2, // 客户池
        Business = 3, // 商机
        Clue = 4,     // 线索
        CluePool = 5  // 线索池
    }
}

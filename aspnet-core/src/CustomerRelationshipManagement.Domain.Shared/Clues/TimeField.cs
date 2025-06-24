using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Clues
{
    public enum TimeField
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        CreateTime,
        /// <summary>
        /// 下次联系时间
        /// </summary>
        NextContact,
        /// <summary>
        /// 最近跟进时间
        /// </summary>
        LastFollow
    }
}

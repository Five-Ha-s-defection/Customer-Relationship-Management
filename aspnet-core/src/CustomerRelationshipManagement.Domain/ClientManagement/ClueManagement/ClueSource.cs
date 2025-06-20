using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.ClientManagement.ClueManagement
{
    /// <summary>
    /// 线索/客户来源
    /// </summary>
    public class ClueSource
    {
        /// <summary>
        /// 线索来源名称
        /// </summary>
        public string? ClueSourceName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int ClueSourceStatus { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string? ClueSourceContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public decimal? CreateTime { get; set; }
    }
}

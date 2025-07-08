using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.Finance.FinanceAi
{
    /// <summary>
    /// ai 请求参数
    /// </summary>
    public class AiRequestDto
    {
        /// <summary>
        ///  输入的 AI 问题内容 
        /// </summary>
        public string Input { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues
{
    public class SearchCustomerDto
    {
        /// <summary>
        /// 关键词（姓名或手机号）
        /// </summary>
        public string? Keyword { get; set; }
    }
}

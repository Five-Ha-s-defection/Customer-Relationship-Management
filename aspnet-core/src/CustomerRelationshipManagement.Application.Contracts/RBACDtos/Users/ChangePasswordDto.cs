using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Users
{
    public class ChangePasswordDto
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        public string CurrentPassword { get; set; }= string.Empty;
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;
    }
}

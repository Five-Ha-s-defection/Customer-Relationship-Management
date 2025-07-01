using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Users
{
    public class UserDto
    {
        /// <summary>  
        /// 用户信息  
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>  
        /// 用户名  
        /// </summary>  
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }=string.Empty;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string? PhoneInfo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBACDtos.Users
{
    public class UserInfoDto
    {
        /// <summary>  
        /// 用户信息  
        /// </summary>
        public Guid UserId { get; set; } 
        /// <summary>  
        /// 用户名  
        /// </summary>  
        public string UserName { get; set; } = string.Empty;

        /// <summary>  
        /// 邮箱  
        /// </summary>  
        public string Email { get; set; } = string.Empty;

        /// <summary>  
        /// 手机号码  
        /// </summary>  
        public string PhoneInfo { get; set; } = string.Empty;

        /// <summary>  
        /// 是否激活  
        /// </summary>  
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 登录令牌
        /// </summary>
        public string? Token { get; set; }
    }
}

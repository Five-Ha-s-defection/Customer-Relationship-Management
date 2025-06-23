using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Dtos.Users
{
    public class CreateOrUpdateUserInfoDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneInfo { get; set; } = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// 确认密码
        /// </summary>
        public string? ConfirmPassword { get; set; } = string.Empty;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; } = string.Empty;

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}

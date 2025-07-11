using System;

namespace CustomerRelationshipManagement.RBACDtos.Users
{
    /// <summary>
    /// 登录结果Dto
    /// </summary>
    public class LoginResultDto
    {
        /// <summary>
        /// 登录成功返回的token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// token的过期时间(单位:秒)
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 当前的登录用户信息包含（角色、权限、菜单等信息）
        /// </summary>
        public UserInfoDto User { get; set; }
    }
}

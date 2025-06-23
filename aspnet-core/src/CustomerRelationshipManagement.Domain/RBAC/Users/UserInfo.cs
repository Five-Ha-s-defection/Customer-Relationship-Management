using CustomerRelationshipManagement.RBAC.UserPermissions;
using CustomerRelationshipManagement.RBAC.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CustomerRelationshipManagement.RBAC.Users
{
    /// <summary>  
    /// 用户信息  
    /// </summary>  
    public class UserInfo : FullAuditedAggregateRoot<Guid>
    {
        
        /// <summary>  
        /// 用户名  
        /// </summary>  
        public string UserName { get; set; } = string.Empty;

        /// <summary>  
        /// 密码  
        /// </summary>  
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 确定密码
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;

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
        /// 用户角色导航属性  
        /// </summary>  
        public ICollection<UserRoleInfo> UserRoles { get; set; } = new List<UserRoleInfo>();

        /// <summary>  
        /// 用户权限导航属性  
        /// </summary>  
        public ICollection<UserPermissionInfo> UserPermissions { get; set; } = new List<UserPermissionInfo>();
    }
}

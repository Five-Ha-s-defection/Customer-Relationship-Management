using CustomerRelationshipManagement.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues
{
    public class GetUserRoleDto 
    {
        /// <summary>
        /// 用户角色Id
        /// </summary>
        public Guid UserRoleId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; } = string.Empty;
        /// <summary>  
        /// 邮箱  
        /// </summary>  
        public string Email { get; set; } = string.Empty;

        /// <summary>  
        /// 手机号码  
        /// </summary>  
        public string PhoneInfo { get; set; } = string.Empty;

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = string.Empty;
    }
}

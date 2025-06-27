using CustomerRelationshipManagement.RBACDtos.Menus;
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
        public Guid Id { get; set; } 
        /// <summary>  
        /// 用户名  
        /// </summary>  
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; } = string.Empty;

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }
        /// <summary>
        /// 用户的所有角色名称的集合
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// 用户的所有权限编码的集合
        /// </summary>
        public List<string> Permissions { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public List<MenuDto> Menus { get; set; }


    }
}

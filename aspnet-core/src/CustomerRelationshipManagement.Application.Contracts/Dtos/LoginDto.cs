using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Dtos
{
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DefaultValue("admin")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [DefaultValue("123")]
        public string Password { get; set; }= string.Empty;
    }
}

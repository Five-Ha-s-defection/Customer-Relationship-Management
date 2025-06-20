using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.ClientManagement.CustomerContactManagement
{
    /// <summary>
    /// 联系人
    /// </summary>
    public class CustomerContact
    {
        /// <summary>
        /// 所属客户ID（外键）
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string? ContactName { get; set; }
        /// <summary>
        /// 联系人关系
        /// </summary>
        public string? ContactRelation { get; set; } 
        /// <summary>
        /// 角色
        /// </summary>
        public int RoleId { get; set; }            
        /// <summary>
        /// 尊称（先生/女士）
        /// </summary>
        public bool Salutation { get; set; }      
        /// <summary>
        /// 职位
        /// </summary>
        public string? Position { get; set; }       
        /// <summary>
        /// 手机
        /// </summary>
        public string? ContactMobile { get; set; }          
        /// <summary>
        /// QQ
        /// </summary>
        public string? ContactQQ { get; set; }              
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? ContactEmail { get; set; }           
        /// <summary>
        /// 微信号
        /// </summary>
        public string? ContactWechat { get; set; }          
        /// <summary>
        /// 备注
        /// </summary>
        public string? ContactRemark { get; set; }         
    }
}

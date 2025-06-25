using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/CustomerManegement/ClueSources/ClueSource.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManegement.ClueSources
========
namespace CustomerRelationshipManagement.CustomerProcess.ClueSources
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Domain/CustomerProcess/ClueSources/ClueSource.cs
{
    /// <summary>
    /// 线索来源表
    /// </summary>
    public class ClueSource:Entity<Guid>
    {
        /// <summary>
        /// 线索来源名称
        /// </summary>
        public string ClueSourceName { get; set; }=string.Empty;

        /// <summary>
        /// 状态
        /// 1 启用
        /// 0 禁用
        /// </summary>
        public int ClueSourceStatus { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ClueSourceContent { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }=DateTime.Now;

    }
}

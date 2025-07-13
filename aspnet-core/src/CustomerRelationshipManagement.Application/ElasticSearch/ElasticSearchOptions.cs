using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.ElasticSearch
{
    /// <summary>
    /// ElasticSearch配置选项
    /// </summary>
    public class ElasticSearchOptions
    {
        /// <summary>
        /// ElasticSearch服务器地址
        /// </summary>
        public string Uri { get; set; } = "http://localhost:9200";

        /// <summary>
        /// 索引名称
        /// </summary>
        public string Index { get; set; } = "crmsystem";

        /// <summary>
        /// 用户名（如果需要认证）
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 密码（如果需要认证）
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 连接超时时间（秒）
        /// </summary>
        public int Timeout { get; set; } = 30;

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public bool EnableSsl { get; set; } = false;

        /// <summary>
        /// 索引分片数
        /// </summary>
        public int NumberOfShards { get; set; } = 1;

        /// <summary>
        /// 索引副本数
        /// </summary>
        public int NumberOfReplicas { get; set; } = 0;
    }
} 
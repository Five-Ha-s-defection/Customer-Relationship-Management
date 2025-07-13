using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.ElasticSearch
{
    /// <summary>
    /// ElasticSearch客户端提供者
    /// 确保整个应用只有一个ES客户端实例 避免重复创建连接，节省资源
    /// </summary>
    public class ElasticSearchClientProvider
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _options;

        public ElasticSearchClientProvider(IOptions<ElasticSearchOptions> options)
        {
            _options = options.Value;
            _elasticClient = CreateElasticClient();
        }

        /// <summary>
        /// 获取ElasticSearch客户端
        /// </summary>
        public IElasticClient GetClient()
        {
            return _elasticClient;
        }

        /// <summary>
        /// 创建ElasticSearch客户端
        /// </summary>
        private IElasticClient CreateElasticClient()
        {
            var settings = new ConnectionSettings(new Uri(_options.Uri))
                .DefaultIndex(_options.Index)
                .RequestTimeout(TimeSpan.FromSeconds(_options.Timeout))
                .EnableDebugMode()
                .DisableDirectStreaming();

            // 如果配置了认证信息
            if (!string.IsNullOrEmpty(_options.Username) && !string.IsNullOrEmpty(_options.Password))
            {
                settings.BasicAuthentication(_options.Username, _options.Password);
            }

            // 如果启用SSL
            if (_options.EnableSsl)
            {
                settings.ServerCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => true);
            }

            return new ElasticClient(settings);
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _elasticClient.PingAsync();
                return response.IsValid;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        public async Task<bool> CreateIndexAsync(string indexName)
        {
            try
            {
                //创建索引的分片和副本数量
                var indexSettings = new IndexSettings
                {
                    NumberOfShards = _options.NumberOfShards,
                    NumberOfReplicas = _options.NumberOfReplicas
                };

                //Indices.CreateAsync异步创建索引的方法
                var response = await _elasticClient.Indices.CreateAsync(indexName, c => c
                    .Settings(s => s
                        .NumberOfShards(_options.NumberOfShards)
                        .NumberOfReplicas(_options.NumberOfReplicas)
                    )
                );

                return response.IsValid;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查索引是否存在
        /// </summary>
        public async Task<bool> IndexExistsAsync(string indexName)
        {
            try
            {
                var response = await _elasticClient.Indices.ExistsAsync(indexName);
                return response.Exists;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        public async Task<bool> DeleteIndexAsync(string indexName)
        {
            try
            {
                var response = await _elasticClient.Indices.DeleteAsync(indexName);
                return response.IsValid;
            }
            catch
            {
                return false;
            }
        }
    }
} 
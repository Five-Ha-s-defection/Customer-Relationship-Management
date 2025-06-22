using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace CustomerRelationshipManagement.Redis
{
    /// <summary>
    /// Redis缓存服务实现类
    /// 提供基于Redis的分布式缓存功能，支持泛型数据类型的序列化和反序列化
    /// </summary>
    public class RedisCacheService : IRedisCacheService, ITransientDependency
    {
        /// <summary>
        /// 分布式缓存接口，由ABP框架注入
        /// </summary>
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 构造函数，注入分布式缓存服务
        /// </summary>
        /// <param name="cache">分布式缓存接口</param>
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 从缓存中获取指定类型的数据
        /// </summary>
        /// <typeparam name="T">要获取的数据类型</typeparam>
        /// <param name="key">缓存键名</param>
        /// <returns>缓存的数据，如果不存在则返回默认值</returns>
        public async Task<T> GetAsync<T>(string key)
        {
            // 从Redis中获取字符串数据
            var data = await _cache.GetStringAsync(key);
            
            // 如果数据为空，返回默认值
            if (data == null)
                return default;

            // 将JSON字符串反序列化为指定类型
            return JsonSerializer.Deserialize<T>(data);
        }

        /// <summary>
        /// 将数据存储到缓存中
        /// </summary>
        /// <typeparam name="T">要存储的数据类型</typeparam>
        /// <param name="key">缓存键名</param>
        /// <param name="value">要存储的数据</param>
        /// <param name="expiration">过期时间，可选</param>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            // 将对象序列化为JSON字符串
            var json = JsonSerializer.Serialize(value);
            
            // 创建缓存选项
            var options = new DistributedCacheEntryOptions();

            // 如果指定了过期时间，则设置绝对过期时间
            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);

            // 将数据存储到Redis中
            await _cache.SetStringAsync(key, json, options);
        }

        /// <summary>
        /// 从缓存中移除指定键的数据
        /// </summary>
        /// <param name="key">要移除的缓存键名</param>
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        /// <summary>
        /// 获取或添加缓存数据
        /// 如果缓存中存在数据则直接返回，否则执行工厂方法获取数据并缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键名</param>
        /// <param name="factory">数据工厂方法，当缓存不存在时执行</param>
        /// <param name="expiration">过期时间，可选</param>
        /// <returns>缓存的数据</returns>
        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            // 尝试从缓存中获取现有数据
            var existing = await GetAsync<T>(key);
            
            // 如果缓存中存在有效数据，直接返回
            if (existing != null && !existing.Equals(default(T)))
                return existing;

            // 缓存中不存在数据，执行工厂方法获取新数据
            var value = await factory();
            
            // 将新数据存储到缓存中
            await SetAsync(key, value, expiration);
            
            // 返回新获取的数据
            return value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.Redis
{
    /// <summary>
    /// Redis缓存服务接口
    /// 定义Redis缓存操作的基本契约，支持泛型数据类型的序列化和反序列化
    /// </summary>
    public interface IRedisCacheService
    {    
        /// <summary>
        /// 从缓存中获取指定类型的数据
        /// </summary>
        /// <typeparam name="T">要获取的数据类型</typeparam>
        /// <param name="key">缓存键名</param>
        /// <returns>缓存的数据，如果不存在则返回默认值</returns>
        Task<T> GetAsync<T>(string key);
        
        /// <summary>
        /// 将数据存储到缓存中
        /// </summary>
        /// <typeparam name="T">要存储的数据类型</typeparam>
        /// <param name="key">缓存键名</param>
        /// <param name="value">要存储的数据</param>
        /// <param name="expiration">过期时间，可选</param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        
        /// <summary>
        /// 从缓存中移除指定键的数据
        /// </summary>
        /// <param name="key">要移除的缓存键名</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// 获取或添加缓存数据
        /// 如果缓存中存在数据则直接返回，否则执行工厂方法获取数据并缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键名</param>
        /// <param name="factory">数据工厂方法，当缓存不存在时执行</param>
        /// <param name="expiration">过期时间，可选</param>
        /// <returns>缓存的数据</returns>
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
    }
}

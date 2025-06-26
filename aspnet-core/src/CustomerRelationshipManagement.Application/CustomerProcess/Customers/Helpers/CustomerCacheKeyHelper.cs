using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CustomerProcess.Customers.Helpers
{
    public static class CustomerCacheKeyHelper
    {
        /// <summary>
        /// 构建可读的缓存Key（推荐：方便调试）
        /// </summary>
        public static string BuildReadableKey(SearchCustomerDto dto)
        {
            return $"CustomerRedis_" +
                   $"Start_{dto.StartTime?.ToString("yyyyMMddHHmmss") ?? "null"}_" +
                   $"End_{dto.EndTime?.ToString("yyyyMMddHHmmss") ?? "null"}_" +
                   $"TimeType_{dto.TimeType?.ToString() ?? "null"}_" +
                   $"OrderBy_{dto.OrderBy?.ToString() ?? "null"}_" +
                   $"OrderDesc_{dto.OrderDesc}_" +
                   $"Keyword_{(string.IsNullOrWhiteSpace(dto.Keyword) ? "All" : dto.Keyword)}_" +
                   $"Page_{dto.PageIndex}_Size_{dto.PageSize}";
        }

        /// <summary>
        /// 构建短缓存Key（哈希版）
        /// </summary>
        public static string BuildHashKey(SearchCustomerDto dto)
        {
            string rawKey = $"{dto.StartTime}_{dto.EndTime}_{dto.TimeType}_{dto.OrderBy}_{dto.OrderDesc}_{dto.Keyword}_{dto.PageIndex}_{dto.PageSize}";
            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
            string hash = BitConverter.ToString(hashBytes).Replace("-", "");
            return $"CustomerRedis_{hash}";
        }
    }
}

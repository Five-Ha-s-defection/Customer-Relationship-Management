using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys.Helps
{
    public class BusinessOpportunityCacheKeyHelper
    {
        /// <summary>
        /// 构建可读的缓存Key（推荐：方便调试）
        /// </summary>
        public static string BuildReadableKey(SearchBusinessOpportunityDto dto)
        {
            string safe(string? input) => string.IsNullOrWhiteSpace(input) ? "null" : input.Trim();
            string joinList(List<Guid>? list) => list == null || list.Count == 0 ? "null" : string.Join("-", list);

            return $"BusinessOpportunity_" +
                   $"PageIndex_{dto.PageIndex}_PageSize_{dto.PageSize}_" +
                   $"Type_{dto.type}_" +
                   $"AssignedTo_{dto.AssignedTo?.ToString() ?? "null"}_" +
                   $"SalesProgress_{joinList(dto.SalesProgressList)}_" +
                   $"Start_{dto.StartTime?.ToString("yyyyMMddHHmmss") ?? "null"}_" +
                   $"End_{dto.EndTime?.ToString("yyyyMMddHHmmss") ?? "null"}_" +
                   $"TimeType_{dto.TimeType?.ToString() ?? "null"}_" +
                   $"OrderBy_{dto.OrderBy?.ToString() ?? "null"}_" +
                   $"OrderDesc_{dto.OrderDesc}_" +
                    $"Keyword_{(string.IsNullOrWhiteSpace(dto.Keyword) ? "All" : dto.Keyword)}" ;
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

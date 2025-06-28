using CustomerRelationshipManagement.Dtos.CrmContractDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CrmContracts.Helpers
{
    /// <summary>
    /// 辅助类，用于构建合同数据在Redis中的缓存Key。
    /// </summary>
    public class CrmContractHelper
    {
        /// <summary>
        /// 构建可读的缓存Key（推荐：方便调试）
        /// 该Key包含了所有合同查询条件，便于在Redis中直接看出缓存内容的含义。
        /// </summary>
        /// <param name="dto">合同查询条件DTO。</param>
        /// <returns>可读的缓存Key字符串。</returns>
        public static string BuildReadableKey(PageCrmContractDto dto)
        {
            // 辅助函数，用于处理可能为空的字符串或Guid列表，确保Key的连贯性
            string safeString(string? input) => string.IsNullOrWhiteSpace(input) ? "null" : input.Trim();
            string safeGuidList(IEnumerable<Guid> guids) => guids != null && guids.Any() ? string.Join(",", guids.OrderBy(g => g)) : "null";
            string safeGuid(Guid id) => id == Guid.Empty ? "null" : id.ToString();

            // 由于时间范围查询可能使用不同的时间类型，这里将所有可能的时间字段都包含进来，
            // 并根据 SearchTimeType 标记哪个时间类型在生效。
            return $"ContractRedis_" +
                   $"ContractName_{safeString(dto.ContractName)}_" +
                   $"CheckType_{dto.CheckType}_" +
                   $"SearchTimeType_{dto.SearchTimeType}_" + // 标记当前时间查询类型
                   $"BeginTime_{(string.IsNullOrWhiteSpace(dto.BeginTime) ? "null" : dto.BeginTime)}_" +
                   $"EndTime_{(string.IsNullOrWhiteSpace(dto.EndTime) ? "null" : dto.EndTime)}_" +
                   $"UserIds_{safeGuidList(dto.UserIds)}_" +
                   $"CreateUserIds_{safeGuidList(dto.CreateUserIds)}_" +
                   $"CustomerId_{safeGuid(dto.CustomerId)}_" +
                   $"SignDate_{safeString(dto.SignDate)}_" +
                   $"CommencementDate_{safeString(dto.CommencementDate)}_" +
                   $"ExpirationDate_{safeString(dto.ExpirationDate)}_" +
                   $"Page_{dto.PageIndex}_Size_{dto.PageSize}";
        }

        /// <summary>
        /// 构建短缓存Key（哈希版）
        /// 该Key将所有合同查询条件进行哈希，生成一个紧凑的Key，适合生产环境。
        /// </summary>
        /// <param name="dto">合同查询条件DTO。</param>
        /// <returns>哈希化的缓存Key字符串。</returns>
        public static string BuildHashKey(PageCrmContractDto dto)
        {
            // 将所有影响查询结果的字段拼接成一个原始字符串
            // 确保字段的顺序和格式是固定的，这样相同的查询会生成相同的Key
            string rawKey = $"{dto.ContractName}_" +
                            $"{dto.CheckType}_" +
                            $"{dto.BeginTime}_" +
                            $"{dto.EndTime}_" +
                            $"{dto.SearchTimeType}_" +
                            $"{string.Join(",", dto.UserIds.OrderBy(g => g))}_" + // 对Guid列表排序以确保一致性
                            $"{string.Join(",", dto.CreateUserIds.OrderBy(g => g))}_" +
                            $"{dto.CustomerId}_" +
                            $"{dto.SignDate}_" +
                            $"{dto.CommencementDate}_" +
                            $"{dto.ExpirationDate}_" +
                            $"{dto.PageIndex}_" +
                            $"{dto.PageSize}";

            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant(); // 统一小写
            return $"ContractRedis_Hash_{hash}";
        }
    }
}

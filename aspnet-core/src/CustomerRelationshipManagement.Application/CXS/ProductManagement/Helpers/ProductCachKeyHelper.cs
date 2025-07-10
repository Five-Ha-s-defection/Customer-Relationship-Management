using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CXS.ProductManagement.Helpers
{
    public static class ProductCachKeyHelper
    {
        /// <summary>
        /// 构建可读的缓存Key（推荐：方便调试）
        /// </summary>
        public static string ProductBuildHashKey(SearchProductDto dto)
        {
            string safe(string? input) => string.IsNullOrWhiteSpace(input) ? "null" : input.Trim();
            return $"ProductRedis_" +
                   $"CategoryId_{(dto.CategoryId.HasValue ? dto.CategoryId.Value.ToString() : "null")}_" +
                   $"CategoryName_{safe(dto.CategoryName)}_" +
                   $"ParentId_{(dto.ParentId.HasValue ? dto.ParentId.Value.ToString() : "null")}_" +
                   $"ProductImageUrl_{safe(dto.ProductImageUrl)}_" +
                   $"ProductBrand_{safe(dto.ProductBrand)}_" +
                   $"ProductSupplier_{safe(dto.ProductSupplier)}_" +
                   $"ProductCode_{safe(dto.ProductCode)}_" +
                   $"ProductDescription_{safe(dto.ProductDescription)}_" +
                   $"SuggestedPrice_{dto.SuggestedPrice?.ToString() ?? "null"}_" +
                   $"ProductRemark_{safe(dto.ProductRemark)}_" +
                   $"ProductStatus_{dto.ProductStatus}_" +
                   $"DealPrice_{dto.DealPrice?.ToString() ?? "null"}_" +
                     $"Page_{dto.PageIndex}_Size_{dto.PageSize}";


        }
        ///// <summary>
        ///// 构建可读缓存key
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>

        //public static string BuildReadableKey(SearchProductDto dto)
        //{
        //    string rawKey = $"ProductRedis_CategoryId_{(dto.CategoryId.HasValue ? dto.CategoryId.Value.ToString() : "null")}_CategoryName_{dto.CategoryName}_ParentId_{(dto.ParentId.HasValue ? dto.ParentId.Value.ToString() : "null")}_ProductImageUrl_{dto.ProductImageUrl}_ProductBrand_{dto.ProductBrand}_ProductSupplier_{dto.ProductSupplier}_ProductCode_{dto.ProductCode}_ProductDescription_{dto.ProductDescription}_SuggestedPrice_{dto.SuggestedPrice?.ToString() ?? "null"}_ProductRemark_{dto.ProductRemark}_ProductStatus_{dto.ProductStatus}_DealPrice_{dto.DealPrice?.ToString() ?? "null"}_Page_{dto.PageIndex}_Size_{dto.PageSize}";
        //    using var md5 = MD5.Create();
        //    var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
        //    string hash = BitConverter.ToString(hashBytes).Replace("-", "");
        //    return $"CustomerRedis_{hash}";
        //}


    }
}

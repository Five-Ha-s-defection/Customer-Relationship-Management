using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.SearchMenus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.SearchMenus.Interfaces
{
    /// <summary>
    /// 菜单搜索服务接口
    /// 提供基于ElasticSearch的菜单全文搜索功能
    /// </summary>
    public interface IMenuSearchService : IApplicationService
    {
        /// <summary>
        /// 搜索菜单
        /// 根据关键字在菜单名称、路径、权限码等字段中进行全文搜索
        /// </summary>
        /// <param name="searchDto">搜索条件</param>
        /// <returns>分页的搜索结果</returns>
        Task<ApiResult<PageInfoCount<MenuSearchResultDto>>> SearchMenusAsync(MenuSearchDto searchDto);

        /// <summary>
        /// 重建菜单索引
        /// 从数据库重新同步所有菜单数据到ElasticSearch
        /// </summary>
        /// <returns>操作结果</returns>
        Task<ApiResult> RebuildMenuIndexAsync();

        /// <summary>
        /// 索引单个菜单
        /// 将单个菜单数据同步到ElasticSearch
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>操作结果</returns>
        Task<ApiResult> IndexMenuAsync(Guid menuId);

        /// <summary>
        /// 删除菜单索引
        /// 从ElasticSearch中删除指定菜单的索引
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>操作结果</returns>
        Task<ApiResult> DeleteMenuIndexAsync(Guid menuId);

        /// <summary>
        /// 获取搜索建议
        /// 根据输入的关键字提供搜索建议
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="limit">建议数量限制</param>
        /// <returns>搜索建议列表</returns>
        Task<ApiResult<List<string>>> GetSearchSuggestionsAsync(string keyword, int limit = 10);

        /// <summary>
        /// 测试ElasticSearch连接
        /// </summary>
        /// <returns>连接状态</returns>
        Task<ApiResult> TestElasticSearchConnectionAsync();
    }
}

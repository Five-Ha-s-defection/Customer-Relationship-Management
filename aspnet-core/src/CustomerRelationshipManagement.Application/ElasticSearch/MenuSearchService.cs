using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Application.Contracts.RBAC.Menus;
using CustomerRelationshipManagement.ElasticSearch.Models;
using CustomerRelationshipManagement.Paging;
using CustomerRelationshipManagement.RBAC.Menus;
using CustomerRelationshipManagement.RBACDtos.Menus;
using CustomerRelationshipManagement.SearchMenus.Dto;
using CustomerRelationshipManagement.SearchMenus.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.ElasticSearch
{
    /// <summary>
    /// 菜单搜索服务实现
    /// 提供基于ElasticSearch的菜单全文搜索功能
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [AllowAnonymous]
    public class MenuSearchService : ApplicationService, IMenuSearchService
    {
        private readonly IRepository<MenuInfo, Guid> _menuRepository;
        private readonly ElasticSearchClientProvider _elasticSearchClientProvider;
        private readonly IOptions<ElasticSearchOptions> searchOptions;
        private const string MENU_INDEX_NAME = "crmsystem";

        public MenuSearchService(
            IRepository<MenuInfo, Guid> menuRepository,
            ElasticSearchClientProvider elasticSearchClientProvider, IOptions<ElasticSearchOptions> searchOptions)
        {
            _menuRepository = menuRepository;
            _elasticSearchClientProvider = elasticSearchClientProvider;
            this.searchOptions = searchOptions;
        }

        /// <summary>
        /// 搜索菜单
        /// </summary>        
        public async Task<ApiResult<PageInfoCount<MenuSearchResultDto>>> SearchMenusAsync(MenuSearchDto searchDto)
        {
            try
            {
                //获取 Elasticsearch 客户端实例，用于后续的搜索操作
                var client = _elasticSearchClientProvider.GetClient();

                // 构建搜索查询,构建一个带分页、排序和高亮的 Elasticsearch 搜索请求，用于查询菜单相关的数据，并且让搜索关键字在 menuName 和 searchText 字段中以 <em>关键字</em> 的形式高亮显示。
                var searchRequest = new SearchRequest<MenuIndexModel>(MENU_INDEX_NAME)
                {
                    From = (searchDto.PageIndex - 1) * searchDto.PageSize,
                    Size = searchDto.PageSize,
                    Query = BuildSearchQuery(searchDto),
                    //排序：先按相关性 _score 降序，再按 sort 字段升序。
                    Sort = new List<ISort>
                    {
                        new FieldSort { Field = "_score", Order = SortOrder.Descending },
                        new FieldSort { Field = "sort", Order = SortOrder.Ascending }
                    },
                    //对 menuName 和 searchText 字段命中内容用 <em>...</em> 标签包裹，便于前端高亮显示。
                    Highlight = new Highlight
                    {
                        PreTags = new[] { "<em>" },
                        PostTags = new[] { "</em>" },
                        Fields = new Dictionary<Field, IHighlightField>
                        {
                            { "menuName", new HighlightField() },
                            { "searchText", new HighlightField() }
                        }
                    }
                };

                //异步执行一次 Elasticsearch 搜索请求。
                var searchResponse = await client.SearchAsync<MenuIndexModel>(searchRequest);

                //判断本次搜索请求是否有效
                if (!searchResponse.IsValid)
                {
                    return ApiResult<PageInfoCount<MenuSearchResultDto>>.Fail("搜索失败", ResultCode.Fail);
                }

                // 转换搜索结果
                var results = new List<MenuSearchResultDto>();
                //遍历 Elasticsearch 搜索响应中的每一条命中（Hit）,每个 hit 代表一条匹配的菜单数据,遍历每条命中数据，将其转换为 MenuSearchResultDto。
                foreach (var hit in searchResponse.Hits)
                {
                    //获取当前命中的原始数据对象（即 MenuIndexModel 实例）
                    var menu = hit.Source;
                    var result = new MenuSearchResultDto
                    {
                        Id = menu.Id,
                        ParentId = menu.ParentId,
                        MenuName = menu.MenuName,
                        Path = menu.Path,
                        Component = menu.Component,
                        Icon = menu.Icon,
                        PermissionCode = menu.PermissionCode,
                        IsVisible = menu.IsVisible,
                        Sort = menu.Sort,
                        ParentMenuName = menu.ParentMenuName,
                        MenuPath = menu.MenuPath,
                        Score = hit.Score ?? 0
                    };

                    // 判断当前命中结果中是否包含 menuName 字段的高亮内容。
                    if (hit.Highlight.ContainsKey("menuName"))
                    {
                        //如果有高亮内容，则取第一个高亮字符串赋值给 result.HighlightedMenuName
                        result.HighlightedMenuName = hit.Highlight["menuName"].FirstOrDefault() ?? menu.MenuName;
                    }
                    else
                    {
                        //如果没有高亮内容，则直接用原始的 menu.MenuName
                        result.HighlightedMenuName = menu.MenuName;
                    }

                    results.Add(result);
                }

                var pageInfo = new PageInfoCount<MenuSearchResultDto>
                {
                    Data = results,
                    TotalCount = (int)searchResponse.Total,
                    PageIndex = searchDto.PageIndex,
                    PageSize = searchDto.PageSize
                };

                return ApiResult<PageInfoCount<MenuSearchResultDto>>.Success(ResultCode.Success, pageInfo);
            }
            catch (Exception ex)
            {
                return ApiResult<PageInfoCount<MenuSearchResultDto>>.Fail($"搜索失败: {ex.Message}", ResultCode.Fail);
            }
        }

        /// <summary>
        /// 构建搜索查询
        /// </summary>
        private QueryContainer BuildSearchQuery(MenuSearchDto searchDto)
        {
            var queries = new List<QueryContainer>();

            // 关键字搜索
            if (!string.IsNullOrWhiteSpace(searchDto.Keyword))
            {
                var keywordQueries = new List<QueryContainer>
                {
                    new MatchQuery { Field = "menuName", Query = searchDto.Keyword, Boost = 3.0f },
                    new MatchQuery { Field = "searchText", Query = searchDto.Keyword, Boost = 1.0f },
                    new WildcardQuery { Field = "menuName", Value = $"*{searchDto.Keyword}*", Boost = 2.0f },
                    new WildcardQuery { Field = "path", Value = $"*{searchDto.Keyword}*", Boost = 1.5f },
                    new WildcardQuery { Field = "permissionCode", Value = $"*{searchDto.Keyword}*", Boost = 1.0f }
                };

                queries.Add(new BoolQuery { Should = keywordQueries });
            }

            // 可见性过滤
            if (searchDto.OnlyVisible)
            {
                queries.Add(new TermQuery { Field = "isVisible", Value = true });
            }

            // 父级菜单过滤
            if (searchDto.ParentId.HasValue)
            {
                queries.Add(new TermQuery { Field = "parentId", Value = searchDto.ParentId.Value });
            }

            return new BoolQuery { Must = queries };
        }

        /// <summary>
        /// 重建菜单索引,删除旧索引，创建新索引
        /// </summary>
        public async Task<ApiResult> RebuildMenuIndexAsync()
        {
            try
            {
                var client = _elasticSearchClientProvider.GetClient();

                // 删除现有索引
                if (await _elasticSearchClientProvider.IndexExistsAsync(MENU_INDEX_NAME))
                {
                    await _elasticSearchClientProvider.DeleteIndexAsync(MENU_INDEX_NAME);
                }

                // 创建新索引,创建一个菜单索引，并为每个字段指定合适的类型和分词方式。
                var createIndexResponse = await client.Indices.CreateAsync(MENU_INDEX_NAME, c => c
                    .Settings(s => s
                        .NumberOfShards(1)
                        .NumberOfReplicas(0)
                        .Analysis(a => a
                            .Analyzers(an => an
                                .Custom("ik_custom", ca => ca
                                    .Tokenizer("ik_max_word")
                                )
                            )
                        )
                    )
                    .Map<MenuIndexModel>(m => m
                        .Properties(p => p
                            .Text(t => t.Name(n => n.MenuName).Analyzer("ik_max_word"))
                            .Text(t => t.Name(n => n.Path).Analyzer("standard"))
                            .Text(t => t.Name(n => n.Component).Analyzer("standard"))
                            .Text(t => t.Name(n => n.PermissionCode).Analyzer("standard"))
                            .Text(t => t.Name(n => n.SearchText).Analyzer("ik_max_word"))
                            .Keyword(k => k.Name(n => n.Id))
                            .Keyword(k => k.Name(n => n.ParentId))
                            .Keyword(k => k.Name(n => n.Icon))
                            .Boolean(b => b.Name(n => n.IsVisible))
                            .Number(i => i.Name(n => n.Sort))
                            .Text(t => t.Name(n => n.ParentMenuName).Analyzer("ik_max_word"))
                            .Text(t => t.Name(n => n.MenuPath).Analyzer("ik_max_word"))
                            .Number(i => i.Name(n => n.Level))
                            .Date(d => d.Name(n => n.CreationTime))
                            .Date(d => d.Name(n => n.LastModificationTime))
                        )
                    )
                );

                if (!createIndexResponse.IsValid)
                {
                    return ApiResult.Fail("创建索引失败", ResultCode.Fail);
                }

                // 获取所有菜单数据
                var allMenus = await _menuRepository.GetListAsync();
                //把菜单列表转成以菜单ID为key的字典，方便后续通过ID快速查找菜单。
                var menuDict = allMenus.ToDictionary(m => m.Id);

                // 构建菜单索引数据
                var menuIndexModels = new List<MenuIndexModel>();
                //遍历所有菜单数据，将其逐个转换为适合 Elasticsearch 索引的数据模型，并收集到一个新列表中。
                foreach (var menu in allMenus)
                {
                    //ConvertToIndexModel 是一个自定义的方法，它的作用通常是把数据库中的菜单对象（如 Menu）转换成适合 Elasticsearch 索引的数据模型（如 MenuIndexModel）。
                    var indexModel = ConvertToIndexModel(menu, menuDict);
                    menuIndexModels.Add(indexModel);
                }

                // 批量索引数据,判断 menuIndexModels 列表是否有数据
                if (menuIndexModels.Any())
                {
                    //BulkRequest是Elasticsearch 的批量操作请求对象，可以一次性插入/更新/删除多条数据
                    var bulkRequest = new BulkRequest(MENU_INDEX_NAME)
                    {
                        //把每个 MenuIndexModel 对象包装成一个批量索引操作。转换为通用的批量操作接口类型
                        Operations = menuIndexModels.Select(m => new BulkIndexOperation<MenuIndexModel>(m)).Cast<IBulkOperation>().ToList()
                    };

                    //调用 Elasticsearch 客户端的 BulkAsync 方法，异步执行批量写入操作。
                    var bulkResponse = await client.BulkAsync(bulkRequest);
                    if (bulkResponse.Errors)
                    {
                        // 真的有错误
                        var errors = bulkResponse.ItemsWithErrors.Select(e => e.Error.Reason);
                        return ApiResult.Fail("部分数据写入失败: " + string.Join(";", errors), ResultCode.Fail);
                    }
                    else
                    {
                        return ApiResult.Success(ResultCode.Success);
                    }
                }

                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "重建索引异常");
                return ApiResult.Fail($"重建索引失败: {ex.Message}", ResultCode.Fail);
            }
        }

        /// <summary>
        /// 索引单个菜单
        /// </summary>
        public async Task<ApiResult> IndexMenuAsync(Guid menuId)
        {
            try
            {
                //根据 menuId 获取特定的菜单数据
                var menu = await _menuRepository.GetAsync(menuId);
                //获取所有菜单列表
                var allMenus = await _menuRepository.GetListAsync();
                //将所有菜单转换为以ID为键的字典，便于快速查找
                var menuDict = allMenus.ToDictionary(m => m.Id);

                //调用 ConvertToIndexModel 方法将菜单数据转换为适合 Elasticsearch 索引的模型
                var indexModel = ConvertToIndexModel(menu, menuDict);
                var client = _elasticSearchClientProvider.GetClient();

                //将转换后的索引模型异步写入
                var response = await client.IndexAsync(indexModel, i => i.Index(MENU_INDEX_NAME));
                return response.IsValid ? ApiResult.Success(ResultCode.Success) : ApiResult.Fail("索引失败", ResultCode.Fail);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"索引菜单失败: {ex.Message}", ResultCode.Fail);
            }
        }

        /// <summary>
        /// 删除菜单索引
        /// </summary>
        public async Task<ApiResult> DeleteMenuIndexAsync(Guid menuId)
        {
            try
            {
                var client = _elasticSearchClientProvider.GetClient();
                var response = await client.DeleteAsync<MenuIndexModel>(menuId, d => d.Index(MENU_INDEX_NAME));
                return response.IsValid ? ApiResult.Success(ResultCode.Success) : ApiResult.Fail("删除索引失败", ResultCode.Fail);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"删除菜单索引失败: {ex.Message}", ResultCode.Fail);
            }
        }

        /// <summary>
        /// 获取搜索建议
        /// </summary>
        public async Task<ApiResult<List<string>>> GetSearchSuggestionsAsync(string keyword, int limit = 10)
        {
            try
            {
                //如果为空，直接返回空列表
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return ApiResult<List<string>>.Success(ResultCode.Success, new List<string>());
                }

                var client = _elasticSearchClientProvider.GetClient();
                var searchRequest = new SearchRequest<MenuIndexModel>(MENU_INDEX_NAME)
                {
                    //Size: 限制返回结果数量
                    Size = limit,
                    //BoolQuery with Should: 使用"或"逻辑，满足任一条件即可
                    Query = new BoolQuery
                    {
                        Should = new List<QueryContainer>
                        {
                            //menuName: 菜单名称前缀匹配（支持中文）
                            new PrefixQuery { Field = "menuName", Value = keyword }
                        }
                    },
                    Sort = new List<ISort>
                    {
                        new FieldSort { Field = "sort", Order = SortOrder.Ascending }
                    }
                };

                //异步执行 Elasticsearch 搜索
                var response = await client.SearchAsync<MenuIndexModel>(searchRequest);
                if (!response.IsValid)
                {
                    return ApiResult<List<string>>.Fail("获取建议失败", ResultCode.Fail);
                }

                //从搜索结果中提取菜单名称 去重处理（Distinct()）限制返回数量（Take(limit)）
                var suggestions = response.Documents.Select(d => d.MenuName).Distinct().Take(limit).ToList();
                return ApiResult<List<string>>.Success(ResultCode.Success, suggestions);
            }
            catch (Exception ex)
            {
                return ApiResult<List<string>>.Fail($"获取搜索建议失败: {ex.Message}", ResultCode.Fail);
            }
        }

        /// <summary>
        /// 测试ElasticSearch连接
        /// </summary>
        public async Task<ApiResult> TestElasticSearchConnectionAsync()
        {
            try
            {
                //调用 _elasticSearchClientProvider 的 TestConnectionAsync 方法 异步测试与 Elasticsearch 集群的连接 返回布尔值表示连接状态
                var isConnected = await _elasticSearchClientProvider.TestConnectionAsync();
                return isConnected ? ApiResult.Success(ResultCode.Success) : ApiResult.Fail("连接失败", ResultCode.Fail);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"连接测试失败: {ex.Message}", ResultCode.Fail);
            }
        }

        /// <summary>
        /// 转换为索引模型
        /// </summary>
        private MenuIndexModel ConvertToIndexModel(MenuInfo menu, Dictionary<Guid, MenuInfo> menuDict)
        {
            var indexModel = new MenuIndexModel
            {
                Id = menu.Id,
                ParentId = menu.ParentId,
                MenuName = menu.MenuName,
                Path = menu.Path,
                Component = menu.Component,
                Icon = menu.Icon,
                PermissionCode = menu.PermissionCode,
                IsVisible = menu.IsVisible,
                Sort = menu.Sort,
                CreationTime = menu.CreationTime,
                LastModificationTime = menu.LastModificationTime ?? DateTime.MinValue
            };

            // 设置父级菜单名称
            if (menu.ParentId.HasValue && menuDict.TryGetValue(menu.ParentId.Value, out var parentMenu))
            {
                indexModel.ParentMenuName = parentMenu.MenuName;
            }

            // 构建菜单路径
            indexModel.MenuPath = BuildMenuPath(menu, menuDict);

            // 计算菜单层级
            indexModel.Level = CalculateMenuLevel(menu, menuDict);

            // 构建搜索文本
            indexModel.SearchText = $"{menu.MenuName} {menu.Path} {menu.Component} {menu.PermissionCode} {indexModel.MenuPath}";

            return indexModel;
        }

        /// <summary>
        /// 构建菜单路径
        /// </summary>
        private string BuildMenuPath(MenuInfo menu, Dictionary<Guid, MenuInfo> menuDict)
        {
            var path = new List<string> { menu.MenuName };
            var currentMenu = menu;

            while (currentMenu.ParentId.HasValue && menuDict.TryGetValue(currentMenu.ParentId.Value, out var parentMenu))
            {
                path.Insert(0, parentMenu.MenuName);
                currentMenu = parentMenu;
            }

            return string.Join("/", path);
        }

        /// <summary>
        /// 计算菜单层级
        /// </summary>
        private int CalculateMenuLevel(MenuInfo menu, Dictionary<Guid, MenuInfo> menuDict)
        {
            var level = 0;
            var currentMenu = menu;

            while (currentMenu.ParentId.HasValue && menuDict.TryGetValue(currentMenu.ParentId.Value, out var parentMenu))
            {
                level++;
                currentMenu = parentMenu;
            }

            return level;
        }
    }
}
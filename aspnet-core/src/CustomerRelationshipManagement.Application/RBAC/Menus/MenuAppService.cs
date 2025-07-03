using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Application.Contracts.RBAC.Menus;
using CustomerRelationshipManagement.RBAC.Menus;
using CustomerRelationshipManagement.RBACDtos.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.Application.RBAC.Menus
{
    /// <summary>
    /// 菜单应用服务，实现菜单的增删改查功能
    /// 使用ABP框架的标准应用服务模式
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [AllowAnonymous]
    public class MenuAppService :ApplicationService, IMenuAppService
    {
        // 菜单仓储，用于数据库操作
        private readonly IRepository<MenuInfo, Guid> _menuRepository;

        /// <summary>
        /// 构造函数，注入仓储
        /// </summary>
        public MenuAppService(IRepository<MenuInfo, Guid> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// 创建菜单
        /// 使用ABP框架的ObjectMapper进行DTO到实体的映射
        /// </summary>
        /// <param name="input">菜单创建数据传输对象</param>
        /// <returns>操作结果</returns>
        [HttpPost]
        public async Task<ApiResult> CreateMenuAsync(CreateOrUpdateMenuDto input)
        {
            try
            {
                // 使用ABP框架的ObjectMapper将DTO映射为实体
                var menu = ObjectMapper.Map<CreateOrUpdateMenuDto, MenuInfo>(input);
                
                // 插入数据库
                await _menuRepository.InsertAsync(menu);
                
                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                // 异常处理，返回失败信息
                return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 更新菜单
        /// 使用ABP框架的ObjectMapper进行DTO到实体的映射
        /// </summary>
        /// <param name="input">菜单更新数据传输对象</param>
        /// <returns>操作结果</returns>
        [HttpPut]
        public async Task<ApiResult> UpdateMenuAsync(Guid id, CreateOrUpdateMenuDto input)
        {
            try
            {
              //查出菜单
              var menu = await _menuRepository.GetAsync(id);
                //使用ABP框架的ObjectMapper将DTO映射为实体
                menu = ObjectMapper.Map(input, menu);
                await _menuRepository.UpdateAsync(menu);
                
                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                // 异常处理，返回失败信息
                return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteMenuAsync(Guid id)
        {
            try
            {
                // 查找菜单
                var menu = await _menuRepository.GetAsync(id);
                
                // 删除菜单
                await _menuRepository.DeleteAsync(menu);
                
                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                // 异常处理，返回失败信息
                return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 获取菜单树形结构
        /// 从数据库获取所有菜单数据，构建树形结构返回给前端
        /// </summary>
        /// <returns>树形结构的菜单列表，包含完整的父子关系</returns>
        [HttpGet("/api/v1/menus/routes")]
        public async Task<ApiResult<List<MenuDto>>> GetMenuTreeAsync()
        {
            try
            {
                // 从数据库获取所有菜单数据
                var allMenus = await _menuRepository.GetListAsync();
                
                // 使用ABP框架的ObjectMapper将实体数据转换为DTO格式
                var menuDtos = ObjectMapper.Map<List<MenuInfo>, List<MenuDto>>(allMenus);
                
                // 为每个DTO初始化子菜单列表
                foreach (var menuDto in menuDtos)
                {
                    menuDto.Children = new List<MenuDto>();
                }

                // 构建树形结构
                var menuTree = BuildMenuTree(menuDtos, allMenus);
                
                // 返回成功结果，包含树形菜单数据
                return ApiResult<List<MenuDto>>.Success(ResultCode.Success, menuTree);
            }
            catch (Exception ex)
            {
                // 异常处理，返回失败信息
                return ApiResult<List<MenuDto>>.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 构建菜单树形结构
        /// 根据ParentId字段构建父子关系，形成树形结构
        /// </summary>
        /// <param name="menuDtos">菜单DTO列表，用于返回给前端</param>
        /// <param name="allMenus">所有菜单实体列表，包含数据库中的完整信息</param>
        /// <returns>构建好的树形结构菜单列表</returns>
        private List<MenuDto> BuildMenuTree(List<MenuDto> menuDtos, List<MenuInfo> allMenus)
        {
            // 初始化返回的树形菜单列表
            var menuTree = new List<MenuDto>();
            
            // 创建ID到DTO的映射字典，用于快速查找父菜单
            var menuDict = new Dictionary<Guid, MenuDto>();

            // 构建ID到DTO的映射字典，提高查找效率
            for (int i = 0; i < allMenus.Count; i++)
            {
                menuDict[allMenus[i].Id] = menuDtos[i];
            }

            // 遍历所有菜单，构建父子关系
            for (int i = 0; i < allMenus.Count; i++)
            {
                var menuEntity = allMenus[i];    // 当前菜单实体
                var menuDto = menuDtos[i];       // 对应的DTO对象

                if (menuEntity.ParentId == null)
                {
                    // 根节点：ParentId为null的菜单作为顶级菜单
                    menuTree.Add(menuDto);
                }
                else
                {
                    // 子节点：根据ParentId找到父菜单，将当前菜单添加到父菜单的Children中
                    if (menuDict.TryGetValue(menuEntity.ParentId.Value, out var parentMenuDto))
                    {
                        parentMenuDto.Children.Add(menuDto);
                    }
                }
            }

            // 对构建好的树形结构进行排序
            SortMenuTree(menuTree);
            
            return menuTree;
        }

        /// <summary>
        /// 递归排序菜单树
        /// 按照Sort字段对菜单树进行排序，包括所有层级的子菜单
        /// </summary>
        /// <param name="menus">需要排序的菜单列表</param>
        private void SortMenuTree(List<MenuDto> menus)
        {
            // 空值检查和边界条件
            if (menus == null || !menus.Any()) return;

            // 对当前层级的菜单按Sort字段进行排序（升序）
            menus.Sort((a, b) => a.Sort.CompareTo(b.Sort));

            // 递归排序每个菜单的子菜单
            foreach (var menu in menus)
            {
                if (menu.Children != null && menu.Children.Any())
                {
                    SortMenuTree(menu.Children);
                }
            }
        }
    }
} 
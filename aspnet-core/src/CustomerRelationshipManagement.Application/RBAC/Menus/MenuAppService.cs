using System.Threading.Tasks;
using CustomerRelationshipManagement.Application.Contracts.RBAC.Menus;
using CustomerRelationshipManagement.RBACDtos.Menus;
using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBAC.Menus;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using System;

namespace CustomerRelationshipManagement.Application.RBAC.Menus
{
    /// <summary>
    /// 菜单应用服务，实现菜单的增删改功能
    /// </summary>
    public class MenuAppService : IMenuAppService
    {
        // 菜单仓储，用于数据库操作
        private readonly IRepository<MenuInfo, Guid> _menuRepository;
        // AutoMapper对象，用于DTO和实体的映射
        private readonly IObjectMapper _objectMapper;

        /// <summary>
        /// 构造函数，注入仓储和映射器
        /// </summary>
        public MenuAppService(IRepository<MenuInfo, Guid> menuRepository, IObjectMapper objectMapper)
        {
            _menuRepository = menuRepository;
            _objectMapper = objectMapper;
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input">菜单新增DTO</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult> CreateMenuAsync(CreateOrUpdateMenuDto input)
        {
            try
            {
                // DTO映射为实体
                var menu = _objectMapper.Map<CreateOrUpdateMenuDto, MenuInfo>(input);
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
        /// 修改菜单
        /// </summary>
        /// <param name="input">菜单修改DTO，必须包含Id</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult> UpdateMenuAsync(CreateOrUpdateMenuDto input)
        {
            try
            {
                // 检查Id是否存在
                if (input.Id == null)
                    throw new Exception("缺少菜单Id");
                // 根据Id查找菜单
                var menu = await _menuRepository.GetAsync(input.Id.Value);
                // DTO映射到实体（更新字段）
                _objectMapper.Map(input, menu);
                // 更新数据库
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
        /// <param name="id">菜单主键Id</param>
        /// <returns>操作结果</returns>
        public async Task<ApiResult> DeleteMenuAsync(long id)
        {
            try
            {
                // long转Guid
                var guid = new Guid(id.ToString());
                // 查找菜单
                var menu = await _menuRepository.GetAsync(guid);
                if (menu == null)
                    return ApiResult.Fail("未找到要删除的菜单", ResultCode.NotFound);
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
    }
} 
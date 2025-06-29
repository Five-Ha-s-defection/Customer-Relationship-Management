using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.RBAC.Permissions
{
    [ApiExplorerSettings(GroupName = "v1")]
    [AllowAnonymous]
    public class PermissionServices : ApplicationService, IPermissionServices
    {
        private readonly IRepository<PermissionInfo, Guid> permissionInfoRepo;

        public PermissionServices(IRepository<PermissionInfo,Guid> permissionInfoRepo)
        {
            this.permissionInfoRepo = permissionInfoRepo;
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="createPermissionDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<PermissionDto>> AddPermission(CreatePermissionDto createPermissionDto)
        {
            try
            {
                  var permissionInfoDto = ObjectMapper.Map<CreatePermissionDto, PermissionInfo>(createPermissionDto);
                  var permissionInfo = await permissionInfoRepo.InsertAsync(permissionInfoDto);
                return ApiResult<PermissionDto>.Success(ResultCode.Success, ObjectMapper.Map<PermissionInfo, PermissionDto>(permissionInfo));
                 
            }
            catch (Exception ex)
            {
                return ApiResult<PermissionDto>.Fail(ex.Message, ResultCode.Fail);
                
            }
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns>权限信息列表</returns>
        public async Task<ApiResult<List<PermissionDto>>> GetPermissionList()
        {
            try
            {
                var permissions = await permissionInfoRepo.GetListAsync();
                return ApiResult<List<PermissionDto>>.Success(ResultCode.Success, ObjectMapper.Map<List<PermissionInfo>, List<PermissionDto>>(permissions.ToList()));

            }
            catch (Exception ex)
            {
                return ApiResult<List<PermissionDto>>.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id">要删除的权限ID</param>
        /// <returns>删除结果</returns>
        public async Task<ApiResult> DeletePermission(Guid id)
        {
            try
            {
                var permission = await permissionInfoRepo.GetAsync(id);
                if (permission == null)
                {
                    return ApiResult.Fail("未找到要删除的权限", ResultCode.NotFound);
                }
                await permissionInfoRepo.DeleteAsync(permission);
                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 修改权限信息
        /// </summary>
        /// <param name="id">要修改的权限ID</param>
        /// <param name="dto">要修改的权限信息</param>
        /// <returns>修改后的权限信息</returns>
        public async Task<ApiResult<PermissionInfo>> UpdatePermission(Guid id, CreatePermissionDto dto)
        {
            try
            {
                var permission = await permissionInfoRepo.GetAsync(id);
                if (permission == null)
                {
                    return ApiResult<PermissionInfo>.Fail("未找到要修改的权限", ResultCode.NotFound);
                }
                ObjectMapper.Map(dto, permission);
                var updated = await permissionInfoRepo.UpdateAsync(permission);
                return ApiResult<PermissionInfo>.Success(ResultCode.Success, updated);
            }
            catch (Exception ex)
            {
                return ApiResult<PermissionInfo>.Fail(ex.Message, ResultCode.Fail);
            }
        }
    }
}

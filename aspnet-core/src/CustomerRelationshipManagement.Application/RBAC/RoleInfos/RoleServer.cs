using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBACDtos.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.RBAC.RoleInfos
{
    [ApiExplorerSettings(GroupName = "v1")]
    [AllowAnonymous]
    public class RoleServer : ApplicationService, IRoleServer
    {
        private readonly IRepository<RoleInfo, Guid> rolerepository;

        public RoleServer(IRepository<RoleInfo, Guid> rolerepository)
        {
            this.rolerepository = rolerepository;
        }
        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="createOrUpdateRoleInfoDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<RoleDto>> AddRoleInfo(CreateOrUpdateRoleDto createOrUpdateRoleInfoDto)
        {
            try
            {
                 var roleInfoDto = ObjectMapper.Map<CreateOrUpdateRoleDto, RoleInfo>(createOrUpdateRoleInfoDto);
                var roleInfo = await rolerepository.InsertAsync(roleInfoDto);
                return ApiResult<RoleDto>.Success(ResultCode.Success, ObjectMapper.Map<RoleInfo, RoleDto>(roleInfo));
            }
            catch (Exception ex)
            { 
                 return ApiResult<RoleDto>.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="id">要删除的角色ID</param>
        /// <returns>删除结果</returns>
        public async Task<ApiResult> DeleteRoleInfo(Guid id)
        {
            try
            {
                var role = await rolerepository.GetAsync(id);
                if (role == null)
                {
                    return ApiResult.Fail("未找到要删除的角色", ResultCode.NotFound);
                }
                await rolerepository.DeleteAsync(role);
                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns>角色信息列表</returns>
        public async Task<ApiResult<List<RoleDto>>> GetRoleInfoList()
        {
            try
            {
                var roles = await rolerepository.GetListAsync();
                var dtos = ObjectMapper.Map<List<RoleInfo>, List<RoleDto>>(roles.ToList());
                return ApiResult<List<RoleDto>>.Success(ResultCode.Success, dtos);
            }
            catch (Exception ex)
            {
                return ApiResult<List<RoleDto>>.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="Id">要修改的角色ID</param>
        /// <param name="createOrUpdateRoleInfoDto">要修改的角色信息</param>
        /// <returns>修改后的角色信息</returns>
        public async Task<ApiResult<RoleDto>> UpdateRoleInfo(Guid Id, CreateOrUpdateRoleDto createOrUpdateRoleInfoDto)
        {
            try
            {
                var role = await rolerepository.GetAsync(Id);
                if (role == null)
                {
                    return ApiResult<RoleDto>.Fail("未找到要修改的角色", ResultCode.NotFound);
                }
                ObjectMapper.Map(createOrUpdateRoleInfoDto, role);
                var updated = await rolerepository.UpdateAsync(role);
                return ApiResult<RoleDto>.Success(ResultCode.Success, ObjectMapper.Map<RoleInfo, RoleDto>(updated));
            }
            catch (Exception ex)
            {
                return ApiResult<RoleDto>.Fail(ex.Message, ResultCode.Fail);
            }
        }
    }
}

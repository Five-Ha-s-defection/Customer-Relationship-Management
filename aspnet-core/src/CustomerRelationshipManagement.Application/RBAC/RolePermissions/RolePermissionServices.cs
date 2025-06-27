using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.RolePermissions;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.RBAC.RolePermissions
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class RolePermissionServices : ApplicationService, IRolePermissionServices
    {
        private readonly IRepository<RolePermissionInfo, Guid> rolePermissionRepository;

        public RolePermissionServices(IRepository<RolePermissionInfo, Guid> rolePermissionRepository)
        {
            this.rolePermissionRepository = rolePermissionRepository;
        }
        /// <summary>
        /// 角色权限分配
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ApiResult> AssignPermissionsAsync(AssignPermissionsDto input)
        {
            //启用事务
            using(var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                { 
                    var rolePermissionInfo = await rolePermissionRepository.GetListAsync(x => x.RoleId == input.RoleId);
                    if (rolePermissionInfo.Count > 0)
                    {
                        //先删除原有的权限
                        await rolePermissionRepository.DeleteAsync(x => x.RoleId == input.RoleId);
                        //throw new Exception("角色已存在权限");
                    }
                   
                    //添加新的权限
                    var list = input.PermissionIds.Select(id => new RolePermissionInfo
                    {
                        RoleId = input.RoleId,
                        PermissionId = id
                    }).ToList();

                    await rolePermissionRepository.InsertManyAsync(list);
                    tran.Complete();
                    return ApiResult.Success(ResultCode.Success);

                }catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 获取角色已分配的权限ID列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>权限ID列表</returns>
        public async Task<ApiResult<List<Guid>>> GetAssignedPermissionIds(Guid roleId)
        {
            try
            {
                var permissions = await rolePermissionRepository.GetListAsync(x => x.RoleId == roleId);
                var ids = permissions.Select(x => x.PermissionId).ToList();
                return ApiResult<List<Guid>>.Success(ResultCode.Success, ids);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Guid>>.Fail(ex.Message, ResultCode.Fail);
            }
        }
    }
}

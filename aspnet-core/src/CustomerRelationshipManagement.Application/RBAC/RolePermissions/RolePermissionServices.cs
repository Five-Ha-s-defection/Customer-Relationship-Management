using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.RolePermissions;
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
    public class RolePermissionServices : ApplicationService, IRolePermissionServices
    {
        private readonly IRepository<RolePermissionInfo, Guid> rolePermissionRepository;

        public RolePermissionServices(IRepository<RolePermissionInfo, Guid> rolePermissionRepository)
        {
            this.rolePermissionRepository = rolePermissionRepository;
        }
        public async Task<ApiResult> AssignPermissionsAsync(AssignPermissionsDto input)
        {
            //启用事务
            using(var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                { 
                    //先删除原有的权限
                    await rolePermissionRepository.DeleteAsync(x => x.RoleId == input.RoleId);
                    //添加新的权限
                    foreach (var permissionId in input.PermissionIds)
                    {
                        var rolePermission = new RolePermissionInfo()
                        {
                            RoleId = input.RoleId,
                            PermissionId = permissionId
                        };
                        await rolePermissionRepository.InsertAsync(rolePermission);
                    }
                    return ApiResult.Success(ResultCode.Success);

                }catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}

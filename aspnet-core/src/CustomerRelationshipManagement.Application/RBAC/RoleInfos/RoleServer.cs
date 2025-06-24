using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.RoleInfos
{
    public class RoleServer : ApplicationService, IRoleServer
    {
        public RoleServer()
        {
            
        }
        public Task<ApiResult<RoleDto>> AddRoleInfo(CreateOrUpdateRoleDto createOrUpdateRoleInfoDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult> DeleteRoleInfo(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<RoleDto>>> GetRoleInfoList()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RoleDto>> UpdateRoleInfo(Guid Id, CreateOrUpdateRoleDto createOrUpdateRoleInfoDto)
        {
            throw new NotImplementedException();
        }
    }
}

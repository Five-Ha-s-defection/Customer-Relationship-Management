using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Dtos.Users;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    public interface ILogServices:IApplicationService
    {
        Task<ApiResult<UserInfoDto>> Login(LoginDto loginDto);
    }
}

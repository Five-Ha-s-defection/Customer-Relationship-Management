using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.Users;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    public interface ILogServices:IApplicationService
    {
        Task<ApiResult<LoginResultDto>> Login(LoginDto loginDto);
        /// <summary>
        /// 退出登录
        /// </summary>
        Task<ApiResult> Logout();
    }
}

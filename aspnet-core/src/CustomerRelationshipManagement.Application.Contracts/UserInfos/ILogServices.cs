using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.UserInfos
{
    public interface ILogServices:IApplicationService
    {
        Task<ApiResult<UserInfoDto>> Login(LoginDto loginDto);
    }
}

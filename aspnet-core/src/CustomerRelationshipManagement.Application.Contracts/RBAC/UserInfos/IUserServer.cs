using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Dtos.Users;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    public interface IUserServer:IApplicationService
    {
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="createOrUpdateUserInfoDto">用户DTO</param>
        /// <returns></returns>
        Task<ApiResult<UserInfo>> AddUserInfo(CreateOrUpdateUserInfoDto createOrUpdateUserInfoDto);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<UserInfoDto>> GetCurrentUserInfoAsync();

    }
}

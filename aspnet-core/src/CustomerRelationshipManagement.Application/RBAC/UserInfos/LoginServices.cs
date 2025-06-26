using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    [ApiExplorerSettings(GroupName ="v1")]
    public class LoginServices : ApplicationService, ILogServices
    {
        /// <summary>
        /// 用户信息仓储
        /// </summary>
        private readonly IRepository<UserInfo, Guid> userInfoRepository;
        private readonly IPasswordHasher<UserInfo> passwordHasher;
        private readonly UserProfileManager userProfileManager;
        private readonly IJwtHelper _jwtHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userInfoRepository"></param>
        public LoginServices(IRepository<UserInfo, Guid> userInfoRepository, IPasswordHasher<UserInfo> passwordHasher,UserProfileManager userProfileManager,IJwtHelper jwtHelper)
        {
            this.userInfoRepository = userInfoRepository;
            this.passwordHasher = passwordHasher;
            this.userProfileManager = userProfileManager;
            _jwtHelper = jwtHelper;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto">用户名和密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ApiResult<LoginResultDto>> Login(LoginDto loginDto)
        {
            try
            {
                var userInfo = await userInfoRepository.FindAsync(x => x.UserName == loginDto.UserName);
                if (userInfo == null)
                {
                    throw new UserFriendlyException("用户不存在");
                }
                //密码加密
                var password = passwordHasher.VerifyHashedPassword(userInfo, userInfo.Password, loginDto.Password);
                //验证密码
                if (password != PasswordVerificationResult.Success)
                {
                    throw new UserFriendlyException("密码错误");
                }
                // 用户被禁用，抛出禁用错误
                if (!userInfo.IsActive)
                {
                    throw new UserFriendlyException("用户已被禁用");
                }
                // 登录成功，返回用户信息和令牌
                
                var token = _jwtHelper.GenerateToken(userInfo.Id, userInfo.UserName);
                var expireTime = DateTime.UtcNow.AddHours(2);
                var profile = await userProfileManager.BuildUserProfileAsync(userInfo.Id);              
               
                //登录成功，返回用户信息和令牌
                var userInfoDto = ObjectMapper.Map<UserInfo, LoginResultDto>(userInfo);
                //返回数据
                return ApiResult<LoginResultDto>.Success(ResultCode.Success, new LoginResultDto
                {
                       Token=token,
                      User=profile,
                      ExpireTime=expireTime,
                });
                
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}

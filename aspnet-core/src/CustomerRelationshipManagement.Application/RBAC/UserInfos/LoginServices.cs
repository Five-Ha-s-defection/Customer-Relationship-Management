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
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.UserRoles;
using System.Linq;
using System.Collections.Generic;

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
        private readonly IDatabase _redisDb;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<UserRoleInfo> userRoleRepoistory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userInfoRepository"></param>
        public LoginServices(IRepository<UserInfo, Guid> userInfoRepository, IPasswordHasher<UserInfo> passwordHasher,UserProfileManager userProfileManager,IJwtHelper jwtHelper, IConnectionMultiplexer redis, IHttpContextAccessor httpContextAccessor,IRepository<UserRoleInfo> userRoleRepoistory)
        {
            this.userInfoRepository = userInfoRepository;
            this.passwordHasher = passwordHasher;
            this.userProfileManager = userProfileManager;
            _jwtHelper = jwtHelper;
            _redisDb = redis.GetDatabase();
            _httpContextAccessor = httpContextAccessor;
            this.userRoleRepoistory = userRoleRepoistory;
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

                //判断管理员角色
                bool isAdmin = profile.Roles.Any(r => r == "ROOT" || r == "超级管理员");

                if (profile.Roles.Contains("超级管理员") && !profile.Permissions.Contains("*:*:*"))
                {
                    profile.Permissions.Add("*:*:*");
                }

                //成功之后给我的token存入redis
                await _redisDb.StringSetAsync($"jwt_token:{token}", userInfo.Id.ToString());

               
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

        /// <summary>
        /// 退出登录
        /// 前端调用此接口后，后端返回成功，前端应清除本地token并跳转到登录页。
        /// </summary>
        /// <remarks>
        /// 路由：POST /logout
        /// 1. 前端调用此接口（无需参数，需携带token）
        /// 2. 后端可做token失效处理（如加入黑名单），此处仅返回成功
        /// 3. 前端收到成功后，清除本地token，跳转到登录页
        /// </remarks>
        [HttpDelete]
        [Route("/api/app/logout")]
        [Authorize]
        public async Task<ApiResult> Logout()
        {
            // 1. 获取当前请求的token
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // 2. 将token存入Redis黑名单，设置过期时间为token剩余有效期
            // 这里假设token有效期2小时
            await _redisDb.StringSetAsync($"jwt_blacklist:{token}", "1", TimeSpan.FromHours(2));

            return ApiResult.Success(ResultCode.Success);
        }
    }
}

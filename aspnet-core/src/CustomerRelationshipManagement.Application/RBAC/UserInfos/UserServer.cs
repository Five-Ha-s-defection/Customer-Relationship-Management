using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.Dtos.Users;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    public class UserServer : ApplicationService, IUserServer
    {
        /// <summary>
        /// 用户服务
        /// </summary>
        private readonly IRepository<UserInfo, Guid> userRep;
        /// <summary>
        /// 密码服务s
        /// </summary>
        private readonly IPasswordHasher<UserInfo> passwordHasher;
        private readonly ICurrentUser currentUser;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRep"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="currentUser"></param>
        public UserServer(IRepository<UserInfo,Guid> userRep,IPasswordHasher<UserInfo> passwordHasher,ICurrentUser currentUser)
        {
            this.userRep = userRep;
            this.passwordHasher = passwordHasher;
            this.currentUser = currentUser;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="createOrUpdateUserInfoDto"></param>
        /// <returns></returns>
        public async Task<ApiResult<UserInfo>> AddUserInfo(CreateOrUpdateUserInfoDto createOrUpdateUserInfoDto)
        {
            try
            {
                // 判断密码是否一致
                if (createOrUpdateUserInfoDto.Password != createOrUpdateUserInfoDto.ConfirmPassword)
                {
                    return ApiResult<UserInfo>.Fail("密码不一致", ResultCode.Fail);
                }
                // 判断用户是否存在
                var userInfo = await userRep.FindAsync(x => x.UserName == createOrUpdateUserInfoDto.UserName);
                if (userInfo != null)
                {
                    return ApiResult<UserInfo>.Fail("用户已存在", ResultCode.Fail);
                }
                //映射dto到实体
                var user = ObjectMapper.Map<CreateOrUpdateUserInfoDto, UserInfo>(createOrUpdateUserInfoDto);
                //密码加密
                user.Password = passwordHasher.HashPassword(user, user.Password);
                //给确认密码赋值
                user.ConfirmPassword = user.Password;
                var userinfos = await userRep.InsertAsync(user);
                return ApiResult<UserInfo>.Success(ResultCode.Success, userinfos);
            }
            catch (Exception ex)
            {
              return ApiResult<UserInfo>.Fail(ex.Message, ResultCode.InternalServerError);
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            try
            {
                // 判断用户是否登录
                if (!currentUser.IsAuthenticated)
                {
                    throw new UserFriendlyException("请先登录");
                }  
                // 获取用户id
                var userId = currentUser.GetId();
                // 获取用户
                var userInfo = await userRep.FindAsync(userId);
                // 判断密码是否一致
                var result = passwordHasher.VerifyHashedPassword(userInfo, userInfo.Password, changePasswordDto.CurrentPassword);
                if (result == PasswordVerificationResult.Failed)
                {
                    throw new UserFriendlyException("原密码不正确");
                }
                userInfo.Password = passwordHasher.HashPassword(userInfo, changePasswordDto.NewPassword);
                await userRep.UpdateAsync(userInfo);

            }
            catch (Exception ex)
            {
               throw new UserFriendlyException(ex.Message);
            }
        }
    }
}

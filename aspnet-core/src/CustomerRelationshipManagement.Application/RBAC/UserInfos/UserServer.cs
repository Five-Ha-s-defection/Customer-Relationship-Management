using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
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
        private readonly UserProfileManager userProfileManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRep"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="currentUser"></param>
        public UserServer(IRepository<UserInfo, Guid> userRep, IPasswordHasher<UserInfo> passwordHasher, ICurrentUser currentUser,UserProfileManager userProfileManager)
        {
            this.userRep = userRep;
            this.passwordHasher = passwordHasher;
            this.currentUser = currentUser;
            this.userProfileManager = userProfileManager;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="createOrUpdateUserInfoDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
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
        /// <summary>
        /// 获取当前用户信息（带角色、权限、菜单）
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/app/me")]
        public async Task<ApiResult<UserInfoDto>> GetCurrentUserInfoAsync()
        {
            try
            {
                //查出当前的用户id
                var userId = currentUser.GetId();
                // 查看当前用户是否登录
                if (userId == null)
                {
                    throw new BusinessException("请先登录");
                }
               var profile = await userProfileManager.BuildUserProfileAsync(userId);

                return ApiResult<UserInfoDto>.Success(ResultCode.Success,profile);

            }
            catch (Exception ex)
            {

                return await Task.FromResult(ApiResult<UserInfoDto>.Fail(ex.Message, ResultCode.Fail));
            }
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns>用户信息列表</returns>
        public async Task<ApiResult<List<UserInfoDto>>> GetUserInfoList()
        {
            try
            {
                var users = await userRep.GetListAsync();

                var dtos = ObjectMapper.Map<List<UserInfo>, List<UserInfoDto>>(users.ToList());

                return ApiResult<List<UserInfoDto>>.Success(ResultCode.Success, dtos);
            }
            catch (Exception ex)
            {
                return ApiResult<List<UserInfoDto>>.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">要删除的用户ID</param>
        /// <returns>删除结果</returns>
        public async Task<ApiResult> DeleteUserInfo(Guid id)
        {
            try
            {
                var user = await userRep.GetAsync(id);
                if (user == null)
                {
                    return ApiResult.Fail("未找到要删除的用户", ResultCode.NotFound);
                }
                await userRep.DeleteAsync(user);
                return ApiResult.Success(ResultCode.Success);
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="id">要修改的用户ID</param>
        /// <param name="dto">要修改的用户信息</param>
        /// <returns>修改后的用户信息</returns>
        public async Task<ApiResult<UserInfoDto>> UpdateUserInfo(Guid id, CreateOrUpdateUserInfoDto dto)
        {
            try
            {
                var user = await userRep.GetAsync(id);
                if (user == null)
                {
                    return ApiResult<UserInfoDto>.Fail("未找到要修改的用户", ResultCode.NotFound);
                }
                ObjectMapper.Map(dto, user);
                var updated = await userRep.UpdateAsync(user);
                return ApiResult<UserInfoDto>.Success(ResultCode.Success, ObjectMapper.Map<UserInfo, UserInfoDto>(updated));
            }
            catch (Exception ex)
            {
                return ApiResult<UserInfoDto>.Fail(ex.Message, ResultCode.Fail);
            }
        }
    }
}

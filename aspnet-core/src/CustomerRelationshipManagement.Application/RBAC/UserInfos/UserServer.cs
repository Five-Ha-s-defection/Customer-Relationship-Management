using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBAC.Permissions;
using CustomerRelationshipManagement.RBAC.RolePermissions;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.UserPermissions;
using CustomerRelationshipManagement.RBAC.UserRoles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IRepository<UserRoleInfo, Guid> userRoleRepo;
        private readonly IRepository<RoleInfo, Guid> roleRepo;
        private readonly IRepository<RolePermissionInfo, Guid> rolePermissionRepo;
        private readonly IRepository<PermissionInfo, Guid> permissionRepo;
        private readonly IRepository<UserPermissionInfo, Guid> userPermissionRepo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRep"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="currentUser"></param>
        public UserServer(IRepository<UserInfo, Guid> userRep, IPasswordHasher<UserInfo> passwordHasher, ICurrentUser currentUser, IRepository<UserRoleInfo, Guid> userRoleRepo, IRepository<RoleInfo, Guid> roleRepo, IRepository<RolePermissionInfo, Guid> rolePermissionRepo, IRepository<PermissionInfo, Guid> permissionRepo, IRepository<UserPermissionInfo, Guid> userPermissionRepo)
        {
            this.userRep = userRep;
            this.passwordHasher = passwordHasher;
            this.currentUser = currentUser;
            this.userRoleRepo = userRoleRepo;
            this.roleRepo = roleRepo;
            this.rolePermissionRepo = rolePermissionRepo;
            this.permissionRepo = permissionRepo;
            this.userPermissionRepo = userPermissionRepo;
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
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
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
                // 通过用户id来查出我的用户信息
                var userInfo = await userRep.GetAsync(userId);
                //转换成query格式
                var userRoleQuery = await userRoleRepo.GetQueryableAsync();
                //通过用户id来筛选出我的角色信息
                var roleIds = await userRoleQuery
                    .Where(x => x.UserId == userId)
                    .Select(x => x.RoleId)
                    .ToListAsync();
                //通过角色id来获取角色名称
                var roleInfo = await roleRepo.GetQueryableAsync();
                var roleNames =await roleInfo.Where(x => roleIds.Contains(x.Id))
                   .Select(x => x.RoleName)
                   .ToListAsync();
                //用户权限的信息
                var userPermissionInfo = await userPermissionRepo.GetQueryableAsync();
                //通过用户信息来获取用户权限的id
                var userpermissionIds =await userPermissionInfo.Where(x => x.UserId == userId)
                .Select(x => x.PermissionId)
                .ToListAsync();



                // 获取角色权限的信息
                var rolePermissionInfo = await rolePermissionRepo.GetQueryableAsync();
                //通过角色信息来获取角色权限的id
                var  rolePermissionIds =await rolePermissionInfo.Where(x => roleIds.Contains(x.RoleId))
                .Select(x => x.PermissionId)
                .ToListAsync();
                // 获取所有权限的id通过用户权限，然后通过union来进行合并用户和角色的权限然后过滤其中一样的权限
                var allPermissionIds =  userpermissionIds.Union(rolePermissionIds).Distinct().ToList();

                //查询权限信息
                var permissionInfo = await permissionRepo.GetQueryableAsync();
                //查询权限的编码
                var permissionCodes = await permissionInfo.Where(x => allPermissionIds.Contains(x.Id))
                    .Select(x => x.PermissionCode)
                    .ToListAsync();

                //将数据存入dto
                 var userInfoDto = ObjectMapper.Map<UserInfo, UserInfoDto>(userInfo);
                return ApiResult<UserInfoDto>.Success(ResultCode.Success, userInfoDto);

               


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

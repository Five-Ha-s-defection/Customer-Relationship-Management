using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.RBACDtos.UserRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.RBAC.UserRoles
{
    [ApiExplorerSettings(GroupName = "v1")]
    [AllowAnonymous]
    public class UserRoleServices : ApplicationService, IUserRoleServices
    {
        /// <summary>
        /// 添加用户角色   
        /// </summary>
        private readonly IRepository<UserRoleInfo, Guid> userRoleRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userRoleRepository"></param>
        public UserRoleServices(IRepository<UserRoleInfo, Guid> userRoleRepository)
        {
            this.userRoleRepository = userRoleRepository;
        }
        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="createUserRoleDto"></param>
        /// <returns></returns>
        public async Task<ApiResult> AddUserRole(CreateUserRoleDto createUserRoleDto)
        {
            try
            {
                //启用事务
                using (var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                { 
                    try
                    {
                        var userRoleInfo = await userRoleRepository.GetListAsync(x => x.UserId == createUserRoleDto.UserId);
                        if (userRoleInfo.Count > 0)
                        {
                            //先删除原有的用户角色
                            //await userRoleRepository.DeleteAsync(x => x.UserId == createUserRoleDto.UserId);
                        }
                        
                        //添加新的用户角色
                        var list = createUserRoleDto.RoleIds.Select(id => new UserRoleInfo
                        {
                            UserId = createUserRoleDto.UserId,
                            RoleId = id
                        }).ToList();

                        await userRoleRepository.InsertManyAsync(list);
                        tran.Complete();
                        return ApiResult.Success(ResultCode.Success);
                    }catch(Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
               return ApiResult.Fail(ex.Message, ResultCode.Fail);
            }
        }
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<Guid>>> GetCreateUserRoleAsync(Guid userId)
        {
            try
            {
                 var list = await userRoleRepository.GetListAsync(x => x.UserId == userId);
                return ApiResult<List<Guid>>.Success(ResultCode.Success, list.Select(x => x.RoleId).ToList());
            }
            catch (Exception ex)
            {
              return ApiResult<List<Guid>>.Fail(ex.Message, ResultCode.Fail);
                
            }
        }
    }
}

using CustomerRelationshipManagement.RBACDtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    public interface IUserProfileManager:IDomainService
    {
        /// <summary>
        /// 构建用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserInfoDto> BuildUserProfileAsync(Guid userId);
    }
}

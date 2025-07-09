using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBAC.RefreshToken
{
    /// <summary>
    /// 定义刷新令牌存储的操作接口，用于抽象存储逻辑（如 Redis、数据库等）
    /// </summary>
    public interface IRefreshTokenStore
    {
        /// <summary>
        /// 保存刷新令牌到存储（如 Redis）
        /// </summary>
        /// <param name="refreshToken">刷新令牌字符串</param>
        /// <param name="userId">对应的用户 ID</param>
        /// <param name="expiresIn">过期时间</param>
        Task SaveRefreshTokenAsync(string refreshToken, string userId, TimeSpan expiresIn);

        /// <summary>
        /// 通过刷新令牌获取用户 ID
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        Task<string?> GetUserIdByRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// 删除刷新令牌（用户退出或刷新完成后删除）
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        Task DeleteRefreshTokenAsync(string refreshToken);
    }
}

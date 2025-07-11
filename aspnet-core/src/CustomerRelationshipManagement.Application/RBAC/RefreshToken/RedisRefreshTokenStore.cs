using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.RBAC.RefreshToken
{
    public class RedisRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IDatabase db;

        public RedisRefreshTokenStore(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
        }

        private string GetKey(string refreshToken) => $"refresh_token:{refreshToken}";

        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
           await db.KeyDeleteAsync(GetKey(refreshToken));
        }

        public async Task<string?> GetUserIdByRefreshTokenAsync(string refreshToken)
        {
           var userId = await db.StringGetAsync(GetKey(refreshToken));
            return userId.HasValue?userId.ToString():null;
        }

        public async Task SaveRefreshTokenAsync(string refreshToken, string userId, TimeSpan expiresIn)
        {
           await db.StringSetAsync(GetKey(refreshToken), userId, expiresIn);
        }
    }
}

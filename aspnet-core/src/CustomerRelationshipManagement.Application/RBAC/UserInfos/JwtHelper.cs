using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerRelationshipManagement.RBAC.UserInfos
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration configuration;

        public JwtHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GenerateToken(Guid userId, string userName)
        {
            IList<Claim> claims = new List<Claim> {
                                new Claim(JwtClaimTypes.Id, userId.ToString()),
                                new Claim(JwtClaimTypes.Name, userName)
                            };

            //JWT密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthServer:JwtBearer:SecurityKey"]));

            //算法
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //过期时间
            DateTime expires = DateTime.UtcNow.AddHours(30);

            //Payload负载
            var token = new JwtSecurityToken(
                issuer: configuration["AuthServer:JwtBearer:Issuer"],
                audience: configuration["AuthServer:JwtBearer:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: cred
                );

            var handler = new JwtSecurityTokenHandler();

            //生成令牌
            string jwt = handler.WriteToken(token);

            return jwt;
        }
    }
}

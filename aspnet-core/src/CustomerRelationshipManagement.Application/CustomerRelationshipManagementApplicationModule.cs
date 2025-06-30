using CustomerRelationshipManagement.RBAC.UserInfos;
using CustomerRelationshipManagement.RBAC.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace CustomerRelationshipManagement;
[DependsOn(
    typeof(CustomerRelationshipManagementDomainModule),
    typeof(CustomerRelationshipManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class CustomerRelationshipManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CustomerRelationshipManagementApplicationModule>();
        });
        // ✅ 注册验证码服务
        context.Services.AddCaptcha(context.Configuration);
        // ✅ 注册自定义密码加密器
        context.Services.AddScoped<IPasswordHasher<UserInfo>, PasswordHasher<UserInfo>>();

        context.Services.AddScoped<IJwtHelper, JwtHelper>();

        var configuration = context.Services.GetConfiguration();

        var redisConfig = configuration["Redis:Configuration"];
        context.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            return ConnectionMultiplexer.Connect(redisConfig);
        });
    }
}

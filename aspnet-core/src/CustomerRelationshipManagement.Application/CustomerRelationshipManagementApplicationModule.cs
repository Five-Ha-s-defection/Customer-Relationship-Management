using CustomerRelationshipManagement.DTOS.SparkAi;
using CustomerRelationshipManagement.DTOS.UploadDto;
using CustomerRelationshipManagement.RBAC.RefreshToken;
using CustomerRelationshipManagement.RBAC.UserInfos;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.Upload;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
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

        context.Services.AddScoped<IRefreshTokenStore,RedisRefreshTokenStore>();

        var configuration = context.Services.GetConfiguration();

        var redisConfig = configuration.GetValue<string>("Redis:Configuration");
        context.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            return ConnectionMultiplexer.Connect(redisConfig);
        });

        Configure<FileUploadOptions>(configuration.GetSection("FileUpload"));

        // 绑定 SparkAI 配置
        context.Services.Configure<SparkAiOptions>(configuration.GetSection("SparkAI"));


    }
}

using CustomerRelationshipManagement.RBAC.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

<<<<<<<< HEAD:aspnet-core/src/CustomerRelationshipManagement.Application/CustomerProcess/CustomerManagement/Clues/CustomerRelationshipManagementApplicationModule.cs
namespace CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Clues;
========
namespace CustomerRelationshipManagement.CustomerProcess.Clues;
>>>>>>>> DevBranth:aspnet-core/src/CustomerRelationshipManagement.Application/CustomerProcess/Clues/CustomerRelationshipManagementApplicationModule.cs

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
    }
}

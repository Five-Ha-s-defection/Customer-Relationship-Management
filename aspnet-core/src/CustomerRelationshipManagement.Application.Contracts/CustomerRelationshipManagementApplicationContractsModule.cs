using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.SettingManagement;

namespace CustomerRelationshipManagement;

[DependsOn(
    typeof(CustomerRelationshipManagementDomainSharedModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule)
)]
public class CustomerRelationshipManagementApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        CustomerRelationshipManagementDtoExtensions.Configure();
    }
}

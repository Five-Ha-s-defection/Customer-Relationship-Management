using Volo.Abp.Modularity;

namespace CustomerRelationshipManagement;

[DependsOn(
    typeof(CustomerRelationshipManagementApplicationModule),
    typeof(CustomerRelationshipManagementDomainTestModule)
)]
public class CustomerRelationshipManagementApplicationTestModule : AbpModule
{

}

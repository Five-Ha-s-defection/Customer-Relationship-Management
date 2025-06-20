using Volo.Abp.Modularity;

namespace CustomerRelationshipManagement;

[DependsOn(
    typeof(CustomerRelationshipManagementDomainModule),
    typeof(CustomerRelationshipManagementTestBaseModule)
)]
public class CustomerRelationshipManagementDomainTestModule : AbpModule
{

}

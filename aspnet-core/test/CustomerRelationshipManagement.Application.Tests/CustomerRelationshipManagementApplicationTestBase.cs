using Volo.Abp.Modularity;

namespace CustomerRelationshipManagement;

public abstract class CustomerRelationshipManagementApplicationTestBase<TStartupModule> : CustomerRelationshipManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

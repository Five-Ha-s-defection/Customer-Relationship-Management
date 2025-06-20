using Volo.Abp.Modularity;

namespace CustomerRelationshipManagement;

/* Inherit from this class for your domain layer tests. */
public abstract class CustomerRelationshipManagementDomainTestBase<TStartupModule> : CustomerRelationshipManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

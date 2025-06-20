using CustomerRelationshipManagement.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CustomerRelationshipManagement.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CustomerRelationshipManagementEntityFrameworkCoreModule),
    typeof(CustomerRelationshipManagementApplicationContractsModule)
    )]
public class CustomerRelationshipManagementDbMigratorModule : AbpModule
{
}

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomerRelationshipManagement.Data;
using Volo.Abp.DependencyInjection;

namespace CustomerRelationshipManagement.EntityFrameworkCore;

public class EntityFrameworkCoreCustomerRelationshipManagementDbSchemaMigrator
    : ICustomerRelationshipManagementDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreCustomerRelationshipManagementDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the CustomerRelationshipManagementDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<CustomerRelationshipManagementDbContext>()
            .Database
            .MigrateAsync();
    }
}

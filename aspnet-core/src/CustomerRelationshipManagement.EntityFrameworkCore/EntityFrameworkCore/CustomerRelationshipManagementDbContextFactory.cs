using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Volo.Abp.Data;

namespace CustomerRelationshipManagement.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class CustomerRelationshipManagementDbContextFactory : IDesignTimeDbContextFactory<CustomerRelationshipManagementDbContext>
{
    public CustomerRelationshipManagementDbContext CreateDbContext(string[] args)
    {
        CustomerRelationshipManagementEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<CustomerRelationshipManagementDbContext>()
            .UseMySql(configuration.GetConnectionString("Default"),new MySqlServerVersion(new Version(8,0,42)), options =>
            {
                options.EnableStringComparisonTranslations(); // 示例其他扩展
                                                              // MySQL EF Core暂时不支持 EnablePrimitiveCollections
            });
        return new CustomerRelationshipManagementDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CustomerRelationshipManagement.HttpApi.Host/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}

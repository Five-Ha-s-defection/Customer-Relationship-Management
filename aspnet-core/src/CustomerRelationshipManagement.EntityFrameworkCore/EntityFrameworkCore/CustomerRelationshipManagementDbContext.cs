using CustomerRelationshipManagement.Finance;
using CustomerRelationshipManagement.Payments;
﻿using CustomerRelationshipManagement.ProductManagement;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace CustomerRelationshipManagement.EntityFrameworkCore;


[ConnectionStringName("Default")]
public class CustomerRelationshipManagementDbContext :
    AbpDbContext<CustomerRelationshipManagementDbContext>
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    /// <summary>
    /// 产品管理
    /// </summary>
    public DbSet<Product> Product { get; set; }
    /// <summary>
    /// 产品管理类型
    /// </summary>
    public DbSet<Category> Category { get; set; }

    public CustomerRelationshipManagementDbContext(DbContextOptions<CustomerRelationshipManagementDbContext> options)
        : base(options)
    {

    }

    public DbSet<Receivables> Receivables { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "YourEntities", CustomerRelationshipManagementConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<Payment>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Payments), CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.PaymentCode).IsRequired().HasMaxLength(128);
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        });

        builder.Entity<Receivables>(b =>
        {
            // 设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Receivables), CustomerRelationshipManagementConsts.DbSchema);
            // 按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
            // 配置 Name 属性为必填，最大长度 128
            b.Property(x => x.ReceivablePay).IsRequired().HasMaxLength(128);
        });
    }
}

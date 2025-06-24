using CustomerRelationshipManagement.Finance;
using CustomerRelationshipManagement.Invoices;
using CustomerRelationshipManagement.Payments;
using CustomerRelationshipManagement.RBAC.Menus;
using CustomerRelationshipManagement.RBAC.Permissions;
using CustomerRelationshipManagement.RBAC.RoleMenus;
using CustomerRelationshipManagement.RBAC.RolePermissions;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.UserPermissions;
using CustomerRelationshipManagement.RBAC.UserRoles;
using CustomerRelationshipManagement.RBAC.Users;
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

    public CustomerRelationshipManagementDbContext(DbContextOptions<CustomerRelationshipManagementDbContext> options)
        : base(options)
    {

    }

    public DbSet<Receivables> Receivables { get; set; }
    /// <summary>
    /// 收款
    /// </summary>
    public DbSet<Payment> Payment { get; set; }

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


        // 配置应收款单表
        builder.Entity<Receivables>(b =>
        {
            // 设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Receivables), CustomerRelationshipManagementConsts.DbSchema);
            // 按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
            // 配置 Name 属性为必填，最大长度 128
            b.Property(x => x.ReceivablePay).IsRequired().HasMaxLength(128);
        });
        // 配置收款方式
        builder.Entity<PaymentMethod>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(PaymentMethod), CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
        });
        // 配置收款
        builder.Entity<Payment>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Payments), CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.PaymentCode).IsRequired().HasMaxLength(128);
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        });
        // 配置发票表
        builder.Entity<Invoice>(b =>
        {
            // 设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Invoice), CustomerRelationshipManagementConsts.DbSchema);
            // 按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });
        // 配置用户信息表
        builder.Entity<UserInfo>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "UserInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            //设置密码为不限长度
            b.Property(x => x.Password).IsRequired().HasMaxLength(200);
            b.Property(x => x.Email).IsRequired().HasMaxLength(50);
            b.Property(x => x.PhoneInfo).IsRequired().HasMaxLength(50);
            b.Property(x => x.IsActive).IsRequired();

            //数据播种随机生成主键的guid
            b.HasData(new UserInfo
            {
                UserName = "admin",
                Password = "123",
                Email = "admin@admin.com",
                PhoneInfo = "12345678901",
                IsActive = true,

            });
        });
       
       
        //配置角色信息表
        builder.Entity<RoleInfo>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "RoleInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.RoleName).IsRequired().HasMaxLength(50);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.IsStatic).HasDefaultValue(false);
        });
        //配置权限表
        builder.Entity<PermissionInfo>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "PermissionInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.PermissionCode).IsRequired().HasMaxLength(50);
            b.Property(x => x.PermissionName).IsRequired().HasMaxLength(50);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x=>x.GroupName).IsRequired().HasMaxLength(50);

        });



        //配置角色权限表
        builder.Entity<RolePermissionInfo>(b =>
        { 
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "RolePermissionInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.RoleId).IsRequired();
            b.Property(x => x.PermissionId).IsRequired();
        });
        //配置角色菜单表
        builder.Entity<RoleMenuInfo>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "RoleMenuInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.RoleId).IsRequired();
            b.Property(x => x.MenuId).IsRequired();
       
        });
        //配置用户角色表
        builder.Entity<UserRoleInfo>(b =>
        { 
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "UserRoleInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.RoleId).IsRequired();
        });

        //配置用户权限表
        builder.Entity<UserPermissionInfo>(b =>
        { 
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "UserPermissionInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.PermissionId).IsRequired();
        });

        //配置角色菜单表
        builder.Entity<RoleMenuInfo>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "RoleMenuInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.RoleId).IsRequired();
            b.Property(x => x.MenuId).IsRequired();
        });
        //配置菜单表
        builder.Entity<MenuInfo>(b =>
        {
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + "MenuInfo", CustomerRelationshipManagementConsts.DbSchema);
            b.ConfigureByConvention();
            //上级菜单Id可以为空
            b.Property(x => x.ParentId).IsRequired(false);
            b.Property(x => x.MenuName).HasMaxLength(50);
            b.Property(x => x.Path).HasMaxLength(200);
            b.Property(x => x.Component).HasMaxLength(200);
            b.Property(x => x.Icon).HasMaxLength(50);
            b.Property(x => x.PermissionCode).HasMaxLength(50);
            b.Property(x=>x.IsVisible).HasDefaultValue(false);
            b.Property(x=>x.Sort).HasDefaultValue(0);

            
        });

        
    }
}

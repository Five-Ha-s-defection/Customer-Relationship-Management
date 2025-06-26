using CustomerRelationshipManagement.crmcontracts;
using CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys;
using CustomerRelationshipManagement.CustomerProcess.Cars;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.CustomerProcess.ClueSources;
using CustomerRelationshipManagement.CustomerProcess.ContactCommunications;
using CustomerRelationshipManagement.CustomerProcess.ContactRelations;
using CustomerRelationshipManagement.CustomerProcess.CustomerContacts;
using CustomerRelationshipManagement.CustomerProcess.CustomerLevels;
using CustomerRelationshipManagement.CustomerProcess.CustomerRegions;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CustomerProcess.CustomerTypes;
using CustomerRelationshipManagement.CustomerProcess.Industrys;
using CustomerRelationshipManagement.CustomerProcess.Prioritys;
using CustomerRelationshipManagement.CustomerProcess.SalesProgresses;
using CustomerRelationshipManagement.Finance.Invoices;
using CustomerRelationshipManagement.Finance.PaymentMethods;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivableses;
using CustomerRelationshipManagement.ProductCategory.Categorys;
using CustomerRelationshipManagement.ProductCategory.Products;
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

    # region 客户管理模块
    /// <summary>
    /// 线索表
    /// </summary>
    public DbSet<Clue> Clue { get; set; }

    /// <summary>
    /// 线索来源表
    /// </summary>
    public DbSet<ClueSource> ClueSource { get; set; }

    /// <summary>
    /// 行业表
    /// </summary>
    public DbSet<Industry> Industry { get; set; }

    /// <summary>
    /// 客户表
    /// </summary>
    public DbSet<Customer> Customer { get; set; }

    /// <summary>
    /// 车架号表
    /// </summary>
    public DbSet<CarFrameNumber> CarFrameNumber { get; set; }

    /// <summary>
    /// 客户级别表
    /// </summary>
    public DbSet<CustomerLevel> CustomerLevel { get; set; }

    /// <summary>
    /// 客户类型表
    /// </summary>
    public DbSet<CustomerType> CustomerType { get; set; }

    /// <summary>
    /// 客户地区表
    /// </summary>
    public DbSet<CustomerRegion> CustomerRegion { get; set; }

    /// <summary>
    /// 优先级表
    /// </summary>
    public DbSet<Priority> Priority { get; set; }

    /// <summary>
    /// 销售进度表
    /// </summary>
    public DbSet<SalesProgress> SalesProgress { get; set; }

    /// <summary>
    /// 联系人表
    /// </summary>
    public DbSet<CustomerContact> CustomerContact { get; set; }

    /// <summary>
    /// 联系人关系表
    /// </summary>
    public DbSet<ContactRelation> ContactRelation { get; set; }

    /// <summary>
    /// 商机表
    /// </summary>
    public DbSet<BusinessOpportunity> BusinessOpportunity { get; set; }
    #endregion

    #region 过程管理模块
    /// <summary>
    /// 联系记录表
    /// </summary>
    public DbSet<ContactCommunication> ContactCommunication { get; set; }
    #endregion

    #region 合同管理模块

    //合同表设计
    public DbSet<CrmContract> CrmContract { get; set; }

    //合同产品关系表
    public DbSet<CrmContractandProduct> CrmContractandProduct { get; set; }

    #endregion

    #region 财务管理模块
    /// <summary>
    /// 应收款
    /// </summary>
    public DbSet<Receivables> Receivables { get; set; }

    /// <summary>
    /// 收款
    /// </summary>
    public DbSet<Payment> Payment { get; set; }

    /// <summary>
    /// 发票实体类
    /// </summary>
    public DbSet<Invoice> Invoice { get; set; }

    /// <summary>
    /// 收款方式
    /// </summary>
    public DbSet<PaymentMethod> PaymentMethod { get; set; }
    #endregion

    #region 产品管理模块

    //产品分类
    public DbSet<Category> Category { get; set; }

    //产品管理模块
    public DbSet<Product> Product { get; set; }

    #endregion

    #region RBAC权限管理
    /// <summary>
    /// 用户信息
    /// </summary>
    public DbSet<UserInfo> UserInfo { get; set; }
    /// <summary>
    /// 角色信息
    /// </summary>
    public DbSet<RoleInfo> RoleInfo { get; set; }

    /// <summary>
    /// 权限信息
    /// </summary>
    public DbSet<PermissionInfo> PermissionInfo { get; set; }
    /// <summary>
    /// 角色权限信息
    /// </summary>
    public DbSet<RolePermissionInfo> RolePermissionInfo { get; set; }
    /// <summary>
    /// 角色菜单信息
    /// </summary>
    public DbSet<RoleMenuInfo> RoleMenuInfo { get; set; }
    /// <summary>
    /// 菜单信息
    /// </summary>
    public DbSet<MenuInfo> MenuInfo { get; set; }
    /// <summary>
    /// 用户权限信息
    /// </summary>
    public DbSet<UserPermissionInfo> UserPermissionInfo { get; set; }
    /// <summary>
    /// 用户角色信息
    /// </summary>
    public DbSet<UserRoleInfo> UserRoleInfo { get; set; }

    #endregion
    public CustomerRelationshipManagementDbContext(DbContextOptions<CustomerRelationshipManagementDbContext> options)
        : base(options)
    {

    }
   
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
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Payment), CustomerRelationshipManagementConsts.DbSchema);
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
            /*b.HasData(new UserInfo
            {
                UserName = "admin",
                Password = "123",
                Email = "admin@admin.com",
                PhoneInfo = "12345678901",
                IsActive = true,

            });*/
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

        //配置线索表
        builder.Entity<Clue>(b =>
        { 
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix +nameof(Clue),CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });

        //配置客户表
        builder.Entity<Customer>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Customer), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });


        //配置商机表
        builder.Entity<BusinessOpportunity>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(BusinessOpportunity), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });

        //配置联系人表
        builder.Entity<CustomerContact>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(CustomerContact), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });

        //配置联系记录表
        builder.Entity<ContactCommunication>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(ContactCommunication), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });

        //配置合同表
        builder.Entity<CrmContract>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(CrmContract), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });

        //配置合同产品关系表
        builder.Entity<CrmContractandProduct>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(CrmContractandProduct), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });



        //配置发票表
        builder.Entity<Invoice>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Invoice), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });

        //配置产品表
        builder.Entity<Product>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Product), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });

        //配置产品分类表
        builder.Entity<Category>(b =>
        {
            //设置表名和架构
            b.ToTable(CustomerRelationshipManagementConsts.DbTablePrefix + nameof(Category), CustomerRelationshipManagementConsts.DbSchema);
            //按约定自动配置基类属性（如主键、审计字段等）
            b.ConfigureByConvention();
        });
    }
}

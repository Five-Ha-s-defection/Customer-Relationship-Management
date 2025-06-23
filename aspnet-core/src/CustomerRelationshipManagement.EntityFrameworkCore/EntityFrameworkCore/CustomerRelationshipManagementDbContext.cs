using CustomerRelationshipManagement.BusinessOpportunitys;
using CustomerRelationshipManagement.Cards;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.ClueSources;
using CustomerRelationshipManagement.ContactCommunications;
using CustomerRelationshipManagement.ContactRelations;
using CustomerRelationshipManagement.CustomerContacts;
using CustomerRelationshipManagement.CustomerLevels;
using CustomerRelationshipManagement.CustomerRegions;
using CustomerRelationshipManagement.Customers;
using CustomerRelationshipManagement.CustomerTypes;
using CustomerRelationshipManagement.Industrys;
using CustomerRelationshipManagement.Prioritys;
using CustomerRelationshipManagement.SalesProgresses;
using Microsoft.EntityFrameworkCore;
﻿using CustomerRelationshipManagement.Menus;
using CustomerRelationshipManagement.Permissions;
using CustomerRelationshipManagement.RoleMenus;
using CustomerRelationshipManagement.RolePermissions;
using CustomerRelationshipManagement.Roles;
using CustomerRelationshipManagement.UserPermissions;
using CustomerRelationshipManagement.UserRoles;
using CustomerRelationshipManagement.Users;
using Microsoft.EntityFrameworkCore;
using System;
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

    //线索表
    public DbSet<Clue> Clue { get; set; }

    //线索来源表
    public DbSet<ClueSource> ClueSource { get; set; }

    //行业表
    public DbSet<Industry> Industry {  get; set; }

    //客户表
    public DbSet<Customer> Customer {  get; set; }

    //车架号表
    public DbSet<CarFrameNumber> CarFrameNumber { get; set; }

    //客户级别表
    public DbSet<CustomerLevel> CustomerLevel {  get; set; }

    //客户类型表
    public DbSet<CustomerType> CustomerType {  get; set; }

    //客户地区表
    public DbSet<CustomerRegion> CustomerRegion { get; set; }

    //优先级表
    public DbSet<Priority> Priority {  get; set; }

    //销售进度表
    public DbSet<SalesProgress> SalesProgress {  get; set; }

    //联系人表
    public DbSet<CustomerContact> CustomerContact { get; set; }

    //联系人关系表
    public DbSet<ContactRelation> ContactRelation {  get; set; }

    //商机表
    public DbSet<BusinessOpportunity> BusinessOpportunity { get; set; }

    //联系记录表
    public DbSet<ContactCommunication> ContactCommunication {  get; set; }

    public CustomerRelationshipManagementDbContext(DbContextOptions<CustomerRelationshipManagementDbContext> options)
        : base(options)
    {

    }
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

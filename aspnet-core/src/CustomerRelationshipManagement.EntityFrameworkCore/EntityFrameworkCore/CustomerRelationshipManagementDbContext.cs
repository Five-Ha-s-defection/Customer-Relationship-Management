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
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
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
    }
}

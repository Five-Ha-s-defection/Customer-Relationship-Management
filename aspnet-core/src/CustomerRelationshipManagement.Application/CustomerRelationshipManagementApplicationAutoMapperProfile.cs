using AutoMapper;
using CustomerRelationshipManagement.crmcontracts;
using CustomerRelationshipManagement.CustomerProcess.BusinessOpportunitys;
using CustomerRelationshipManagement.CustomerProcess.Cars;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.CustomerProcess.ClueSources;
using CustomerRelationshipManagement.CustomerProcess.CustomerContacts;
using CustomerRelationshipManagement.CustomerProcess.CustomerLevels;
using CustomerRelationshipManagement.CustomerProcess.CustomerRegions;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CustomerProcess.Industrys;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.BusinessOpportunitys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Cars;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerContacts;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.CustomerRegions;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Industrys;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Levels;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Sources;
using CustomerRelationshipManagement.DTOS.Finance.Incoices;
using CustomerRelationshipManagement.DTOS.Finance.Payments;
using CustomerRelationshipManagement.DTOS.Finance.Receibableses;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
//using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.Finance.Invoices;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivableses;
using CustomerRelationshipManagement.ProductCategory.Categorys;
using CustomerRelationshipManagement.ProductCategory.Products;
using CustomerRelationshipManagement.RBAC.Menus;
using CustomerRelationshipManagement.RBAC.Permissions;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Menus;
using CustomerRelationshipManagement.RBACDtos.Permissions;
using CustomerRelationshipManagement.RBACDtos.Roles;
using CustomerRelationshipManagement.RBACDtos.Users;


namespace CustomerRelationshipManagement;

public class CustomerRelationshipManagementApplicationAutoMapperProfile : Profile
{
    public CustomerRelationshipManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        //线索表
        CreateMap<Clue, ClueDto>().ReverseMap();
        CreateMap<Clue,CreateUpdateClueDto>().ReverseMap();

        //客户表
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Customer, CreateUpdateCustomerDto>().ReverseMap();

        //车架号表
        CreateMap<CarFrameNumber, CarDto>().ReverseMap();
        CreateMap<CarFrameNumber, CreateUpdateCarDto>().ReverseMap();

        //客户级别表
        CreateMap<CustomerLevel, LevelDto>().ReverseMap();
        CreateMap<CustomerLevel, CreateUpdateLevelDto>().ReverseMap();

        //线索/客户来源表
        CreateMap<ClueSource, SourceDto>().ReverseMap();
        CreateMap<ClueSource, CreateUpdateSourceDto>().ReverseMap();

        //线索行业表
        CreateMap<Industry, IndustryDto>().ReverseMap();
        CreateMap<Industry, CreateUpdateIndustryDto>().ReverseMap();

        //客户区域表
        CreateMap<CustomerRegion,RegionDto>().ReverseMap();
        CreateMap<CustomerRegion, CreateUpdateRegionDto>().ReverseMap();

        //商机表
        CreateMap<BusinessOpportunity, BusinessOpportunityDto>().ReverseMap();
        CreateMap<BusinessOpportunity, CreateUpdateBusinessOpportunityDto>().ReverseMap();

        //应收款表
        CreateMap<Receivables, ReceivablesDTO>().ReverseMap();
        CreateMap<CreateUpdateReceibablesDto, Receivables>().ReverseMap();

      


        #region RBAC
        //用户信息映射
        CreateMap<UserInfo, UserInfoDto>().ReverseMap();
        CreateMap<UserInfo, CreateOrUpdateUserInfoDto>().ReverseMap();
        //角色信息映射
        CreateMap<RoleInfo,RoleDto>().ReverseMap();
        CreateMap<CreateOrUpdateRoleDto,RoleInfo>().ReverseMap();
        //权限信息映射
        CreateMap<CreatePermissionDto, PermissionInfo>().ReverseMap();
        CreateMap<PermissionInfo, PermissionDto>().ReverseMap();
        //登录返回结果映射
        CreateMap<UserInfo, LoginResultDto>().ReverseMap();
        //菜单信息映射
        CreateMap<MenuInfo, MenuDto>().ReverseMap();
        CreateMap<CreateOrUpdateMenuDto, MenuInfo>().ReverseMap();



        #endregion

        CreateMap<Payment, PaymentDTO>().ReverseMap();
        CreateMap<CreateUpdatePaymentDTO, Payment>().ReverseMap();

        //发票表
        CreateMap<Invoice, InvoiceDTO>().ReverseMap();
        CreateMap<CreateUpdateInvoiceDto, Invoice>().ReverseMap();

        //用户信息映射
        CreateMap<UserInfo, UserInfoDto>().ReverseMap();
        CreateMap<UserInfo, CreateOrUpdateUserInfoDto>().ReverseMap();
        
        //产品管理显示Dto
        CreateMap<Product, ProductDtos>().ReverseMap();
        //产品管理添加修改Dto
        CreateMap<Product,CreateUpdateProductDtos>().ReverseMap();
        //产品分类显示Dto
        CreateMap<Category, CategoryDtos>().ReverseMap();
        //产品分类添加修改Dto
        CreateMap<Category, CreateUpdateCategoryDtos>().ReverseMap();

        //合同表
        CreateMap<AddCrmContractDto, CrmContract>().ReverseMap();
        CreateMap<CrmContract, ShowCrmContractDto>().ReverseMap();
        CreateMap<UpdateCrmContractDto, CrmContract>().ReverseMap();
        CreateMap<CrmContractandProduct,UpdateCrmcontractandProductDto>().ReverseMap();


        //联系人
        CreateMap<CustomerContactDto,CustomerContact>().ReverseMap();
        CreateMap<CreateUpdateCustomerContactDto, CustomerContact>().ReverseMap();
        CreateMap<SearchCustomerContactDto, CustomerContact>().ReverseMap();

        //联系人关系
        CreateMap<CustomerRegionDto, CustomerRegion >().ReverseMap();

    }
}

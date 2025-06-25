using AutoMapper;
using CustomerRelationshipManagement.CustomerProcess.Clues;
using CustomerRelationshipManagement.CustomerProcess.Customers;
using CustomerRelationshipManagement.CXS.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.CXS.ProductManagementDto;
using CustomerRelationshipManagement.Categorys;
using CustomerRelationshipManagement.Dtos.Users;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.DTOS.ProductManagementDto;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Clues;
using CustomerRelationshipManagement.DTOS.CustomerProcessDtos.Customers;
using CustomerRelationshipManagement.Finance;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivables;
using CustomerRelationshipManagement.Payments;
using CustomerRelationshipManagement.ProductCategory.Categorys;
using CustomerRelationshipManagement.ProductCategory.Products;
using CustomerRelationshipManagement.RBAC.Permissions;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.RBACDtos.Permissions;
using CustomerRelationshipManagement.RBACDtos.Roles;

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


        #endregion

        CreateMap<Payment, PaymentDTO>().ReverseMap();
        CreateMap<CreateUpdatePaymentDTO, Payment>().ReverseMap();
        
        
        //产品管理显示Dto
        CreateMap<Product, ProductDtos>().ReverseMap();
        //产品管理添加修改Dto
        CreateMap<Product,CreateUpdateProductDtos>().ReverseMap();
        //产品分类显示Dto
        CreateMap<Category, CategoryDtos>().ReverseMap();
        //产品分类添加修改Dto
        CreateMap<Category, CreateUpdateCategoryDtos>().ReverseMap();

    }
}

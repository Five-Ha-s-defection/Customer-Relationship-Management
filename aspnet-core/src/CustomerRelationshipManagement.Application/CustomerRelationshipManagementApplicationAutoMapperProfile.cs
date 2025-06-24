using AutoMapper;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Customers;
using CustomerRelationshipManagement.Dtos.Clues;
using CustomerRelationshipManagement.Dtos.Customers;
using CustomerRelationshipManagement.Dtos.Users;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.Finance;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivables;
using CustomerRelationshipManagement.Payments;
using CustomerRelationshipManagement.ProductManagement;
using CustomerRelationshipManagement.RBAC.Roles;
using CustomerRelationshipManagement.RBAC.Users;
using CustomerRelationshipManagement.Contracts;
using CustomerRelationshipManagement.CrmContracts;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;
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

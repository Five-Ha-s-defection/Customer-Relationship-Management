using AutoMapper;
using CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Clues.Dtos;
using CustomerRelationshipManagement.CustomerProcess.CustomerManagement.Customers.Dtos;
using CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Clues;
using CustomerRelationshipManagement.CustomerProcess.CustomerManegement.Customers;
using CustomerRelationshipManagement.Dtos.Users;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivableses;
using CustomerRelationshipManagement.FinanceInfo.Finance;
using CustomerRelationshipManagement.FinanceInfo.Payments;
using CustomerRelationshipManagement.ProductCategory.Categorys;
using CustomerRelationshipManagement.ProductCategory.Products;
using CustomerRelationshipManagement.ProductManagement;
using CustomerRelationshipManagement.RBAC.Users;

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



        CreateMap<Payment, PaymentDTO>().ReverseMap();
        CreateMap<CreateUpdatePaymentDTO, Payment>().ReverseMap();
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

    }
}

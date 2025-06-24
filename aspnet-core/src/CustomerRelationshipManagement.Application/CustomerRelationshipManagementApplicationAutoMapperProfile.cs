using AutoMapper;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Customers;
using CustomerRelationshipManagement.Dtos.Clues;
using CustomerRelationshipManagement.Dtos.Customers;
using CustomerRelationshipManagement.Finance;
using CustomerRelationshipManagement.Payments;
using CustomerRelationshipManagement.Dtos;
using CustomerRelationshipManagement.Users;
using CustomerRelationshipManagement.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.ProductManagement;

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

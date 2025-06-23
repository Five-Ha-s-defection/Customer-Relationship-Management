using AutoMapper;
using CustomerRelationshipManagement.Finance;
using CustomerRelationshipManagement.Payments;
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


        CreateMap<Receivables, ReceivablesDTO>().ReverseMap();
        CreateMap<CreateUpdateReceibablesDto, Receivables>().ReverseMap();



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

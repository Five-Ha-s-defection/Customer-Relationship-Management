using AutoMapper;
using CustomerRelationshipManagement.Categorys;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Customers;
using CustomerRelationshipManagement.CXS.DTOS.CategoryMangamentDto;
using CustomerRelationshipManagement.CXS.ProductManagementDto;
using CustomerRelationshipManagement.Dtos.Clues;
using CustomerRelationshipManagement.Dtos.Customers;
using CustomerRelationshipManagement.Dtos.Users;
using CustomerRelationshipManagement.Finance.Invoices;
using CustomerRelationshipManagement.Finance.Payments;
using CustomerRelationshipManagement.Finance.Receivables;
using CustomerRelationshipManagement.FinanceInfo.Finance;
using CustomerRelationshipManagement.FinanceInfo.Invoices;
using CustomerRelationshipManagement.FinanceInfo.Payments;
using CustomerRelationshipManagement.Products;
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

        //应收款表
        CreateMap<Receivables, ReceivablesDTO>().ReverseMap();
        CreateMap<CreateUpdateReceibablesDto, Receivables>().ReverseMap();

        //收款表
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

    }
}

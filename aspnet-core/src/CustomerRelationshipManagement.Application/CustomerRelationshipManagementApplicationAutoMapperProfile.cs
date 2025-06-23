using AutoMapper;
using CustomerRelationshipManagement.Clues;
using CustomerRelationshipManagement.Customers;
using CustomerRelationshipManagement.Dtos.Clues;
using CustomerRelationshipManagement.Dtos.Customers;
using CustomerRelationshipManagement.Dtos;
using CustomerRelationshipManagement.Users;

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
        //用户信息映射
        CreateMap<UserInfo, UserInfoDto>().ReverseMap();
    }
}

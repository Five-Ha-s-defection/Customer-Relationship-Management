using AutoMapper;
using CustomerRelationshipManagement.Contracts;
using CustomerRelationshipManagement.CrmContracts;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;

namespace CustomerRelationshipManagement;

public class CustomerRelationshipManagementApplicationAutoMapperProfile : Profile
{
    public CustomerRelationshipManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Contracts.CrmContract, AddUpdateCrmContractDto>().ReverseMap();
        CreateMap<Contracts.CrmContract, ShowCrmContractDto>().ReverseMap();
    }
}

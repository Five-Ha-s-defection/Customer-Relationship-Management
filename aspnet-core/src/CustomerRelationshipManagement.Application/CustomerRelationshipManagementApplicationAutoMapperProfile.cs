using AutoMapper;
using CustomerRelationshipManagement.Finance;

namespace CustomerRelationshipManagement;

public class CustomerRelationshipManagementApplicationAutoMapperProfile : Profile
{
    public CustomerRelationshipManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */


        CreateMap<Receivables, ReceivablesDTO>();
        CreateMap<CreateUpdateReceibablesDto, Receivables>();
    }
}

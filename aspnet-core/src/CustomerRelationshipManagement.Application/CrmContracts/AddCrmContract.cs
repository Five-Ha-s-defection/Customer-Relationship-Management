using CustomerRelationshipManagement.Contracts;
using CustomerRelationshipManagement.Dtos.CrmContractDtos;
using CustomerRelationshipManagement.ICrmContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CrmContracts
{
    public class AddCrmContract: ApplicationService,ICrmContractService
    {

    }
}

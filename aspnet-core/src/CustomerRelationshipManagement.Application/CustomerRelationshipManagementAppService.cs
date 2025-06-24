using System;
using System.Collections.Generic;
using System.Text;
using CustomerRelationshipManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Services;
using Volo.Abp.Modularity;

namespace CustomerRelationshipManagement;

/* Inherit your application services from this class.
 */
public abstract class CustomerRelationshipManagementAppService : ApplicationService
{
    protected CustomerRelationshipManagementAppService()
    {
        LocalizationResource = typeof(CustomerRelationshipManagementResource);
    }

}

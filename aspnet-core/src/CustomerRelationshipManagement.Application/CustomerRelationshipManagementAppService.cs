using System;
using System.Collections.Generic;
using System.Text;
using CustomerRelationshipManagement.Localization;
using Volo.Abp.Application.Services;

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

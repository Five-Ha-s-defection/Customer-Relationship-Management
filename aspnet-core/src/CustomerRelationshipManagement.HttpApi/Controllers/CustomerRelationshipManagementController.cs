using CustomerRelationshipManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace CustomerRelationshipManagement.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class CustomerRelationshipManagementController : AbpControllerBase
{
    protected CustomerRelationshipManagementController()
    {
        LocalizationResource = typeof(CustomerRelationshipManagementResource);
    }
}

using Microsoft.Extensions.Localization;
using CustomerRelationshipManagement.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace CustomerRelationshipManagement;

[Dependency(ReplaceServices = true)]
public class CustomerRelationshipManagementBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<CustomerRelationshipManagementResource> _localizer;

    public CustomerRelationshipManagementBrandingProvider(IStringLocalizer<CustomerRelationshipManagementResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}

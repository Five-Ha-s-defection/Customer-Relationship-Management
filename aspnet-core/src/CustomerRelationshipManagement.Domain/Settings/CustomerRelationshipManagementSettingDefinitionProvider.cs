using Volo.Abp.Settings;

namespace CustomerRelationshipManagement.Settings;

public class CustomerRelationshipManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(CustomerRelationshipManagementSettings.MySetting1));
    }
}

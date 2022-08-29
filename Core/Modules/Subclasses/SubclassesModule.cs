using Core.Features.Attribute;
using Core.Loader.Features;
using Core.Modules.Subclasses.Features;

namespace Core.Modules.Subclasses;

public class SubclassesModule : CoreModule<SubclassesConfig>
{
    public override string Name { get; } = "Subclasses";

    public static SubclassesConfig PluginConfig { get; private set; }
    public static SubclassesManager SubclassesManager { get; private set; }

    public override void OnEnabled()
    {
        PluginConfig = Config;

        SubclassesManager = new SubclassesManager();
        SubclassesManager.Load();

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        PluginConfig = null;

        SubclassesManager = null;
            
            
        base.OnDisabled();
    }
}
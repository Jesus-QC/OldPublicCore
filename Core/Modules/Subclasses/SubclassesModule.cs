using Core.Loader.Features;
using Core.Modules.Subclasses.Features;
using Exiled.API.Features;

namespace Core.Modules.Subclasses;

[DisabledModule]
public class SubclassesModule : CoreModule<SubclassesConfig>
{
    public override string Name { get; } = "Subclasses";

    public static SubclassesConfig PluginConfig { get; private set; }
    public static SubclassesManager SubclassesManager { get; private set; }

    public override void OnEnabled()
    {
        PluginConfig = Config;
            
        //Paths.Load();

        SubclassesManager = new SubclassesManager();
        //SubclassesManager.Load();

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
            
            
            
        base.OnDisabled();
    }
}
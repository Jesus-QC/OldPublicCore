using Core.Features.Attribute;
using Core.Loader.Features;
using Core.Modules.Subclasses.Features;
using Core.Modules.Subclasses.Handlers;

namespace Core.Modules.Subclasses;

public class SubclassesModule : CoreModule<SubclassesConfig>
{
    public override string Name { get; } = "Subclasses";

    public static SubclassesConfig PluginConfig { get; private set; }
    public static SubclassesManager SubclassesManager { get; private set; }

    private PlayerHandler _playerHandler;

    public override void OnEnabled()
    {
        PluginConfig = Config;
        
        SubclassesManager = new SubclassesManager();
        SubclassesManager.Load();

        _playerHandler = new PlayerHandler();
        Exiled.Events.Handlers.Player.ChangingRole += _playerHandler.OnChangingRole;
        Exiled.Events.Handlers.Player.Spawning += _playerHandler.OnSpawning;
        Exiled.Events.Handlers.Player.Hurting += _playerHandler.OnHurting;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        PluginConfig = null;

        SubclassesManager = null;
            
            
        base.OnDisabled();
    }
}
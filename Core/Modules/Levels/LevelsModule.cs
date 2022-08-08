using Core.Loader.Features;
using Core.Modules.Levels.Events;

namespace Core.Modules.Levels;

public class LevelsModule : CoreModule<LevelsConfig>
{
    public override string Name { get; } = "Levels";
    public override byte Priority { get; } = 5;

    public static LevelsConfig ModuleConfig;
        
    private PlayerHandler _playerHandler;
    private ServerHandler _serverHandler;
        
    public override void OnEnabled()
    {
        ModuleConfig = Config;
            
        _playerHandler = new PlayerHandler();
        _playerHandler.RegisterEvents();

        _serverHandler = new ServerHandler();
        _serverHandler.RegisterEvents();
        
        base.OnEnabled();
    }
    
    public override void OnDisabled()
    {
        _playerHandler.UnRegisterEvents();
        _playerHandler = null;

        _serverHandler.UnRegisterEvents();
        _serverHandler = null;

        ModuleConfig = null;
        
        base.OnEnabled();
    }
}
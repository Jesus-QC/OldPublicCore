using Core.Loader.Features;
using Core.Modules.Essentials.Handlers;

using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.Essentials;

public class EssentialsModule : CoreModule<EssentialsConfig>
{
    public override string Name => "Core.Essentials";
    public override string Prefix => "core_essentials";

    public static EssentialsConfig PluginConfig { get; private set; }

    private PlayerHandler _playerHandler;
    private ServerHandler _serverHandler;
        
    public override void OnEnabled()
    {
        PluginConfig = Config;

        _serverHandler = new ServerHandler();
        _playerHandler = new PlayerHandler();
            
        Server.RestartingRound += _serverHandler.OnRestartingRound;
        Server.WaitingForPlayers += _playerHandler.OnWaitingForPlayers;
            
        Player.Hurting += _playerHandler.OnHurting;
        Player.Verified += _playerHandler.OnVerified;
        Player.TriggeringTesla += _playerHandler.OnTriggeringTesla;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Player.Hurting -= _playerHandler.OnHurting;
        Player.Verified -= _playerHandler.OnVerified;
        Player.TriggeringTesla -= _playerHandler.OnTriggeringTesla;
        
        Server.RestartingRound -= _serverHandler.OnRestartingRound;
        Server.WaitingForPlayers -= _playerHandler.OnWaitingForPlayers;
        
        _playerHandler = null;
        _serverHandler = null;

        PluginConfig = null;

        base.OnDisabled();
    }
}